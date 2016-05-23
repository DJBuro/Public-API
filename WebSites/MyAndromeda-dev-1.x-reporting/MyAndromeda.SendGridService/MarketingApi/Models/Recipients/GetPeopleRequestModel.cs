using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class GetPeopleRequestModel : Auth
    {
        [JsonProperty("list")]
        public string ListName { get; set; }
    }
}