using Newtonsoft.Json;

namespace MyAndromeda.Services.Bringg.IncomingWebHooks
{
    public class BringgWebhookWayPointUpdate 
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("scheduled_at")]
        public string ScheduledAt { get; set; }

        [JsonProperty("task_id")]
        public string TaskId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("eta")]
        public string Eta { get; set; }

        [JsonProperty("etl")]
        public string Etl { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("done")]
        public string Done { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lng")]
        public string Long { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }
    }
}