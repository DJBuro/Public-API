using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.Bringg.IncomingWebHooks
{
    public class BringWebhook
    {
        //{
        //  id:
        //  title: 
        //  note:
        //  customer_id: 
        //  user_id: 
        //  status: 
        //  scheduled_at: 
        //  company_id: 
        //  active_way_point_id: 
        //  extras: 
        //  way_points: 
        //  customer: 
        //  user: 
        //  late: 
        //  external_id:
        //}

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("task_id")]
        public int TaskId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("scheduled_at")]
        public string ScheduledAt { get; set; }

        [JsonProperty("company_id")]
        public string CompanyId { get; set; }

        //[JsonProperty("active_way_point_id")]
        //public string ActiveWayPointId { get; set; }

        //[JsonProperty("extras")]
        //public string Extras { get; set; }

        //[JsonProperty("way_points")]
        //public string WayPoints { get; set; }

        //[JsonProperty("customer")]
        //public string Customer { get; set; }

        //[JsonProperty("user")]
        //public string User { get; set; }

        //[JsonProperty("late")]
        //public string Late { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
    }
}
