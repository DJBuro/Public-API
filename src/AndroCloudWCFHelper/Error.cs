using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AndroCloudHelper
{
    [XmlRoot("Error")]
    public class Error
    {
        [XmlElement(ElementName = "ErrorCode")]
        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; set; }

        [XmlElement(ElementName="Message")]
        [JsonProperty(PropertyName="message")]
        public string ErrorMessage { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public ResultEnum Result { get; set; }

        public Error()
        {
        }

        public Error (string errorMessage, int errorCode, ResultEnum result)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorCode = errorCode;
            this.Result = result;
        }
    }
}
