using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Deal")]
    public class CustomerOrderDeal
    {
        public int MenuId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public List<CustomerDealLine> DealLines { get; set; }
    }
}
