using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromedaDataAccess.Domain.Reporting.Query;
using System.Monads;
using MyAndromeda.Data.DataWarehouse;

namespace MyAndromeda.Web.Areas.Reporting.Services
{
    public interface IOrderService :  IDependency
    {
        /// <summary>
        /// Gets the order popularity.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<SummaryOfLineItem> GetOrderPopularity(FilterQuery filter);
        
        /// <summary>
        /// Get the max of the popularity.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        LineItemLimits GetMaxLimitsOnOrderPopularity(FilterQuery filter);

        /// <summary>
        /// Gets the list data.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IQueryable<OrderHeader> GetListData(FilterQuery filter);
        
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IEnumerable<SummaryByDay<decimal>> GetData(FilterQuery filter);

        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        OrdersSummary GetSummary(FilterQuery filter);
    }


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

        IEnumerable<SummaryByDay<decimal>> data; 
        public IEnumerable<SummaryByDay<decimal>> GetData(FilterQuery filter)
        {
            if (data != null)
                return data;

            Guid siteExternalId;
            IEnumerable<SummaryByDay<decimal>> ordersByDay = data; 

            var applicationIds = reportingContext.AcsApplicationIds;
            if(Guid.TryParse(reportingContext.ExternalId, out siteExternalId))
            {
                ordersByDay = orderDataService.GetTotalOrdersByDay(
                    e =>
                        e.ExternalSiteID == reportingContext.ExternalId 
                        && filter.FilterTo > e.TimeStamp 
                        && filter.FilterFrom < e.TimeStamp
                        //&& applicationIds.Any(id => id == e.ApplicationID)
                );

                return ordersByDay;
            }

            data = ordersByDay = orderDataService.GetTotalOrdersByDay(
                e => applicationIds.Any(id => id == e.ApplicationID)
            );

            return ordersByDay;
        }

        public IQueryable<OrderHeader> GetListData(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            Guid siteExternalId;
            if (Guid.TryParse(reportingContext.ExternalId, out siteExternalId))
            {
                var orders = orderDataService.GetOrders(
                    e =>
                        e.ExternalSiteID == reportingContext.ExternalId
                        && filter.FilterTo > e.TimeStamp
                        && filter.FilterFrom < e.TimeStamp
                );

                return orders;
            }

            return Enumerable.Empty<OrderHeader>().AsQueryable();
        }

        public IEnumerable<SummaryOfLineItem> GetOrderPopularity(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            Guid siteExternalId;
            if (Guid.TryParse(reportingContext.ExternalId, out siteExternalId))
            {
                var orders = orderDataService
                    .GetOrderLineSummary(e => e.OrderHeader.ExternalSiteID == reportingContext.ExternalId
                        && e.OrderHeader.TimeStamp > filter.FilterFrom
                        && e.OrderHeader.TimeStamp < filter.FilterTo
                );

                return orders;
            }

            siteExternalId.Check(e=> e == default(Guid), e=> new ArgumentException("External Site Id"));

            return Enumerable.Empty<SummaryOfLineItem>().AsQueryable();
        }

        public LineItemLimits GetMaxLimitsOnOrderPopularity(FilterQuery filter)
        {
            filter.CheckNull("FilterQuery");

            Guid siteExternalId;
            if (!Guid.TryParse(reportingContext.ExternalId, out siteExternalId))
            {
                siteExternalId.Check(e => e == default(Guid), e => new ArgumentException("External Site Id"));
            }

            var limits = orderDataService.GetLimits(e =>
                e.OrderHeader.ExternalSiteID == reportingContext.ExternalId && 
                e.OrderHeader.TimeStamp >= filter.FilterFrom && 
                e.OrderHeader.TimeStamp <= filter.FilterTo
            );

            return limits;
        }

    }


}