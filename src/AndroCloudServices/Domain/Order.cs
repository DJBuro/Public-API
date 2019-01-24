using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Order")]
    public class Order
    {
        public string Type { get; set; }
        public DateTime OrderPlacedTime { get; set; }
        public DateTime OrderWantedTime { get; set; }
        public string OrderTimeType { get; set; }
        public Vouchers Vouchers { get; set; }
        public Pricing Pricing { get; set; }
        public CustomerAccount Customer { get; set; }
    }
}
