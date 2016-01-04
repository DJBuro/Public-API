using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    internal class CachedCustomer
    {
        public long CustomerId { get; set; }
        public DateTime LastPolledDateTime { get; set; }
        public Dictionary<Int64, CachedOrder> OrdersByOrderId { get; set; }

        public CachedCustomer()
        {
            this.OrdersByOrderId = new Dictionary<long, CachedOrder>();
        }
    }
}
