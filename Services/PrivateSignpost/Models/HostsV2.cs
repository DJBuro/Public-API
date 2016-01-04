using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;

namespace PrivateSignpost.Models
{
    [XmlRoot(ElementName="Hosts")]
    public class HostsV2 : List<HostV2>
    {
    }
}
