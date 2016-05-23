using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models
{
    public class Auth : IAuth
    {
        [JsonProperty(PropertyName = "api_user")]
        public string ApiUser { get; internal set; }

        [JsonProperty(PropertyName = "api_key")]
        public string ApiKey { get; internal set; }
    }

    public interface IAuth 
    {
        /// <summary>
        /// Added automatically 
        /// </summary>
        [JsonProperty(PropertyName = "api_user")]
        string ApiUser { get; }

        /// <summary>
        /// Added automatically 
        /// </summary>
        [JsonProperty(PropertyName = "api_key")]
        string ApiKey { get; }
    }
}