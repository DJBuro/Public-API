using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Lists
{
    public class DeleteRequestModel : Auth
    {
        [JsonProperty(PropertyName = "list")]
        public string ListName { get; set; }
    }
}