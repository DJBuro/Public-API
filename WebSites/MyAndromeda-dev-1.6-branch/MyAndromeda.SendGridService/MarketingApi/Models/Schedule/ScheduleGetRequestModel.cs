using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Schedule
{
    public class ScheduleGetRequestModel : Auth 
    {
        [JsonProperty("name")]
        public string TemplateName { get; set; }
    }
}