using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("OrderLine")]
    public class CustomerOrderLine
    {
        public int MenuId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ChefNotes { get; set; }
        public string Person { get; set; }
        public CustomerOrderLineModifiers Modifiers { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
    }
}
