using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Recipients
{
    public class AddOrRemoveRequestModel : Auth
    {
        [JsonProperty("list")]
        public string RecipientListName { get; set; }

        [JsonProperty("name")]
        public string MarketingEmailName { get; set; }
    }

    public class GetRequestModel : Auth
    {
        [JsonProperty("name")]
        public string MarketingEmailName { get; set; }
    }

    public class ListMetaModel 
    {
        [JsonProperty("list")]
        public string ListName { get; set; }
    }

    public class ListRecipientListNames : List<ListMetaModel> { }
}
