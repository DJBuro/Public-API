using System.Collections.Generic;
using System.Web.Mvc;
using WebDashboard.Dao.Domain;
using WebDashboard.Mvc;

namespace WebDashboard.Web.Models
{
    public class WebDashboardViewData : SiteViewData
    {
        /// <summary>
        /// Masterpage, all other pages inherit from this.
        /// </summary>
        public class MasterPageViewData : SiteViewData
        {
            public string ErrorMessage;
        }

        public class PageViewData : MasterPageViewData
        {
            public IList<HeadOffice> HeadOffices { get; set; }
            public IList<Site> Sites{ get; set; }
            public IList<Indicator> Indicators{ get; set; }
            public IList<User> Users{ get; set; }
            public IList<Region> Regions{ get; set; }
            public IList<Permission> Permissions{ get; set; }
            public HeadOffice HeadOffice { get; set; }
            public Site Site { get; set; }
            public Region Region { get; set; }
            public Indicator Indicator { get; set; }
            public User User{ get; set; }
            public IList<Log> Logs { get; set; }

            public IEnumerable<SelectListItem> RegionListItems;
            public IEnumerable<SelectListItem> HeadOfficeListItems;
            public IEnumerable<SelectListItem> SiteTypeListItems;
            public IEnumerable<SelectListItem> ValueTypeListItems;
            public IEnumerable<SelectListItem> DivisorTypeListItems;
            public IEnumerable<SelectListItem> IndicatorTypeListItems;
            public IEnumerable<SelectListItem> DefinitionListItems;
        }
    }
}
