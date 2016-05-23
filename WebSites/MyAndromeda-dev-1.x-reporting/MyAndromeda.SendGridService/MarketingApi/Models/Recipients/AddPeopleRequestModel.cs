using MyAndromeda.SendGridService.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class AddPeopleRequestModel<TModel>: Auth
        where TModel : Person
    {
        [JsonProperty("list")]
        public string ListName { get; set; }

        //[JsonProperty("data")]
        [SendGridArray("data[]")]
        public List<TModel> Data { get; set; }
    }

}