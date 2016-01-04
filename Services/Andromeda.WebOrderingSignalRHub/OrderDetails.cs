using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrderingSignalRHub
{
    public class OrderDetails
    {
        public string OrderReference { get; set; }
        public string ClientConnectionId { get; set; }
        public bool IsCompleted { get; set; }
        public int? ErrorCode { get; set; }
        public DateTime AddedDateTime { get; set; }
        public string PaymentProvider { get; set; }

        public OrderDetails(string orderReference, string paymentProvider)
        {
            this.OrderReference = orderReference;
            this.ClientConnectionId = null;
            this.IsCompleted = false;
            this.ErrorCode = null;
            this.AddedDateTime = DateTime.Now;
            this.PaymentProvider = paymentProvider;
        }
    }
}