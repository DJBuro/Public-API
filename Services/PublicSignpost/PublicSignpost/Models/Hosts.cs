using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;

namespace PublicSignpost.Models
{
    [XmlRoot(ElementName="Hosts")]
    public class Hosts : List<Host>
    {
    }
}
