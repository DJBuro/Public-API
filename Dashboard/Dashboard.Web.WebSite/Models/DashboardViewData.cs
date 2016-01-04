using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Dao.Domain;
using Dashboard.Web.Mvc;

namespace Dashboard.Web.WebSite.Models
{
    public class DashboardViewData : SiteViewData
    {
        public class DashboardBaseViewData : SiteViewData
        {
            public Site Site { get; set; }
            public Dash Dash { get; set; }
            public string Scroller { get; set; }
            public string HeadOfficeMessage { get; set; }
            public string ScrollTickets { get; set; }
            public string ScrollOPD { get; set; }
            public string ScrollOTD { get; set; }
        }    

        public class IndexViewData : DashboardBaseViewData
        {
           
 
        }

        public class HeadOfficeViewData : DashboardBaseViewData
        {
            public List<Region> Regions { get; set; }
            public List<IndicatorDefinition> IndicatorDefinitions { get; set; }
        }

        public class DisplayViewData : DashboardBaseViewData
        {
            public List<Region> Regions { get; set; }
            public Region Region { get; set; }

            //used in the Region area
            public List<IndicatorDefinition> IndicatorDefinitions { get; set;}
            public IndicatorDefinition IndicatorDefinition { get; set; }
            //public IEnumerable<SelectListItem> dropdownItems;
        }

        public class AdminViewData: DashboardBaseViewData
        {
            public IList<HeadOffice> HeadOffices { get; set; }
            public HeadOffice HeadOffice { get; set; }
            public Region Region { get; set; }
            public SitesRegion SitesRegion { get; set; }
            public IEnumerable<SelectListItem> RegionListItems;
        }

        public class AccountViewData : DashboardBaseViewData
        {
            public User User { get; set; }
        }

        public class ScrollerViewData : DashboardBaseViewData
        {
            public HeadOffice HeadOffice { get; set; }
        }
    }


}
