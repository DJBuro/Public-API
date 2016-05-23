using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.SendGridService.Models
{
    //public class MarketingEventModel
    //{
    //    public int AndromedaSiteId { get; set; }
        
    //    public string TemplateName { get; set; }
    //    public string Subject { get; set; }
    //    public bool EnableEmail { get; set; }
    //}

    public class MaketingContactModel 
    {
        public string name { get; set; }
        public string email { get; set; }
        public string replyto { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }
}
