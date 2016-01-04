using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Categories
{
    public class CategoryCreateRequestModel : Auth
    {
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
