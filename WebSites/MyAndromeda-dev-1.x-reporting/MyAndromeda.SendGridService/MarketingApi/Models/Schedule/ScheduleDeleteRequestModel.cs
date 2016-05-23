using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Schedule
{
    public class ScheduleDeleteRequestModel : Auth
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}