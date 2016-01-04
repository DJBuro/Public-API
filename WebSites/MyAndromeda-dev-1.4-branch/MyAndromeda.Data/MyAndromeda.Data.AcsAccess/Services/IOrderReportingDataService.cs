using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsOrders.Model;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromedaDataAccess.Domain.Reporting;

namespace MyAndromeda.Data.AcsOrders.Services
{
    public interface IOrderReportingDataService : IDependency
    {
        /// <summary>
        /// Queries the Order Header table.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IQueryable<OrderHeader> Query(Expression<Func<OrderHeader, bool>> query);

        /// <summary>
        /// Gets the total orders by day in a summary mode - avg, min, max computed sql side.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<SummaryByDay<decimal>> GetTotalOrdersByDay(Expression<Func<OrderHeader, bool>> query);

        /// <summary>
        /// Gets the order line summary. Counting order counts of items
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IEnumerable<SummaryOfLineItem> GetOrderLineSummary(Expression<Func<OrderLine, bool>> query);

        /// <summary>
        /// Gets the order line max. I want to know the highest selling item for a maximum value on a chart
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        int GetOrderLineMax(Expression<Func<OrderLine, bool>> query);

        /// <summary>
        /// Gets the limits.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        LineItemLimits GetLimits(Expression<Func<OrderLine, bool>> query);

        /// <summary>
        /// Gets the orders.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        IQueryable<OrderHeader> GetOrders(Expression<Func<OrderHeader, bool>> query);
    }
}