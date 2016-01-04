using System.Collections.Generic;
using System.Web.Mvc;
using WebDashboard.Dao.Domain;
using WebDashboard.Dao.Domain.Helpers;
using WebDashboard.Mvc;
using System;

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
            public User User { get; set; }
        }

        public class GlobalViewData : MasterPageViewData
        {
        }

        public class PageViewData : MasterPageViewData
        {
            public IList<Site> Sites { get; set; }
            public Site Site { get; set; }
            public IList<Indicator> Indicators { get; set; }
            public IList<SiteRank> SiteRanks { get; set; }
            public Indicator Indicator { get; set; }
            public IList<Region> Regions { get; set; }
            public Region Region { get; set; }
            public IList<User> Users { get; set; }
            public User EditedUser { get; set; }
            public Group Group { get; set; }
            public IList<HeadOffice> HeadOffices { get; set; }
            public HeadOffice HeadOffice { get; set; }
            public IList<UserRegion> UserRegions { get; set; }
            public SortedList<string, bool> AvailableDates { get; set; }
            public DateTime? ForDateTime { get; set; }

            public IEnumerable<SelectListItem> RegionListItems;
            public IEnumerable<SelectListItem> SiteTypeListItems;

            public ExecutiveGroupDashboard ExecutiveGroupDashboard;
            public ExecutiveCompanyDashboard ExecutiveCompanyDashboard;
            public ExecutiveRegionDashboard ExecutiveRegionDashboard;
            public ExecutiveStoreDashboard ExecutiveStoreDashboard;
            
            public bool IsCompanySites = false;
            public string CurrencySymbol = "";
        }
    }
}