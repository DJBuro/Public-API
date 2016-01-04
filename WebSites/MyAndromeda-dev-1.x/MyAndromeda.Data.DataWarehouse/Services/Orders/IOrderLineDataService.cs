using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface IOrderLineDataService : IDependency
    {
        /// <summary>
        /// Gets the ordered items.
        /// </summary>
        /// <param name="acsOrderId">The acs order id.</param>
        /// <returns></returns>
        IEnumerable<OrderLine> GetOrderedItems(Guid acsOrderId);
    }

    public static class OrderLineExtensions 
    {
        public static decimal GetCombinedPrice(this OrderLine model) 
        {
            var itemPrice = model.Price.GetValueOrDefault(0);
            var modifierPrices = model.modifiers.Sum(e => e.Price.GetValueOrDefault(0));

            var combinedPrice = ((decimal)(itemPrice + modifierPrices)) / 100 ;

            return combinedPrice;
        }
    }
}
