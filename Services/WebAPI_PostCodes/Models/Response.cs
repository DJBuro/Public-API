using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_PostCodes.Models
{
    [DataContract(Name = "Response")]
    public class Response
    {
        [DataMember(Name = "Status")]
        public string Status { get; set; }
        [DataMember(Name = "Message")]
        public string Message { get; set; }
        //[CollectionDataContract(Name = "PostcodeList")]
        [DataMember(Name = "PostcodeList")]
        public List<string> PostcodeList { get; set; }
    }
}