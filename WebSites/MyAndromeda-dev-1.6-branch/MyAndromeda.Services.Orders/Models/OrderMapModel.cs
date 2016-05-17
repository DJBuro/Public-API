using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Services.Orders.Models
{
    public class OrderMapModel
    {
        /// <summary>
        /// Gets or sets the store.
        /// </summary>
        /// <value>The store.</value>
        public Store Store { get; set; }

        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public OrderHeader[] Orders { get; set; }
    }
}
