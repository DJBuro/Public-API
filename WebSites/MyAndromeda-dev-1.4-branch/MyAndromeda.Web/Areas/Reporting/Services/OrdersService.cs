using System.Collections.Generic;
using System.Linq;
using System.Monads;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromedaDataAccess.Domain.Reporting.Query;

namespace MyAndromeda.Web.Areas.Reporting.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IReportingContext reportingContext;
        private readonly IOrderReportingDataService orderDataService;

        public OrdersService(IReportingContext reportingContext, IOrderReportingDataService orderDataService)
        {
            this.orderDataService = orderDataService;
            this.reportingContext = reportingContext;
        }

        public OrdersSummary GetSummary(FilterQuery filter)
        {
            var orderData = this.GetData(filter);
            var orderSummary = new OrdersSummary(orderData);

            return orderSummary;
        }

        public IEnumerable<SummaryByDay<decimal>> GetData(FilterQuery filter)
        {
            IEnumerable<SummaryByDay<decimal>> ordersByDay = 
                ordersByDay = orderDataService.GetTotalOrdersByDay(
                    e =>
                        e.ExternalSiteID == reportingContext.ExternalSiteId &&
                        filter.FilterTo > e.TimeStamp &&
                        filter.FilterFrom < e.TimeStamp);

            return ordersByDay;
        }

        public IQueryable<OrderHeader> GetListData(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            var orders = orderDataService.GetOrders(
                e =>
                    e.ExternalSiteID == reportingContext.ExternalSiteId &&
                    filter.FilterTo > e.TimeStamp &&
                    filter.FilterFrom < e.TimeStamp);

            return orders;
        }

        public IEnumerable<SummaryOfLineItem> GetOrderPopularity(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            //Guid siteExternalId;
            //if (Guid.TryParse(reportingContext.ExternalId, out siteExternalId))
            {
                var orders = orderDataService
                                             .GetOrderLineSummary(e => e.OrderHeader.ExternalSiteID == reportingContext.ExternalSiteId &&
                                                                       e.OrderHeader.TimeStamp > filter.FilterFrom &&
                                                                       e.OrderHeader.TimeStamp < filter.FilterTo);

                return orders;
            }

            //siteExternalId.Check(e=> e == default(Guid), e=> new ArgumentException("External Site Id"));

            return Enumerable.Empty<SummaryOfLineItem>().AsQueryable();
        }

        public LineItemLimits GetMaxLimitsOnOrderPopularity(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            var limits = orderDataService.GetLimits(e =>
                                                        e.OrderHeader.ExternalSiteID == reportingContext.ExternalSiteId &&
                                                        e.OrderHeader.TimeStamp >= filter.FilterFrom &&
                                                        e.OrderHeader.TimeStamp <= filter.FilterTo);

            return limits;
        }
    }
}