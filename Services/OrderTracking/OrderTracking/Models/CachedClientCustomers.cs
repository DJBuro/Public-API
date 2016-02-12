using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTracking.Models
{
    internal class CachedClientCustomers
    {
        public Dictionary<string, CachedCustomer> CustomersByCustomerCredentials { get; set; }

        public CachedClientCustomers()
        {
            this.CustomersByCustomerCredentials = new Dictionary<string, CachedCustomer>();
        }
    }
}