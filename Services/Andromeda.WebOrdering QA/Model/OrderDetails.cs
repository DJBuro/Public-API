using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Andromeda.WebOrdering.PaymentProviders;
using Newtonsoft.Json.Linq;

namespace Andromeda.WebOrdering.Model
{
    public class OrderDetails
    {
        public PaymentDetails PaymentDetails { get; set; }
        public DomainConfiguration DomainConfiguration { get; set; }
        public BrowserDetails BrowserDetails { get; set; }
        public string SiteId { get; set; }
        public string OrderId { get; set; }
        public string PaymentType { get; set; }
        public string Key { get; set; }
        public JObject OrderElement { get; set; }
        public JObject OrderRootElement { get; set; }
        public JObject PaymentDataElement { get; set; }
        public JArray Payments { get; set; }
        public Payment Payment { get; set; }
        public HttpStatusCode ReturnHttpStatus  { get; set; }
        public string ReturnJson { get; set; }
        public string PaymentProviderName { get; set; }
        public bool IsOnline { get; set; }
        public string Password { get; set; }
        public string MerchantId { get; set; }
        public double Amount { get; set; }
        public IPaymentProvider PaymentProvider { get; set; }
        public string OrderType { get; set; }
        public string OrderWantedTime { get; set; }
        public OrderAddress OrderAddress { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public string OneOffDirections { get; set; }
        public string ChefNotes { get; set; }
        public string CustomerTitle { get; set; }
        public string CustomerFirstName  { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmailAddress { get; set; }
    }
}