using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class StoreDetails
    {
        public string PaymentProvider { get; set; }
        public string PaymentClientId { get; set; }
        public string PaymentClientPassword { get; set; }
    }
}