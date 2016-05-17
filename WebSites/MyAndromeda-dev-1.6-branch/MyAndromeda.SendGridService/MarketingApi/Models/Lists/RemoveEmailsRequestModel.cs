using MyAndromeda.SendGridService.Attributes;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Lists
{
    public class RemoveEmailsRequestModel : Auth 
    {
        [JsonProperty(PropertyName = "list")]
        public string ListName { get; set; }

        [SendGridArray("email[]")]
        [JsonProperty("email[]")]
        public string[] Emails { get; set; }
    }
    public class RemoveEmailsResponseModel 
    {
        [JsonProperty("removed")]
        public int Removed { get; set; }
    }
}