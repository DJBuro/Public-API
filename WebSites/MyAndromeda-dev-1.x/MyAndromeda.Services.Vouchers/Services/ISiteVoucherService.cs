using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Vouchers.Services
{
    public interface ISiteVoucherService : IDependency
    {
        Voucher New();
        Voucher Get(System.Guid Id);
        Voucher GetByExpression(System.Linq.Expressions.Expression<Func<Voucher, bool>> query);
        IQueryable<Voucher> List();
        void Create(Voucher voucher);
        void Update(Voucher voucher);
        bool Delete(Voucher voucher);
    }
}
