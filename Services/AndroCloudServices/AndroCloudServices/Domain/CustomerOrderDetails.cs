using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName = "Order")]
    public class CustomerOrderDetails
    {
        public string Id { get; set; }
        public DateTime ForDateTime { get; set; }
        public int OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal PaymentCharge { get; set; }

        public CustomerOrderLines OrderLines { get; set; }
        public CustomerOrderDeals Deals { get; set; }
        public CustomerOrderDiscounts Discounts { get; set; }
    }
}
