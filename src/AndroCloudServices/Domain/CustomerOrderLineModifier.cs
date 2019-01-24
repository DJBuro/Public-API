using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Modifier")]
    public class CustomerOrderLineModifier
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public bool? Removed { get; set; }
        public int? Price { get; set; }
    }
}
