using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AndroCloudServices.Domain
{
    [XmlRoot("Order")]
    public class OrderStatusUpdate
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
