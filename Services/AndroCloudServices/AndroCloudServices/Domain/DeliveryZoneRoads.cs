using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName="DeliveryZoneRoads")]
    public class DeliveryZoneRoads : List<DeliveryZoneRoad>
    {
        public DeliveryZoneRoads(List<DeliveryZoneRoad> list) : base(list)
        {
        }
    }
}
