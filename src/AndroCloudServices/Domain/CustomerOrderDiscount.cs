using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Discount")]
    public class CustomerOrderDiscount
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
    }
}
