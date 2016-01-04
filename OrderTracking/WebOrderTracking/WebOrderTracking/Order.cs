using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    public class Order
    {
        public Int64 OrderId { get; set; }
        public DateTime? OrderTakenDateTime { get; set; }
        public DateTime? OutForDeliveryDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public double? StoreLatitude { get; set; }
        public double? StoreLongitude { get; set; }
        public double? CustomerLatitude { get; set; }
        public double? CustomerLongitude { get; set; }
        public string PersonProcessing { get; set; }
        public Int64 Status { get; set; }
        public string TrackerName { get; set; }

        public Order(Int64 orderId,
            DateTime? orderTakenDateTime,
            DateTime? outForDeliveryDateTime,
            DateTime? completedDateTime,
            double? storeLatitude,
            double? storeLongitude,
            double? customerLatitude,
            double? customerLongitude,
            string personProcessing,
            Int64 status,
            string trackerName)
        {
            this.OrderId = orderId;
            this.OrderTakenDateTime = orderTakenDateTime;
            this.OutForDeliveryDateTime = outForDeliveryDateTime;
            this.CompletedDateTime = completedDateTime;
            this.StoreLatitude = storeLatitude;
            this.StoreLongitude = storeLongitude;
            this.CustomerLatitude = customerLatitude;
            this.CustomerLongitude = customerLongitude;
            this.PersonProcessing = personProcessing;
            this.Status = status;
            this.TrackerName = trackerName;
        }
    }
}
