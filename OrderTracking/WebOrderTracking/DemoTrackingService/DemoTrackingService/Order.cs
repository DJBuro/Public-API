using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoTrackingService
{
    public class Order
    {
        //public DateTime? OrderTakenDateTime { get; set; }
        //public DateTime? OutForDeliveryDateTime { get; set; }
        //public DateTime? CompletedDateTime { get; set; }
        public string StoreLatitude { get; set; }
        public string StoreLongitude { get; set; }
        public string CustomerLatitude { get; set; }
        public string CustomerLongitude { get; set; }
        public string TrackerLatitude { get; set; }
        public string TrackerLongitude { get; set; }
        public string PersonProcessing { get; set; }
        public string Status { get; set; }

        public Order(
            //DateTime? orderTakenDateTime,
            //DateTime? outForDeliveryDateTime,
            //DateTime? completedDateTime,
            string storeLatitude,
            string storeLongitude,
            string customerLatitude,
            string customerLongitude,
            string trackerLatitude,
            string trackerLongitude,
            string personProcessing,
            string status)
        {
            //this.OrderTakenDateTime = orderTakenDateTime;
            //this.OutForDeliveryDateTime = outForDeliveryDateTime;
            //this.CompletedDateTime = completedDateTime;
            this.StoreLatitude = storeLatitude;
            this.StoreLongitude = storeLongitude;
            this.CustomerLatitude = customerLatitude;
            this.CustomerLongitude = customerLongitude;
            this.TrackerLatitude = trackerLatitude;
            this.TrackerLongitude = trackerLongitude;
            this.PersonProcessing = personProcessing;
            this.Status = status;
        }
    }
}