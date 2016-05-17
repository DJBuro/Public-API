using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class AddPeopleResponseModel 
    {
        [JsonProperty("inserted")]
        public int Inserted { get; set; }
    }
}