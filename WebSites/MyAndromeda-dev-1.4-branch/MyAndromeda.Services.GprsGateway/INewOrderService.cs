using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Services.GprsGateway
{
    public interface INewOrderService : IDependency
    {
        Task<IQueryable<OrderHeader>> ListActiveOrdersByAndromedaId(int andromedaSiteId); 
        
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

        public async Task<IQueryable<OrderHeader>> ListActiveOrdersByAndromedaId(int andromedaSiteId) 
        {
            IQueryable<OrderHeader> result = Enumerable.Empty<OrderHeader>().AsQueryable();

            var store = await this.storeDataService.List()
                .Where(e=> e.AndromedaSiteId == andromedaSiteId).SingleOrDefaultAsync();

            var acsApplicationIds = await store.ACSApplicationSites
                .AsQueryable()
                .Select(e=> e.ACSApplicationId)
                .ToArrayAsync();

            if(store == null){ return result; }

            
            var orders = this.customerOrderDataService.List()
                .Where(e=> e.ExternalSiteID == store.ExternalId && acsApplicationIds.Contains(e.ApplicationID));

            return orders;
        }
    }
}
