using Andromeda.WebOrdering.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.PaymentProviders
{
    public class PayPalOrderDetails
    {
        public string SiteId { get; set; }
        public string HostHeader { get; set; }
        public string PayToken { get; set; }
        public JObject OrderDetailsJson { get; set; }
        public string PayerId { get; set; }
        public DomainConfiguration DomainConfiguration { get; set; }
        public OrderDetails OrderDetails { get; set; }
        public string OrderId { get; set; }
    }
}