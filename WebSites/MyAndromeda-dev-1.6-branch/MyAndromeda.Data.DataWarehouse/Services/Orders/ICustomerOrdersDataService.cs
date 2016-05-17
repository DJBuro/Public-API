using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Collections.Generic;

namespace MyAndromeda.Data.DataWarehouse.Services.Orders
{
    public interface ICustomerOrdersDataService : IDataProvider<OrderHeader>, IDependency
    {
        /// <summary>
        /// Updates the specified order header.
        /// </summary>
        /// <param name="orderHeader">The order header.</param>
        void Update(OrderHeader orderHeader);

        /// <summary>
        /// Updates the specified order headers.
        /// </summary>
        /// <param name="orderHeaders">The order headers.</param>
        void Update(IEnumerable<OrderHeader> orderHeaders);
    }
}