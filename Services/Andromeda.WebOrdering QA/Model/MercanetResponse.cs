using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class MercanetResponse
    {
        public string Code { get; set; }
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public string ResponseCode { get; set; }
        public string Amount { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentTime { get; set; }
    }
}