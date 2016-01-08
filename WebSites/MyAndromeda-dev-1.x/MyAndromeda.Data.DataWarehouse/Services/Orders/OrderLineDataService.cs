using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public class OrderLineDataService : IOrderLineDataService 
    {
        private readonly DataWarehouseDbContext sharedDbContext;

        public OrderLineDataService(DataWarehouseDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public IQueryable<OrderLine> Query()
        {
            var table = this.sharedDbContext.Set<OrderLine>();

            return table;
        }
    }
}