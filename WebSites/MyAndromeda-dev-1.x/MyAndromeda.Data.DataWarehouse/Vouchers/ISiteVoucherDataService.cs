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
    public interface ISiteVoucherDataService : IDataProvider<SiteVoucher>, IDependency
    {
        SiteVoucher Get(System.Guid Id);
        void Create(SiteVoucher siteVoucher);
        void Update(SiteVoucher siteVoucher);
        bool Delete(SiteVoucher siteVoucher);
    }
}
