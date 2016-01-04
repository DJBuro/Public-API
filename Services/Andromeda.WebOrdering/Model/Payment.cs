using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class Payment
    {
        public string PaymentType { get; set; }
        public string Value { get; set; }
        public string AuthCode { get; set; }
        public string CVVStatus { get; set; }
        public string PaytypeName { get; set; }
        public string LastFourDigits { get; set; }
        public string PayProcessor { get; set; }
        public PSPSpecificDetails PSPSpecificDetails { get; set; }
        public string PaymentCharge { get; set; }
    }
}