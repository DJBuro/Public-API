using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

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

    public class NewOrderService : INewOrderService 
    {
        private readonly ICustomerOrdersDataService customerOrderDataService; 
        private readonly IStoreDataService storeDataService; 

        public NewOrderService(ICustomerOrdersDataService customerOrderDataService, IStoreDataService storeDataService)
        {
            this.storeDataService = storeDataService;
            this.customerOrderDataService = customerOrderDataService;
        }

        public async Task<IQueryable<OrderHeader>> ListActiveOrdersByStoreAsync(Store store) 
        {
            IQueryable<OrderHeader> result = Enumerable.Empty<OrderHeader>().AsQueryable();

            var acsApplicationIds = await store.ACSApplicationSites
                .AsQueryable()
                .Select(e=> e.ACSApplicationId)
                .ToArrayAsync();

            if(store == null){ return result; }

            var orders = this.customerOrderDataService.List()
                .Include(e=> e.Customer)
                .Include(e=> e.CustomerAddress)
                .Include(e=> e.OrderStatu)
                .Include(e=> e.OrderLines)
                .Include(e=> e.OrderPayments)
                .Where(e=> e.ExternalSiteID == store.ExternalId && acsApplicationIds.Contains(e.ApplicationID));

            return orders;
        }

        public IQueryable<OrderHeader> Query()
        {
            var orders = this.customerOrderDataService.List()
               .Include(e => e.Customer)
               .Include(e => e.CustomerAddress)
               .Include(e => e.OrderStatu)
               .Include(e => e.OrderLines)
               .Include(e => e.OrderPayments);

            return orders;
        }
    }
}
