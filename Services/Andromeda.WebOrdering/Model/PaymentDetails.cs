using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class PaymentDetails
    {
        public string Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string BrowserUserAgent { get; set; }
    }
}