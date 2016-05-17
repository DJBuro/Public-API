using System;
using System.Linq;

namespace MyAndromeda.Web.Areas.Menu.ViewModels
{
    public class DebugDatabaseMenuViewModel
    {
        public bool Available { get; set; }
        
        public string DbSiteId { get; set; }

        public string DbMasterSiteId { get; set; }
        
        public int MenuVersion { get; set; }

        public DateTime MenuVersionLastUpdated { get; set; }

        public string LastError { get; set; }

        public int TempMenuVersion { get; set; }

        public DateTime TempMenuVersionLastUpdated { get; set; }
        
        public string Message { get; set; }

        public string ConnectionString { get; set; }

        public int AndromedaSiteId { get; set; }
    }
}