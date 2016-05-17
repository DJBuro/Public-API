using MyAndromeda.Data.AcsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.AndroWebOrdering.Models
{
    public class AndroWebOrderingWebSiteModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public bool Enabled { get; set; }

        public int AcsApplicationId { get; set; }

        public string PreviewWebSite { get; set; }
        public string LiveDomainName { get; set; }

        public string[] Devices { get; set; }
        //public WebSiteConfigurations WebsiteConfigurations { set; get; }
        //public IList<ThemeSettings> AndroWebOrderingThemes { set; get; }
        //public MyAndromedaMenu Menu { get; set; }
    }

    public class StoreViewModel 
    {
        public int AndromedaSiteId { get; set; }
        public string StoreName { get; set; }
    }
}