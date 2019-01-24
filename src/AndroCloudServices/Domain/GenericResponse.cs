using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using System.Runtime.Serialization;

namespace AndroCloudServices.Domain
{
    [DataContract]
    [XmlRoot("Response")]
    public class GenericResponse
    {
        [DataMember(Name = "result")]
        [XmlAttribute("result", DataType = "string")]
        public string Result { get; set; }

        public GenericResponse()
        {
            this.Result = "OK";
        }
    }
}
