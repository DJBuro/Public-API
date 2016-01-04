using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Andromeda.WebOrdering.Model
{
    public class Feedback
    {
        [JsonIgnore]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "feedbackCategory")]
        public int? FeedbackCategory { get; set; }

        [JsonProperty(PropertyName = "feedbackCategoryName")]
        public string FeedbackCategoryName { get; set; }

        [JsonProperty(PropertyName = "feedback")]
        public string FeedbackText { get; set; }

        [JsonProperty(PropertyName = "storeName")]
        public string StoreName { get; set; }
    }
}