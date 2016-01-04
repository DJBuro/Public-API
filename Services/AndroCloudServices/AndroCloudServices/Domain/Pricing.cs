using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName = "Pricing")]
    public class Pricing
    {
        public decimal priceBeforeDiscount { get; set; }
        public bool pricesIncludeTax { get; set; }
        public decimal priceAfterDiscount { get; set; }
    }
}
