using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Template
{
    public class GetResponseTemplateModel : Auth
    {
        [JsonProperty(PropertyName = "can_edit")]
        public bool CanEdit { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "newsletter_id")]
        public int NewsletterId { get; set; }

        [JsonProperty(PropertyName = "total_recipients")]
        public string TotalRecipients { get; set; }

        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "date_schedule")]
        public string DateSchedule { get; set; }

        [JsonProperty(PropertyName = "identity")]
        public string Identity { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
    }
}