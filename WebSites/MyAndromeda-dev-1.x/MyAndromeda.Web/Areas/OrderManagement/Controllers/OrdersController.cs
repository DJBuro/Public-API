using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.OrderMonitoring.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Web.Areas.OrderManagement.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Data.DataWarehouse.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.Framework.Dates;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MyAndromeda.Web.Areas.OrderManagement.Controllers
{
    public class OrdersController : Controller
    {
        /* Utility services */
        private readonly IDateServices dateServices;

        private readonly IMyAndromedaLogger logger;
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;

        /* Context variables */
        private readonly IOrderMonitoringService orderMonitoringService;
        private readonly IOrderLineDataService orderLineDataService;

        private readonly ISiteAddressDataService siteAddressDataService;
        private readonly IOrderStatusDataService orderStatusDataService;
        private readonly ICurrentSite currentSite;

        /* Events */
        private readonly IOrderChangedEvent[] events;

        public OrdersController(IDateServices dateServices,
            IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            IOrderMonitoringService orderMonitoringService,
            ISiteAddressDataService siteAddressDataService,
            IOrderStatusDataService orderStatusDataService,
            ICurrentSite currentSite,
            IOrderChangedEvent[] events,
            IOrderLineDataService orderLineDataService,
            IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.orderLineDataService = orderLineDataService;
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

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewOrderManagement))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }

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

            var result = items.ToDataSourceResult(request, orderHeader => orderHeader.ToViewModel(this.dateServices));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadTemplate([DataSourceRequest]DataSourceRequest request, Guid id)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewOrderManagement))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }

            var orderHeader = orderMonitoringService.GetOrderById(id);

            var result = orderHeader.ToViewModel(this.dateServices).OrderLines.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> FindOrder(Guid id)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewOrderDetail))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }

            var dbItem = orderMonitoringService.GetOrderById(id);
            //var orderLines = dbItem.ToViewModel(this.dateServices).OrderLines;
            //var orderLineItems = orderLineDataService.GetOrderedItems(dbItem.ACSOrderId)
            //    .ToArray();
            var orderLineItems = await orderLineDataService.Query()
                .Where(e => e.OrderHeaderID == id)
                .ToListAsync();

            if (orderLineItems.Count == 0) 
            {
                orderLineItems.Add(new Data.DataWarehouse.Models.OrderLine() { 
                    Description = "Rameses Order",
                    Price = 0,
                    Qty = 0
                });
            }

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

            var voucherItems = dbItem.UsedVouchers.Select(e => new
            {
                e.Voucher.VoucherCode,
                e.Voucher.DiscountType,
                e.Voucher.DiscountValue
            }).ToArray();

            var loyaltyItems = dbItem.OrderLoyalties.Where(e=> e.redeemedPoints > 0).Select(e=> new {
                e.redeemedPoints,
                e.redeemedPointsValue, 
            }).ToArray();
            

            //lets ignore order discounts as they are not used. 
            //var saved = dbItem.FinalPrice - loyaltyItems.Sum(e=> e.redeemedPointsValue.GetValueOrDefault() / 100);
            var itemsTotal = dbItem.OrderLines
                .Sum(orderLine => orderLine.Price.GetValueOrDefault() + orderLine.modifiers.Sum(topping => topping.Price).GetValueOrDefault());
            
            var discountRowsVouchers = voucherItems.Select(e=> new DiscountRow{
                Description = string.Format("{0} {1}", e.VoucherCode, e.DiscountType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase) 
                    ? string.Format("({0:0}%)", 100 - e.DiscountValue) 
                    : ""),
                Value = e.DiscountType.Equals("Percentage", StringComparison.InvariantCultureIgnoreCase) 
                    ? Math.Floor (((100 - e.DiscountValue) * (itemsTotal)) / 100) 
                    : e.DiscountValue * 100
            });
            
            var discountRows = loyaltyItems.Select(e=> new DiscountRow {
                Description = string.Format("Loyalty ({0} points)", e.redeemedPoints.GetValueOrDefault()),
                Value = e.redeemedPointsValue.GetValueOrDefault() 
            }).Concat(discountRowsVouchers);


            var orderLineTotal = orderLineItems.Sum(e => e.GetCombinedPrice());
            var paymentLineChargeTotal = (decimal)dbItem.OrderPayments.Sum(e=>e.PaymentCharge.GetValueOrDefault()) / 100;
            var discountLineTotal = dbItem.OrderDiscounts.Sum(e => e.DiscountAmount);
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
                LineItems = orderLineItems.Where(e => !e.DealID.HasValue).Select(e => new
                {
                    e.Description,
                    Price = e.GetCombinedPrice(),
                    e.Qty,
                    e.Cat1,
                    e.Cat2,
                    Modifiers = e.modifiers.Select(modifier => new
                    {
                        modifier.Description,
                        modifier.Price,
                        modifier.Qty,
                        modifier.Removed
                    }),
                    Children = orderLineItems.Where(item => item.DealID == e.ID)
                                             .OrderBy(item => item.DealSequence)
                                             .Select(item => new { 
                        item.Cat1,
                        item.Cat2,
                        item.Description,
                        item.Qty,
                        Price = item.GetCombinedPrice(),
                        Modifiers = item.modifiers.Select(modifier => new
                        {
                            modifier.Description,
                            modifier.Price,
                            modifier.Qty,
                            modifier.Removed
                        })
                    })
                }),
                DiscoutLineItems = discountRows,
                OrderLineAggregate = new
                {
                    ItemsTotal = orderLineTotal,//orderLineItems.Sum(e => e.GetCombinedPrice()),
                    CalculatedTotal = orderLineTotal + paymentLineChargeTotal + dbItem.DeliveryCharge,
                    DiscountTotal = discountLineTotal / 100,
                    dbItem.TotalTax,
                    dbItem.DeliveryCharge,
                    CardCharge = paymentLineChargeTotal,
                    ChargeTotal = paymentLineChargeTotal + dbItem.DeliveryCharge,
                    dbItem.FinalPrice
                },
                Customer = new
                {
                    dbItem.Customer.FirstName,
                    dbItem.Customer.LastName,
                    Mobile = dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault() != null ?
                        dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 1).FirstOrDefault().Value : string.Empty,
                    Email = dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 0).FirstOrDefault() != null ?
                        dbItem.Customer.Contacts.Where(e => e.ContactTypeId == 0).FirstOrDefault().Value : string.Empty,
                    TableNumber = dbItem.TableNumber.GetValueOrDefault(),
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
                    OrderPlacedTime = dateServices.ConvertToLocalString(dbItem.OrderPlacedTime.GetValueOrDefault()),
                    OrderWantedTime = dateServices.ConvertToLocalString(dbItem.OrderWantedTime.GetValueOrDefault()),
                    OrderPlacedTimeLocalString = dateServices.ConvertToLocalString(dbItem.OrderPlacedTime.GetValueOrDefault()),
                    OrderWantedTimeLocalString = dateServices.ConvertToLocalString(dbItem.OrderWantedTime.GetValueOrDefault(dbItem.OrderPlacedTime.GetValueOrDefault()))
                },
                ReferenceIds = new
                {
                    dbItem.RamesesOrderNum,
                    dbItem.ExternalOrderRef,
                    dbItem.TicketNumber
                },
                Vouchers = dbItem.UsedVouchers.Select(e => new
                {
                    e.Voucher.VoucherCode,
                    e.Voucher.DiscountType,
                    e.Voucher.DiscountValue
                }),
                Loyalty = dbItem.OrderLoyalties.Select(e => new
                {
                    e.awardedPoints,
                    e.awardedPointsValue,
                    e.redeemedPoints,
                    e.redeemedPointsValue
                })
            };

            

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Update([DataSourceRequest]DataSourceRequest request, OrderHeaderViewModel model)
        {
            if (!this.authorizer.Authorize(UserPermissions.CreateOrEditOrderDetails))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }

            var dbItem = orderMonitoringService.GetOrderById(model.Id);
            var oldStatus = dbItem.OrderStatu;

            dbItem.Status = model.Status;
            dbItem.OrderStatu = this.orderStatusDataService.Get(x => x.Id == model.Status);

            this.orderMonitoringService.Update(dbItem);

            var andromedaSiteId = currentSite.AndromediaSiteId;
            foreach (var ev in this.events)
            {
                try
                {
                    await ev.OrderStatusChangedAsync(andromedaSiteId, dbItem, oldStatus);
                }
                catch (Exception e)
                {
                    this.logger.Error("Event failed: " +ev.Name);
                    this.logger.Error(e);
                    this.notifier.Error("Failed to complete the event: " + ev.Name);
                }
            }

            this.notifier.Success("Order status updated: " + model.OrderStatus.Description, true);

            //ActionResult actionResult = RedirectToAction("Update", new { Id = model.ID, edit = false });
            return Json(new[] { dbItem.ToViewModel(this.dateServices) }.ToDataSourceResult(request));
        }

        public ActionResult ReadOrderStatuses([DataSourceRequest] DataSourceRequest request)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewOrderManagement))
            {
                this.notifier.Error(translator.T("You do not have permissions to view Order Management"));
                return new HttpUnauthorizedResult();
            }

            var data = this.orderStatusDataService.List(e => true);
            var result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}