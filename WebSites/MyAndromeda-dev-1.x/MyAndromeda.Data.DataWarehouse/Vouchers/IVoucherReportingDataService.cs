using MyAndromeda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Domain.Reporting;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public interface IVoucherReportingDataService : IDependency
    {
        VoucherSummary GetTotalOrdersByCode(Guid voucherId);
    }
}
