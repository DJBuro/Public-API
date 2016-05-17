using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.Models
{
    public class BringgTaskModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("scheduled_at")]
        public string ScheduledAt { get; set; }
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }
        [JsonProperty("team_id")]
        public int TeamId { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lan")]
        public double Lan { get; set; }
    }
}
