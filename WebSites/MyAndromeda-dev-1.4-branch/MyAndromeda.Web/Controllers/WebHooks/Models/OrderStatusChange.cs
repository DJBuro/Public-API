using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Controllers.WebHooks.Models
{
    public class OrderStatusChange : IHook 
    {
        public int AndromedaSiteId
        {
            get;
            set;
        }

        public string ExternalOrderId 
        {
            get; set; 
        }

        public string Status { get; set; }
        public string StatusDescription { get; set; }

    }
}