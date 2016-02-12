using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderTracking.Models
{
    public class PollOrder
    {
        public Int64 OrderId { get; set; }
        public Int64 Status { get; set; }
        public DateTime? OrderCompletedDateTime { get; set; }
        public string TrackerName { get; set; }

        public PollOrder(
            Int64 orderId,
            Int64 status,
            DateTime? orderCompletedDateTime,
            string trackerName)
        {
            this.OrderId = orderId;
            this.Status = status;
            this.OrderCompletedDateTime = orderCompletedDateTime;
            this.TrackerName = trackerName;
        }
    }
}