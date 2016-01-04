using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Lists
{
    public class CreateListModel : Auth
    {
        [JsonProperty(PropertyName = "list")]
        public string ListName { get; set; }
    }
}
