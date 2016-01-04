using System.Xml.Serialization;
using AndroCloudDataAccess.Domain;
using AndroCloudServices.Helper;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using AndroCloudHelper;

namespace AndroCloudServices.Domain
{
    [XmlRoot("Response")]
    [XmlInclude(typeof(SiteMenu))]
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "message")]
        [XmlAttribute("message", DataType = "string")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        [XmlAttribute("errorCode", DataType = "integer")]
        public int ErrorCode { get; set; }

        public static string Serialize(Error error, DataTypeEnum dataType)
        {
            // Build an error response
            ErrorResponse errorResponse = new ErrorResponse();
            errorResponse.Message = error.ErrorMessage;
            errorResponse.ErrorCode = error.ErrorCode;

            return SerializeHelper.Serialize<ErrorResponse>(errorResponse, dataType);
        }
    }
}
