using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Categories
{
    public class CategoryResponseModel 
    {
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}