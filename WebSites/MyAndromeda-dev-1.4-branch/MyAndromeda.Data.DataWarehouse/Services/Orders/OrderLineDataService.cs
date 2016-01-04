using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class OrderLineDataService : IOrderLineDataService 
    {
        private readonly DataWarehouseEntities sharedDbContext;

        public OrderLineDataService(DataWarehouseEntities sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }


        public IEnumerable<OrderLine> GetOrderedItems(Guid acsOrderId)
        {
            var table = this.sharedDbContext.OrderLines;
            var query = table.Where(e => e.OrderHeader.ACSOrderId == acsOrderId);
            var result = query.ToArray();

            return result;
        }
    }
}