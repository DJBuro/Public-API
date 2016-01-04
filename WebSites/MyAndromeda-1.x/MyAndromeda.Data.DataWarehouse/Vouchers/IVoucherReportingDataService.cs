using MyAndromeda.Core;
using MyAndromedaDataAccess.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public interface IVoucherReportingDataService : IDependency
    {
        VoucherSummary GetTotalOrdersByCode(Guid voucherId);
    }
}
