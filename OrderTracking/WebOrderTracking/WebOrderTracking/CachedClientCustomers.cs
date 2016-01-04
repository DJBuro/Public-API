using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    internal class CachedClientCustomers
    {
        public Dictionary<string, CachedCustomer> CustomersByCustomerCredentials { get; set; }

        public CachedClientCustomers ()
        {
            this.CustomersByCustomerCredentials = new Dictionary<string, CachedCustomer>();
        }
    }
}
