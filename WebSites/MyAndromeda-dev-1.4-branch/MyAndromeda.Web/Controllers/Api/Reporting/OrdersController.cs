using Kendo.Mvc.Extensions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Web.Areas.Reporting;
using MyAndromeda.Web.Areas.Reporting.Services;
using MyAndromeda.Web.Extensions;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using MyAndromeda.Data.DailyReporting.Services;

namespace MyAndromeda.Web.Controllers.Api.Reporting
{
    public class OrdersController : ApiController
    {
        private readonly IOrderService orderService;
        private readonly IDailyReportingSalesAggregateServices dailyReportingService;

        private readonly IAuthorizer authorizer;
        private readonly ITranslator translator;
        private readonly IDateServices dateServices;
        private readonly INotifier notifier;
        private readonly ICurrentSite currentSite;

        public OrdersController(IAuthorizer authorizer, ITranslator translator, IDateServices dateServices, IOrderService orderService, ICurrentSite currentSite, IDailyReportingSalesAggregateServices dailyReportingService)
        {
            this.dailyReportingService = dailyReportingService;
            this.currentSite = currentSite;
            this.orderService = orderService;
            this.dateServices = dateServices;
            this.authorizer = authorizer;
            this.translator = translator;
        }

        [HttpGet, HttpPost]
        [Route("api/Reporting/{chainId}/{externalSiteId}/Orders/list/from-{fromYear}-{fromMonth}-{fromDay}/to-{toYear}-{toMonth}-{toDay}", Name = "OrderHistoryList")]
        public DataSourceResult List(
            [ModelBinder(typeof(WebApiDataSourceRequestModelBinder))]DataSourceRequest request,
            [FromUri]int fromYear, [FromUri]int fromMonth, [FromUri]int fromDay, [FromUri]int toYear, [FromUri]int toMonth, [FromUri]int toDay)
        {
            if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
            {
                this.notifier.Notify(translator.T(Messages.NotAuthorizedForAction));

                throw new Exception("Not available");
            }

            var filter = new FilterQuery()
            {
                FilterFrom = dateServices.ConvertToUtcFromLocal(new DateTime(fromYear, fromMonth, fromDay)),
                FilterTo = dateServices.ConvertToUtcFromLocal(new DateTime(toYear, toMonth, toDay))
            };

            var data = orderService
                .GetListData(filter)
                .OrderByDescending(e => e.TimeStamp);

            //DataSourceRequest is expecting a flatter model, which doesn't mean i cant redirect it to the ORM model as to more effectively query the database.
            var remapDictionary = new StringDictionary() 
            { 
                { "FirstName","Customer.FirstName" },
                //{ "LastName", "Customer.LastName" }
            };

            request.ReMap(remapDictionary);
            var result = data.ToDataSourceResult(request, e => e.ToViewModel(this.dateServices));

            return result;
        }

        [HttpPost]
        [Route("api/Reporting/{chainId}/{externalSiteId}/Orders/Summary", Name = "RamesesSaleRangeOverview")]
        public object RangeData(FilterQuery filter)
        {
            var ids = new long[] { this.currentSite.AndromediaSiteId };
            var data = dailyReportingService
                .SalesByDay(ids, e => e.TheDate > filter.FilterFrom && e.TheDate < filter.FilterTo).ToList();

            //var aggregates = new {
            //    SumSales = data.Sum(e=> e.Combined.Sales),
            //    SumOrders = data.Sum(e => e.Combined.OrderCount),
            //    SumCancelled = data.Sum(e => e.Combined.Cancelled),
            //    AvgSales = data.Where(e=> e.Combined.Sales> 0).Average(e => e.Combined.Sales),
            //    AvgOrders = data.Average(e => e.Combined.OrderCount),
            //    AvgCancelled = data.Average(e => e.Combined.Cancelled.GetValueOrDefault()),
            //    SumDelivery = data.Sum(e=> e.Delivery.OrderCount),
            //    SumCollection = data.Sum(e=> e.Collection.OrderCount),
            //    SumInStore = data.Sum(e=> e.InStore.OrderCount),
            //    Less30Mins = data.Sum(e=> e.Performance.NumOrdersLT30Mins)
            //};

            return new
            {
                //Aggregates = aggregates,
                Data = data,
                Total = data.Count
            };
        }

    }
}