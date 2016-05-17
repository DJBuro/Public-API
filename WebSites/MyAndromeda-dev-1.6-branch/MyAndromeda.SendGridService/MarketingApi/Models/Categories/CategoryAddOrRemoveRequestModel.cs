using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Categories
{
    public class CategoryAddOrRemoveRequestModel : Auth 
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// The Marketing Email to which the categories will be added.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}