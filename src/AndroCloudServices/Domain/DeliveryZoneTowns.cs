using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName="DeliveryZoneTowns")]
    public class DeliveryZoneTowns : List<DeliveryZoneTown>
    {
        public DeliveryZoneTowns(List<DeliveryZoneTown> list) : base(list)
        {
        }
    }
}
