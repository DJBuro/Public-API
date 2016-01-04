using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.GPSIntegration.Model
{
    public class Order
    {
        public string Note { get; set; }
        public DateTime ScheduledAt { get; set; }
        public Address Address { get; set; }
        public string BringgTaskId { get; set; }
        public decimal TotalPrice { get; set; } // Everything - including delivery!
        public decimal DeliveryFee { get; set; }
        public string AndromedaOrderId { get; set; }
        public bool HasBeenPaid { get; set; }
    }
}
