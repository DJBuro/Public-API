using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName = "Vouchers")]
    public class Vouchers : List<Voucher>
    {
    }
}
