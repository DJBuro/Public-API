using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Template
{
    public class AddTemplateModel : Auth
    {
        [JsonProperty(PropertyName = "identity")]
        public string Identity { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }
    }
}