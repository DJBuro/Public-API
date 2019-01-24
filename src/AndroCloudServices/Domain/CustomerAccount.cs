using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlType("Customer")]
    public class CustomerAccount
    {
        public string CustomerId { get; set; }
    }
}
