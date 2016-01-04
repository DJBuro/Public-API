using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Schedule
{
    public class ScheduleGetResponseModel 
    {
        [JsonProperty("date")]
        public string Date { get;set; }
    }
}
