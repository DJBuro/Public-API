using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Services.Orders.Emails
{
    public class OrderWatchingEmail : Postal.Email
    {
        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public ICollection<Models.OrderMapModel> StoreOrderCollection { get; set; }

        public string EmailTo { get; set; }

        /// <summary>
        /// Gets or sets the date services factory.
        /// </summary>
        /// <value>The date services factory.</value>
        public DateServicesFactory DateServicesFactory { get; set; }
    }
}
