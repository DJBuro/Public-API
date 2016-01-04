using Kendo.Mvc.UI;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Services.Orders.OrderMonitoring.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using MyAndromeda.Web.Areas.OrderManagement.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.Framework.Dates;
using System.Collections.Generic;


namespace MyAndromeda.Web.Areas.OrderManagement.Controllers
{
    public class OrdersController : Controller
    {
        /* Utility services */
        private readonly IDateServices dateServices;

        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;

        /* Context variables */
        private readonly IOrderMonitoringService orderMonitoringService;
        private readonly ISiteAddressDataService siteAddressDataService;
        private readonly IOrderStatusDataService orderStatusDataService;
        private readonly ICurrentSite currentSite;

        /* Events */
        private readonly IOrderChangedEvent[] events;

        public OrdersController(
            IDateServices dateServices,
            IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            IOrderMonitoringService orderMonitoringService,
            ISiteAddressDataService siteAddressDataService,
            IOrderStatusDataService orderStatusDataService,
            ICurrentSite currentSite,
            IOrderChangedEvent[] events
            )
        {
            this.dateServices = dateServices;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
            this.orderMonitoringService = orderMonitoringService;
            this.siteAddressDataService = siteAddressDataService;
            this.orderStatusDataService = orderStatusDataService;
            this.currentSite = currentSite;
            this.events = events;
        }
        //
        // GET: /OrderManagement/Orders/
        public ActionResult Index()
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewOrderManagement))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }
            return View();
        }

        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            //var items = orderMonitoringService.GetOrders(24 * 60, -1);
            DateTime future = DateTime.UtcNow.AddHours(24);
            DateTime past = DateTime.UtcNow.AddHours(-36);

            //var localTime = this.dateServices.ConvertToLocalFromUtc(utcNow);

            int status = -1; // fetch all

            var items = orderMonitoringService
                .List(x => x.ExternalSiteID == this.currentSite.ExternalSiteId && (status < 0 || x.Status == status))
                .Where(order => order.OrderWantedTime > past && order.OrderWantedTime < future)
                .ToList().OrderBy(x => Math.Abs(((TimeSpan)(DateTime.UtcNow - x.OrderWantedTime)).Ticks))
                .ToList();

            var result = items.ToDataSourceResult(request, e => e.ToViewModel(this.dateServices));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadTemplate([DataSourceRequest]DataSourceRequest request, Guid id)
        {
            var dbItem = orderMonitoringService.GetOrderById(id);

            var result = dbItem.ToViewModel(this.dateServices).OrderLines.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FindOrder(Guid id)
        {
            var dbItem = orderMonitoringService.GetOrderById(id);
            var orderLines = dbItem.ToViewModel(this.dateServices).OrderLines;
            var storeAddress = this.siteAddressDataService.GetSiteAddressByExternalSiteId(dbItem.ExternalSiteID);

            if (dbItem.CustomerAddress == null)
            {
                dbItem.CustomerAddress = new Data.DataWarehouse.Models.CustomerAddress();
            }
            if (dbItem.UsedVouchers == null)
            {
                dbItem.UsedVouchers = new List<Data.DataWarehouse.Models.UsedVoucher>();
            }
            //if (dbItem.UsedVouchers != null) {
            //    dbItem.UsedVouchers.ToList().ForEach(f => f.Voucher.DiscountValue = (decimal)100 - f.Voucher.DiscountValue);
            //}

            var result = new
            {
                Header = new
                {
                    Name = dbItem.SiteName,
                    Address = new
                    {
                        storeAddress.RoadNum,
                        storeAddress.RoadName,
                        City = storeAddress.Town,
                        storeAddress.State,
                        ZipCode = storeAddress.Postcode,
                        Country = storeAddress.Country.CountryName
                    }
                },
                LineItems = orderLines.Select(e => new
                {
                    e.Description,
                    e.Price,
                    e.Qty,
                    e.Modifiers,
                    e.Cat1,
                    e.Cat2
                }),

                OrderLineAggregate = new
                {
                    Sum = orderLines.Sum(e => e.Price),
                    dbItem.TotalTax,
                    dbItem.DeliveryCharge,
                    dbItem.FinalPrice
                },

                Customer = new
                {
                    dbItem.Customer.FirstName,
                    dbItem.Customer.LastName,
                    Mobile = dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault() != null ?
                        dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault().Value : string.Empty,
                    Email = dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault() != null ?
                        dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault().Value : string.Empty,

                    Addresses = new
                    {
                        dbItem.CustomerAddress.RoadNum,
                        dbItem.CustomerAddress.RoadName,
                        dbItem.CustomerAddress.City,
                        dbItem.CustomerAddress.State,
                        dbItem.CustomerAddress.ZipCode,
                        dbItem.CustomerAddress.Country
                    }
                },
                Delivery = new
                {
                    dbItem.OrderType,
                    OrderPlacedTime = dbItem.OrderPlacedTime,
                    OrderWantedTime = dbItem.OrderWantedTime,
                    OrderPlacedTimeLocalString = dateServices.ConvertToLocalString(dbItem.OrderPlacedTime.GetValueOrDefault()),
                    OrderWantedTimeLocalString = dateServices.ConvertToLocalString(dbItem.OrderWantedTime.GetValueOrDefault(dbItem.OrderPlacedTime.GetValueOrDefault()))

                },
                ReferenceIds = new
                {
                    dbItem.RamesesOrderNum,
                    dbItem.ExternalOrderRef
                },
                Vouchers = dbItem.UsedVouchers.Select(e => new
                {
                    e.Voucher.VoucherCode,
                    e.Voucher.DiscountType,
                    e.Voucher.DiscountValue
                })
            };

            return Json(result);
        }

        [HttpPost]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, OrderHeaderViewModel model)
        {
            var dbItem = orderMonitoringService.GetOrderById(model.ID);
            var oldStatus = dbItem.OrderStatu;
            dbItem.Status = model.Status;
            dbItem.OrderStatu = this.orderStatusDataService.Get(x => x.Id == model.Status);
            this.orderMonitoringService.Update(dbItem);
            //this.orderStatusDataService.AddHistory(dbItem, oldStatus);
            foreach (var ev in this.events)
            {
                ev.OrderStatusChanged(dbItem, oldStatus);
            }

            //ActionResult actionResult = RedirectToAction("Update", new { Id = model.ID, edit = false });
            return Json(new[] { dbItem.ToViewModel(this.dateServices) }.ToDataSourceResult(request));
        }

        public ActionResult ReadOrderStatuses([DataSourceRequest] DataSourceRequest request)
        {
            var data = this.orderStatusDataService.List(e => true);
            var result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}