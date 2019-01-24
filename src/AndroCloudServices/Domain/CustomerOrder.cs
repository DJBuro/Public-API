using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Order")]
    public class CustomerOrder
    {
        public string Id { get; set; }
        public DateTime ForDateTime { get; set; }
        public int OrderStatus { get; set; }
    }
}
