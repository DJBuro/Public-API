using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Services.Orders.Models;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Services.Orders.Services
{
    public class OrderSupportService : IOrderSupportService
    {
        private readonly ICustomerOrdersDataService orderHeaderDataService;
        private readonly IStoreDataService storeDataService; 
        
        public OrderSupportService(ICustomerOrdersDataService orderHeaderDataService, IStoreDataService storeDataService)
        {
            this.storeDataService = storeDataService;
            this.orderHeaderDataService = orderHeaderDataService;
        }

        public void UpdateOrders(IEnumerable<Guid> ids)
        {
            IEnumerable<OrderHeader> orders = this.orderHeaderDataService.List()
                .Where(e => ids.Contains(e.ID))
                .ToArray();
            
            foreach (var order in orders) 
            {
                order.Status = 1000; 
            }

            this.orderHeaderDataService.Update(orders);
        }

        public IEnumerable<OrderMapModel> List(IEnumerable<Guid> ids) 
        {
            var orders = this.orderHeaderDataService.List()
                .Where(e=> ids.Contains(e.ID))
                .ToArray();

            var orderExternalStoreIds = orders.Select(e=> e.ExternalSiteID)
                .ToArray();

            var stores = this.storeDataService.List()
                .Where(e => orderExternalStoreIds.Contains(e.ExternalId))
                .ToArray();

            var storeOrders = stores.Select(e => new OrderMapModel
            {
                Store = e, 
                Orders = orders
                    .Where(order=> order.ExternalSiteID.Equals(e.ExternalId, StringComparison.InvariantCultureIgnoreCase))
                    .ToArray()
            })
            .ToArray();

            return storeOrders;
        }
    }
}
