using System;
using System.Linq;
using MyAndromedaDataAccess.Domain.Reporting;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting.Query;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderReportingService
    {
        OrdersSummaryDomainModel GetSummaryOfOrders(int siteId, FilterQuery filter);
    }
}
