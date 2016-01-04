using System.Linq;
using System.Web.Mvc;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Web.Mvc;
using Dashboard.Web.Mvc.Extensions;
using Dashboard.Web.Mvc.Filters;
using DashboardAdmin.Models;

namespace DashboardAdmin.Controllers
{
    public class HeadOfficeController : SiteController
    {
        public ISitesRegionDao SitesRegionDao;
        public IIndicatorDefinitionDao IndicatorDefinitionDao;
        public IIndicatorTranslationDao IndicatorTranslationDao;

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffices = HeadOfficeDao.FindAll();

            return (View(AdminControllerViews.Index, data));
        }
        //[RequiresAuthorisation]
        public ActionResult Details(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffice = HeadOfficeDao.FindById(id);

            return (View(AdminControllerViews.Details, data));
        }

        //[RequiresAuthorisation]
        public ActionResult Dashboard(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffice = HeadOfficeDao.FindById(id);

            data.IndicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(data.HeadOffice);

            return (View(AdminControllerViews.Dashboard, data));
        }

        //[RequiresAuthorisation]
        public ActionResult Translation(int id, int trans)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffice = HeadOfficeDao.FindById(id);

            data.IndicatorTranslation = IndicatorTranslationDao.FindById(trans);

            return (View(AdminControllerViews.Translation, data));
        }

        //[RequiresAuthorisation]
        public ActionResult Sites(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffice = HeadOfficeDao.FindById(id);

            return (View(AdminControllerViews.Sites, data));
        }

        //[RequiresAuthorisation]
        public ActionResult Site(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.Site = SiteDao.FindById(id);

            if (data.Site.SitesRegions.Count > 0)
            {
                var selected = (SitesRegion) data.Site.SitesRegions[0];

                data.RegionListItems = RegionDao.FindAll().Where(c => c.HeadOffice.Id.Value == data.Site.HeadOffice.Id.Value).ToSelectList("Id","RegionName", selected.Region.Id.Value.ToString()); 
            }
            else // there isn't a region that this site belongs to
            {
                data.RegionListItems = RegionDao.FindAll().Where(c => c.HeadOffice.Id.Value == data.Site.HeadOffice.Id.Value).ToSelectList("Id", "RegionName"); 
                //note: on an update, this forces a selection, no reason a site shouldn't be assigned to a region.
            }

            return (View(AdminControllerViews.Site, data));
        }

        //[RequiresAuthorisation]
        public ActionResult Region(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOffice = HeadOfficeDao.FindById(id);

            return (View(AdminControllerViews.Region, data));
        }

        //[RequiresAuthorisation]
        public ActionResult RegionSites(int id)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.Region = RegionDao.FindById(id);

            return (View(AdminControllerViews.RegionSites, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSite(Site site)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if(UpdateModel(site))
            {
                SiteDao.Update(site);
            }

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Sites, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSite(Site site)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(site))
            {
                data.Site = SiteDao.Create(site);
            }

            data.RegionListItems = RegionDao.FindAll().Where(c => c.HeadOffice.Id.Value == site.HeadOffice.Id.Value).ToSelectList("Id", "RegionName"); 

            return (View(AdminControllerViews.AddSiteRegion, data));
        }

        //todo: dashboard message

        //[RequiresAuthorisation]
        public ActionResult AddSite()
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.HeadOfficeListItems = HeadOfficeDao.FindAll().ToSelectList("Id", "HeadOfficeName");

            return (View(AdminControllerViews.AddSite, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddSiteRegion(SitesRegion sitesRegion)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(sitesRegion))
            {
                SitesRegionDao.Create(sitesRegion);
            }

            data.HeadOffices = HeadOfficeDao.FindAll();

            return (View(AdminControllerViews.Index, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveSite(Site site)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(site))
            {
                site = SiteDao.FindById(site.Id.Value);

                

                SiteDao.Delete(site);

                //todo: does this remove the siteRegion too?
            }

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Sites, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateRegion(Region region)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if(UpdateModel(region))
            {
                RegionDao.Update(region);
            }

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);

            //todo: return to Region
            return (View(AdminControllerViews.Details, data));
        }

        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSiteRegion(SitesRegion sitesRegion)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            var siteRegion = new SitesRegion();

            if (UpdateModel(sitesRegion))
            {
                if (sitesRegion.Id == null)
                {
                    siteRegion = SitesRegionDao.Create(sitesRegion);
                }
                else
                {
                    SitesRegionDao.Update(sitesRegion);
                }
            }

            data.Site = SiteDao.FindById(sitesRegion.Site.Id.Value);

            data.HeadOffice = HeadOfficeDao.FindById(data.Site.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Details, data));
        } 
        
        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateTranslation(IndicatorTranslation indicatorTranslation)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(indicatorTranslation))
            {
                IndicatorTranslationDao.Update(indicatorTranslation);
            }

            data.HeadOffice = HeadOfficeDao.FindById(indicatorTranslation.IndicatorDefinition.HeadOffice.Id.Value);

            data.IndicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(data.HeadOffice);

            return (View(AdminControllerViews.Dashboard, data));
        }        
        
        //[RequiresAuthorisation]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateDashboard(IndicatorDefinition indicatorDefinition)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            data.IndicatorDefinition = IndicatorDefinitionDao.FindById(indicatorDefinition.Id.Value);

            if (UpdateModel(data.IndicatorDefinition))
            {
                IndicatorDefinitionDao.Update(data.IndicatorDefinition);
            }

            data.HeadOffice = HeadOfficeDao.FindById(data.IndicatorDefinition.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Details, data));
        }
        

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRegion(Region region)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(region))
            {
                RegionDao.Create(region);
            }

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);
            
            //todo: return to Region
            return (View(AdminControllerViews.Details, data));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveRegion(Region region)
        {
            var data = new DashboardAdminViewData.IndexViewData();

            if (UpdateModel(region))
            {
                RegionDao.Delete(region);
            }

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);

            //todo: return to Region
            return (View(AdminControllerViews.Details, data));
        }

    }
}
