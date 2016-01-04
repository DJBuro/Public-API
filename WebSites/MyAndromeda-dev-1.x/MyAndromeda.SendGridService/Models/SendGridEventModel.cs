using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.Models
{
    public class SendGridEventModel
    {
        // Docs: https://sendgrid.com/docs/API_Reference/Webhooks/event.html
        public string Email { get; set; }
        public double Timestamp { get; set; }
        public int Uid { get; set; }
        public int Id { get; set; }

        [JsonProperty("sg_event_id")]
        public string SendgridEventId { get; set; }
        [JsonProperty("smtp-id")] 
        public string SmtpId { get; set; }
        [JsonProperty("sg_message_id")]
        public string SgMessageId { get; set; }
        [JsonProperty("event")] // event is protected keyword
        public string SendgridEvent { get; set; }
        [JsonProperty("type")]
        public string EventType { get; set; }
        public IList<string> Category { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }

        // Add your custom fields here
        /// <summary>
        /// Custom: Email Guid
        /// </summary>
        public string EmailId { get; set; } // this is a custom field sent by each of the emails
        /// <summary>
        /// Custom: AndromedaSiteId
        /// </summary>
        public int AndromedaSiteId { get; set; }

        /// <summary>
        /// Customer: Order Header Id - links back to orders. 
        /// </summary>
        public string OrderHeaderId { get; set; }

        private DateTime? timeStampUtc; 
        [JsonIgnore]
        public DateTime TimeStampUtc 
        {
            get 
            {
                if(timeStampUtc.HasValue)
                    return timeStampUtc.Value;

                timeStampUtc = UnixTimeStampToDateTime(this.Timestamp);

                return this.timeStampUtc.Value;
            }
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            
            return dtDateTime;
        }
    }
}
