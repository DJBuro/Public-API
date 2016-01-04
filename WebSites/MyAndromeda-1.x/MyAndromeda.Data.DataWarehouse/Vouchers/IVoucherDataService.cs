using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Vouchers
{
    public interface IVoucherDataService : IDataProvider<Voucher>, IDependency
    {
        Voucher Create(Voucher voucher);
        void Update(Voucher voucher);
        bool Delete(Voucher voucher);
    }
}
