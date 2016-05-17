using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MyAndromeda.SendGridService.MarketingApi.Models.Template
{
    public class EditTemplateModel : AddTemplateModel
    {
        [JsonProperty(PropertyName = "newname")]
        public string NewName { get { return this.Name; } }
    }
}
