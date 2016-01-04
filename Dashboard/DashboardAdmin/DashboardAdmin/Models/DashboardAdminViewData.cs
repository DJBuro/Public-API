using System.Collections.Generic;
using System.Web.Mvc;
using Dashboard.Dao.Domain;
using Dashboard.Web.Mvc;

namespace DashboardAdmin.Models
{
    public class DashboardAdminViewData : SiteViewData
    {
        /// <summary>
        /// Masterpage, all other pages inherit from this.
        /// </summary>
        public class DashboardAdminBaseViewData : SiteViewData
        {
            public string ErrorMessage;
            
        }

        public class IndexViewData : DashboardAdminBaseViewData
        {
            public IList<HeadOffice> HeadOffices { get; set; }
            public HeadOffice HeadOffice { get; set; }
            public IList<Site> Sites { get; set; }
            public Site Site { get; set; }
            public Region Region { get; set; }
            public IList<IndicatorDefinition> IndicatorDefinitions { get; set; }
            public IndicatorDefinition IndicatorDefinition { get; set; }
            public IndicatorTranslation IndicatorTranslation { get; set; }

            public IEnumerable<SelectListItem> RegionListItems;
            public IEnumerable<SelectListItem> HeadOfficeListItems;
        }
    }
}