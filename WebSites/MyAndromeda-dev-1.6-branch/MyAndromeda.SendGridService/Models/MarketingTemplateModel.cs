using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.Models
{
    public class MarketingTemplateModel
    {
        public MarketingApi.Models.Template.GetResponseTemplateModel Template { get; set; }

        //public MarketingApi.Models.Recipients.ListRecipientListNames RecipientList { get; set; }
    }
}
