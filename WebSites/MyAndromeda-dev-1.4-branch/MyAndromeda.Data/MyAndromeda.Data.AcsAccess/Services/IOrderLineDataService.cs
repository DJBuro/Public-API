using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.AcsOrders.Model;
using MyAndromeda.Core;

namespace MyAndromeda.Data.AcsOrders.Services
{
    public interface IOrderLineDataService : IDependency
    {
        /// <summary>
        /// Gets the ordered items.
        /// </summary>
        /// <param name="acsOrderId">The acs order id.</param>
        /// <returns></returns>
        IEnumerable<Model.OrderLine> GetOrderedItems(Guid acsOrderId);
    }
    
    public class OrderLineDataService : IOrderLineDataService 
    {
        private readonly IAcsOrdersDbContext sharedDbContext;

        public OrderLineDataService(IAcsOrdersDbContext sharedDbContext)
        {
            this.sharedDbContext = sharedDbContext;
        }

        public IEnumerable<OrderLine> GetOrderedItems(Guid acsOrderId)
        {
            var table = this.sharedDbContext.GetContext().OrderLines;
            var query = table.Where(e => e.OrderHeader.ACSOrderId == acsOrderId);
            var result = query.ToArray();

            return result;
        }

        
    }
}
