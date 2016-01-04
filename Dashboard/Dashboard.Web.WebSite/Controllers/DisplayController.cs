using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Dao.Domain.Helpers;
using Dashboard.Web.Mvc;
using Dashboard.Web.Mvc.Filters;
using Dashboard.Web.Mvc.Helpers;
using Dashboard.Web.WebSite.Models;

namespace Dashboard.Web.WebSite.Controllers
{
    [HandleError]
    public class DisplayController : SiteController
    {
        public IDashboardDataDao DashboardDataDao { get; set; }
        public IIndicatorDefinitionDao IndicatorDefinitionDao { get; set; }

    [ValidateInput(false)]
        public ActionResult Index()
        {
            //Note: we are assuming that the user isn't a headoffice (global) user 
            var user = this.User;
            
            if(user == null)
                return RedirectToAction("Logon", "Account");

            if (user.HeadOfficeUser)
                return RedirectToAction("Index", "HeadOffice");

            var data = new DashboardViewData.DisplayViewData();

            var regions = RegionDao.FindAll().Where(c => c.HeadOffice == user.HeadOffice).ToList();

            var availableRegions = new List<SitesRegion>();

            //todo: there are better ways
            foreach (Region region in regions)
            {
                foreach (SitesRegion sitesRegion in region.SitesRegions)
                {
                    foreach (Permission permitted in user.Permissions)
                    {
                        if (sitesRegion.Site.Id == permitted.Site.Id)
                        {                       
                            availableRegions.Add(sitesRegion);
                        }
                    }
                } 
            }

            var avaliableRegions = new List<Region>();

            //todo: sort out
            foreach (Region region in regions)
            {
                if(availableRegions[0].Region == region)
                {
                    avaliableRegions.Add(availableRegions[0].Region);
                    region.SitesRegions = availableRegions;
                }
            }

            foreach (SiteMessage message in user.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            data.Regions = avaliableRegions;

            data.IndicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(user.HeadOffice).OrderByDescending(c => c.UseFormula).ToList();

            return (View(DisplayControllerViews.Index, data));
        }


        //http://weblogs.asp.net/rashid/archive/2009/04/01/asp-net-mvc-best-practices-part-1.aspx
        [ValidateInput(false)]
        public ActionResult Site(string id)
        {
            var user = this.User;
            bool isSite = false;

            var data = new DashboardViewData.DisplayViewData();

            if (user == null)
            {
                var cookieSiteId = Dashboard.Web.Mvc.Utilities.Cookie.GetAuthoriationCookieSiteId(this.Request);

                if (cookieSiteId.ToString() != id)
                    return RedirectToAction("Logon", "Account");

                isSite = true;
            }

            if (!isSite)
            {
                if (!Dashboard.Web.Mvc.Utilities.Authorization.CanDo(user, id))
                    return RedirectToAction("Logon", "Account");
            }

            data.Site = SiteDao.FindById(Convert.ToInt32(id));

            foreach (SiteMessage message in data.Site.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            return (View(DisplayControllerViews.Site, data));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDefinition(IndicatorDefinition definitions)
        {
            var data = new DashboardViewData.DisplayViewData();

            data.IndicatorDefinition = new IndicatorDefinition();

            data.IndicatorDefinition = IndicatorDefinitionDao.FindById(definitions.Id.Value);

            if (TryUpdateModel(data.IndicatorDefinition))
            {
                IndicatorDefinitionDao.Update(data.IndicatorDefinition);
            }

            return RedirectToAction(DisplayControllerViews.Index, data);
          
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveDefinitions(IList<IndicatorDefinition> definitions)
        {
            var data = new DashboardViewData.DisplayViewData();

            data.IndicatorDefinitions = new List<IndicatorDefinition>();

            //data.IndicatorDefinition = IndicatorDefinitionDao.FindById(definitions.Id.Value);

            //if (TryUpdateModel(data.IndicatorDefinitions))
            //{
            //    //IndicatorDefinitionDao.Update(data.IndicatorDefinition);
            //}

            return RedirectToAction(DisplayControllerViews.Index, data);
        }

        /// <summary>
        /// Only called by the Flex Dashboard
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult FlexData()
        {
            var data = new DashboardViewData.IndexViewData();

            ////note: this is only for testing!!!!
            //if (this.HttpContext.Request.IsLocal)
            //{
            //    var siteId = "369";
            //    data.Site = SiteDao.FindById(Convert.ToInt32(siteId));
            //}
            //else
            //{
            //}

            var userHostAddress = this.HttpContext.Request.UserHostAddress;

            data.Site = SiteDao.FindByIP(userHostAddress);

            if (data.Site == null)
            {
                //NYP
                //data.Site = SiteDao.FindById(369);
                //TBBC
                data.Site = SiteDao.FindById(45);
                //PJ
              // data.Site = SiteDao.FindById(175);
            }

            

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == data.Site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(data.Site.HeadOffice);

            //Display class
            var displayData = BuildDashboardData.GetData(data.Site, indicatorDefinitions, allSiteData);

            foreach (var gaug in displayData.Gauges)
            {
                if(gaug.TopSite.Count < 3)
                {
                    for (int i = gaug.TopSite.Count; i < 3; i++)
                    {
                        gaug.TopSite.Add(new SiteRank(0,"no sites reporting",0,"","",null));
                    }
                }

                if(gaug.BottomSite == null)
                {
                    gaug.BottomSite = new List<SiteRank>();

                    for (int i = 0; i < 3; i++)
                    {
                        gaug.BottomSite.Add(new SiteRank(0, "no sites reporting", 0, "", "", null));
                    }
                }

            }

            var scrollingOtd = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());
            var scrollingOpd = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());
            var scrollingTickets = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());

            if (scrollingOtd.Length == 0)
                scrollingOtd = " Sorry, no recent data available for today... ";

            if (scrollingOpd.Length == 0)
                scrollingOpd = " Sorry, no recent data available for today... ";

            if (scrollingTickets.Length == 0)
                scrollingTickets = " Sorry, no recent data available for today... ";

            var gauges = new List<Gauge>();

            foreach (Gauge gauge in displayData.Gauges)
            {
                gauges.Add(gauge);
            }

            var hoMessage = new StringBuilder(data.Site.SiteName);
            if(data.Site.HeadOffice.SiteMessages.Count > 0)
            {
                foreach (SiteMessage c in data.Site.HeadOffice.SiteMessages)
                {
                    hoMessage.Append(" - ");
                    hoMessage.Append(c.Message);
                }
            }

            var fxdb = new FlexDashboard
                           {
                               HeadOfficeMessage = hoMessage.ToString(),
                               Gauges = gauges,
                               scrollingOpd = scrollingOpd,
                               scrollingOtd = scrollingOtd,
                               scrollingTickets = scrollingTickets
                           };

            

            var jsonResult = this.Json(fxdb);

            return jsonResult;

        }


        /// <summary>
        /// Gauges
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UpdateDisplayData(string siteId)
        {

            //#if (DEBUG)

            // siteId = "369";

            //#endif

            if (siteId == null)
                return null;

            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            //Display class
            var displayData = BuildDashboardData.GetData(site, indicatorDefinitions, allSiteData);

            

            return this.Json(displayData);
        }


        //[AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UpdateScrollingOtdData(string siteId)
        {
            if (siteId == null)
                return null;
            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            var otd = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());

            if (otd.Length == 0)
                otd = " Sorry, no recent data available for today...";


            return this.Json(otd);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UpdateScrollingOpdData(string siteId)
        {
            if (siteId == null)
                return null;



            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            var opd = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());

            if (opd.Length == 0)
                opd = " Sorry, no recent data available for today...";

            return this.Json(opd);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult UpdateScrollingTicketsData(string siteId)
        {
            if (siteId == null)
                return null;
            var site = SiteDao.FindById(Convert.ToInt32(siteId));

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();

            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            var otd = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());

            if (otd.Length == 0)
                otd = " Sorry, no recent data available for today...";

            return this.Json(otd);
        } 
    }
}
