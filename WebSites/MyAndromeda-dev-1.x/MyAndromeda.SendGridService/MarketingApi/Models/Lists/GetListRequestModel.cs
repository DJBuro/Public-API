using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Lists
{
    public class GetListRequestModel : Auth
    {
        [JsonProperty(PropertyName = "list")]
        public string ListName { get; set; }
    }
}