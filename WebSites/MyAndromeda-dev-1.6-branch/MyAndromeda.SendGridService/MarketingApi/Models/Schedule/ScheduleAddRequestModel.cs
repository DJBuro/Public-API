using System;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Schedule
{
    public class ScheduleAddRequestModel : Auth
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Iso8601
        /// </summary>
        [JsonProperty("at")]
        public string At 
        { 
            get 
            {
                if (!SendAtUtc.HasValue)
                    return null;

                return SendAtUtc.Value.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        [JsonIgnore]
        public DateTime? SendAtUtc { get; set; }
    }
}