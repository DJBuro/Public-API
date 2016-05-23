using MyAndromeda.Core;
using System;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public interface IVoucherReportingDataService : IDependency
    {
        VoucherSummary GetTotalOrdersByCode(Guid voucherId, DateTime? fromDate, DateTime? toDate);
    }
}
