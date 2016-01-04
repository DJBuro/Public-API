using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderHeaderDataService : IDependency
    {
        IDbSet<OrderHeader> OrderHeaders { get; }
        IDbSet<OrderStatusHistory> OrderStatusHistory { get; }

        OrderHeader GetByOrderId(Guid acsOrderId);

        Task<int> SaveChangesAsync();
    }
}
