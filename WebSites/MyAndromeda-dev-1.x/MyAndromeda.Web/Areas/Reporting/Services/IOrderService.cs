using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromeda.Data.DataWarehouse;

namespace MyAndromeda.Web.Areas.Reporting.Services
{
    public interface IOrderService : IDependency
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
        OrdersSummaryDomainModel GetSummary(FilterQuery filter);
    }


}