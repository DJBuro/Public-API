using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Areas.Reporting.Services;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using MyAndromeda.Web.Extensions;
using System.Collections.Specialized;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Contexts;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class OrdersController : Controller 
    {
        private readonly ICurrentSite currentSite;

        private readonly IAcsCustomerDataService acsCustomerDataService;
        private readonly ICustomerOrdersDataService customerOrderService;
        private readonly IOrderService orderService;
        //private readonly ISpreadsheetDocumentExport spreadsheetExport;
        private readonly ITranslator translator;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly IDateServices dateServices;

        public OrdersController(
            ICurrentSite currentSite, 
            IOrderService orderService, 
            //ISpreadsheetDocumentExport spreadsheetExport, 
            ITranslator translator, 
            INotifier notifier, 
            IAuthorizer authorizer, 
            IAcsCustomerDataService acsCustomerDataService, 
            ICustomerOrdersDataService customerOrderService, 
            IDateServices dateServices) 
        {
            this.currentSite = currentSite;
            this.customerOrderService = customerOrderService;
            this.acsCustomerDataService = acsCustomerDataService;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
            this.orderService = orderService;
            //this.spreadsheetExport = spreadsheetExport;
            this.dateServices = dateServices;
        }

        [ChildActionOnly]
        public ActionResult IndexChild(FilterQuery filter, [DataSourceRequest]DataSourceRequest request) 
        {
            var model = orderService.GetSummary(filter);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Daily(FilterQuery filter) 
        {
            var model = orderService.GetSummary(filter);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Week(FilterQuery filter)
        {
            var model = orderService.GetSummary(filter);

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Month(FilterQuery filter)
        {
            var model = orderService.GetSummary(filter);

            return PartialView(model);
        }

        public ActionResult PopularItems(FilterQuery filter) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedView));

                return new HttpUnauthorizedResult();
            }
            //var model = orderService.GetOrderPopularity(filter);

            var orderlimit = orderService.GetMaxLimitsOnOrderPopularity(filter);

            var vm = new ViewModels.PopularItemsViewModel() { 
                OrderQuantityMax = orderlimit.QuantitySoldInAllOrders.Max,
                OrderSumMax = orderlimit.SumPriceOfAllItemsInAllOrders.Max.DevideBy(100),
                Filter = filter
            };

            return PartialView(vm);
        }

        public ActionResult List(FilterQuery filter) 
        {
            return View(filter);
        }

        public ActionResult ReadCustomerHistory([DataSourceRequest] DataSourceRequest request, Guid? customerId, Guid? acsOrderId) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }

            IQueryable<OrderHeader> data;

            if (!customerId.HasValue)
            {
                var customer = this.acsCustomerDataService.GetCustomerByAcsOrderId(acsOrderId.GetValueOrDefault());
                
                data = this.customerOrderService.List()
                    .Where(e => e.CustomerID == customer.ID && e.ExternalSiteID == this.currentSite.ExternalSiteId);
            }
            else 
            {
                data = this.customerOrderService.List()
                    .Where(e => e.CustomerID == customerId.Value && e.ExternalSiteID == this.currentSite.ExternalSiteId);
            }

            var result = data.ToDataSourceResult(request, e => e.ToViewModel(dateServices));
            return Json(result);
        }

        [HttpPost]
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int fromYear, int fromMonth, int fromDay, int toYear, int toMonth, int toDay) 
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                return new HttpUnauthorizedResult();
            }

            var filter = new FilterQuery()
            {
                FilterFrom = dateServices.ConvertToUtcFromLocal(new DateTime(fromYear, fromMonth, fromDay)),
                FilterTo = dateServices.ConvertToUtcFromLocal(new DateTime(toYear, toMonth, toDay))
            };

            var data = orderService
                .GetListData(filter)
                .OrderByDescending(e=> e.TimeStamp);

            //DataSourceRequest is expecting a flatter model, which doesn't mean i cant redirect it to the ORM model as to more effectively query the database.
            var remapDictionary = new StringDictionary() 
            { 
                { "FirstName","Customer.FirstName" },
                //{ "LastName", "Customer.LastName" }
            };

            request.ReMap(remapDictionary);
            var result = data.ToDataSourceResult(request, e=> e.ToViewModel(dateServices));
            
            return Json(result);
        }

        public JsonResult ReadOrderSummary([DataSourceRequest] DataSourceRequest request, DateTime from, DateTime to, bool? dayOnly) 
        {
            FilterQuery filter = null;

            if (dayOnly.GetValueOrDefault()) 
            { 
                filter = new FilterQuery()
                {
                    FilterFrom = new DateTime(from.Year, from.Month, from.Day),
                    FilterTo = new DateTime(to.Year, to.Month, to.Day)
                };
            }

            var model = orderService.GetSummary(filter);

            return Json(model.OrderData.OrderBy(e=> e.Day));
        }

        public PartialViewResult ListPreviousOrdersTemplate() 
        {
            return PartialView("_List_PreviousOrdersTemplate");
        }

        public PartialViewResult OrderTicketTemplate()
        {
            return PartialView("_List_TicketTemplates");
        }
    }
}