using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Services.GprsGateway
{
    public interface INewOrderService : IDependency
    {
        /// <summary>
        /// Lists the active orders by store async.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        Task<IQueryable<OrderHeader>> ListActiveOrdersByStoreAsync(Store store);

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderHeader> Query();
    }

}
