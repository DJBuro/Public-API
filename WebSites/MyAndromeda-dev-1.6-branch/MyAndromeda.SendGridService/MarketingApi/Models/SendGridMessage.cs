using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models
{
    public class SendGridMessage 
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        [JsonIgnore]
        public bool IsSuccessful
        {
            get
            {
                return this.Message == "success";
            }
        }
    }
}