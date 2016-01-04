using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [DataContract]
    [XmlRoot("Response")]
    [XmlInclude(typeof(Site))]
    public class OrderResponse
    {
        [DataMember(Name = "result")]
        [XmlAttribute("result", DataType = "string")]
        public string Result { get; set; }

        [DataMember(Name = "order")]
        public Order Order { get; set; }
    }
}
