using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    public class Site
    {
        public SiteDetails Details { get; set; }
        public SiteMenu Menu { get; set; }
        public string DeliveryZones { get; set; }
    }
}
