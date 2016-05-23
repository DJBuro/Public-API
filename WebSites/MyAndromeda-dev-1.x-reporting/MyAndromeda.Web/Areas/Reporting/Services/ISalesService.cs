using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;
using MyAndromeda.Framework;
using MyAndromeda.Web.Areas.Reporting.Context;
using MyAndromedaDataAccess.Domain.Reporting;

namespace MyAndromeda.Web.Areas.Reporting.Services
{
    public interface ISalesService : IDependency
    {
        /// <summary>
        /// Gets the sales summary.
        /// </summary>
        /// <returns></returns>
        SalesSummmary GetSalesSummary(FilterQuery filter);
    }

    public class SalesService : ISalesService 
    {
        private readonly IReportingContext reportingContext;
        private readonly IOrderService orderService;

        public SalesService(IReportingContext reportingContext, IOrderService orderService)
        {
            this.orderService = orderService;
            this.reportingContext = reportingContext;
        }

        /// <summary>
        /// Gets the sales summary.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SalesSummmary GetSalesSummary(FilterQuery filter)
        {
            var data = this.orderService.GetData(filter);
            var summary = new SalesSummmary(data);

            return summary;
        }
    }
}