using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Andromeda.WebOrdering.Model
{
    public class Site
    {
        public string siteId { get; set; }
        public string name { get; set; }
        public int menuVersion { get; set; }
        public bool isOpen { get; set; }
        public int estDelivTime { get; set; }
    }
}