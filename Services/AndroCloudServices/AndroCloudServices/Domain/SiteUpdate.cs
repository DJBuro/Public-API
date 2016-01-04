using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AndroCloudServices.Domain
{
    [XmlRoot("Site")]
    public class SiteUpdate
    {
        [JsonProperty(PropertyName = "etd")]
        public int ETD { get; set; }
    }
}
