using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTracking.Models
{
    internal class CachedOrder
    {
        public long OrderId { get; set; }
        public long OrderStatusId { get; set; }
        public Tracker Tracker { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}