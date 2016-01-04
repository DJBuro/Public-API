using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Dao.Domain.Helpers;
using Dashboard.Dao.NHibernate;
using Dashboard.Web.Mvc;
using Dashboard.Web.Mvc.Helpers;
using Dashboard.Web.WebSite.Models;


namespace Dashboard.Web.WebSite.Controllers
{
    [HandleError]
    public class DashboardController : SiteController
    {
        public IDashboardDataDao DashboardDataDao { get; set; }
        public IIndicatorDefinitionDao IndicatorDefinitionDao { get; set; }
 //public IHeadOfficeDao HeadOfficeDao { get; set; }

        public ActionResult Index()
        {
            var dashboardUser = this.DashboardUser;

            //if(!this.User.Identity.IsAuthenticated)
            //    return RedirectToAction("Logon", "Account");//todo: move to sitefilter


            var data = new DashboardViewData.HeadOfficeViewData();

            var regions = RegionDao.FindAll().Where(c => c.HeadOffice == dashboardUser.HeadOffice).ToList();

            foreach( SiteMessage message in dashboardUser.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            data.Regions = regions;

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == dashboardUser.HeadOffice).ToList();
            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(dashboardUser.HeadOffice);

            //data.Scroller = ScrollingHelper.GetScrolling(allSiteData, indicatorDefinitions.ToList());
            data.ScrollTickets = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOPD = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOTD = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());

            var p = 0;

            foreach(var region in regions)
            {
                foreach(SitesRegion sitesRegion in region.SitesRegions)
                {
                    switch (sitesRegion.Region.Id.Value)
                    {
                        case 4:
                            DashboardData x = (DashboardData) sitesRegion.Site.DashboardData[0];
                            p = p + x.Column20.Value;
                            break;
                    }
                }
            }

            return (View(DashboardControllerViews.Index, data));

        }

        public ActionResult Display(int? id)
        {
            var data = new DashboardViewData.HeadOfficeViewData();

            //note: compare site.ip to ip

            //note: check if id is not null
            
            data.Site = SiteDao.FindById(Convert.ToInt32(id));

            foreach (SiteMessage message in data.Site.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            return (View(DashboardControllerViews.Display, data));
        }

        /// <summary>
        /// Gauges
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult UpdateDisplayData(string siteId)
        {
            if (siteId.Length == 0)
                return null;

            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            //Display class
            var displayData = BuildDashboardData.GetData(site, indicatorDefinitions, allSiteData);
           //displayData.TopSite = new List<SiteRank>();

            //var siteranker = new SiteRank();
            //siteranker.rank = 1;
            //siteranker.name = "one";

            //displayData.TopSite.Add(siteranker);

            return this.Json(displayData);
        }

 
        /// <summary>
        /// Blocked site scrollers
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult UpdateScrollingData(string siteId)
        {
            if (siteId.Length == 0)
                return null;

            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            //Display class
            //var displayData = BuildDashboardData.GetData(site, indicatorDefinitions, allSiteData);

            var scrollingData = ScrollingHelper.ScrollingSite(allSiteData, indicatorDefinitions.ToList());
            //scrollingData[].Avg.Tick.OPD.InStore.LocationName
            return this.Json(scrollingData);
        } 


        [HandleError]
        public ActionResult Site(string id)
        {
            /*
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Logon", "Account"); //todo: move to sitefilter
           */

            if(id.Length == 0)
                return (View(DashboardControllerViews.Index));


            var data = new DashboardViewData.HeadOfficeViewData();

            var site = SiteDao.FindById(Convert.ToInt32(id));

            foreach (SiteMessage message in site.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            //TODO: refactor scrolling into a class
           // data.Scroller = ScrollingHelper.GetScrolling(allSiteData, indicatorDefinitions.ToList());
            data.ScrollTickets = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOPD = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOTD = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());

            data.Site = site;
            //data.Dash = BuildDashboardData.DashboardData(site, allSiteData, indicatorDefinitions);

            return (View(DashboardControllerViews.Site, data));
        }

        [HandleError]
        public ActionResult Region(string id)
        {
            if (!this.User.Identity.IsAuthenticated)
                return RedirectToAction("Logon", "Account"); //todo: move to sitefilter

            if (id.Length == 0)
                return (View(DashboardControllerViews.Index));


            var data = new DashboardViewData.HeadOfficeViewData();


            var region = RegionDao.FindById(Convert.ToInt32(id));

            //var Site = SiteDao.FindById(Convert.ToInt32(id));


            //foreach (SiteMessage message in Site.HeadOffice.SiteMessages)
            //{
            //    data.HeadOfficeMessage = message.Message;
            //}

           var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == region.HeadOffice).ToList();
            //var allRegionData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice.Regions == Site.SitesRegions).ToList();


            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(region.HeadOffice);

            //data.Scroller = ScrollingHelper.GetScrolling(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOPD = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOTD = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());


            data.Region = region;
            data.Dash = BuildDashboardData.DashboardData(region, allSiteData, indicatorDefinitions);

            return (View(DashboardControllerViews.Region, data));
        }

    }
}
