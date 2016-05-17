using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Lists
{
    public class GetListResponseModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("list")]
        public string ListName { get; set; }
    }
}