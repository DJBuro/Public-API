using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;
using System.Collections;

namespace AndroCloudServices.Domain
{
    [XmlRoot(ElementName="Hosts")]
    public class HostsV2 : List<HostV2>
    {
    }
}
