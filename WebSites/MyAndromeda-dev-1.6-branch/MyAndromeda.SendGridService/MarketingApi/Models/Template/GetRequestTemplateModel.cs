using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Template
{
    public class GetRequestTemplateModel : Auth
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}