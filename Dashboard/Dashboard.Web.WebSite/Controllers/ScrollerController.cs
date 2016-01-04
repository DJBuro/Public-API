using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate;
using Dashboard.Web.Mvc;
using Dashboard.Web.Mvc.Helpers;
using Dashboard.Web.WebSite.Models;

namespace Dashboard.Web.WebSite.Controllers
{
    [HandleError]
    public class ScrollerController : SiteController
    {
        public IDashboardDataDao DashboardDataDao { get; set; }
        public IIndicatorDefinitionDao IndicatorDefinitionDao { get; set; }

        public ActionResult Index()
        {
            Site site;

            /* Site = SiteDao.FindById(388);*/

            site = SiteDao.FindByIP(this.Request.UserHostAddress);

            if (site == null)
                return RedirectToAction("Logon", "Account");

            var data = new DashboardViewData.ScrollerViewData();

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();
            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            data.ScrollTickets = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOPD = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());
            data.ScrollOTD = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());

            return (View(ScrollerControllerViews.Index, data));
        }

        public ActionResult Tickets()
        {
            Site site;

            /*Site = SiteDao.FindById(388);*/

             site = SiteDao.FindByIP(this.Request.UserHostAddress);

            if (site == null)
                return RedirectToAction("Logon", "Account");

            var data = new DashboardViewData.ScrollerViewData();

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();
            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            data.ScrollTickets = ScrollingHelper.GetScrollingTickets(allSiteData, indicatorDefinitions.ToList());

            return (View(ScrollerControllerViews.Ticket, data));
        }

        public ActionResult OPD()
        {
            Site site;

             /* Site = SiteDao.FindById(388);*/

           site = SiteDao.FindByIP(this.Request.UserHostAddress);

            if (site == null)
                return RedirectToAction("Logon", "Account");

            var data = new DashboardViewData.ScrollerViewData();

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();
            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            data.ScrollOPD = ScrollingHelper.GetScrollingOPD(allSiteData, indicatorDefinitions.ToList());

            return (View(ScrollerControllerViews.OPD , data));
        }

        public ActionResult OTD()
        {
            Site site;

            /*Site = SiteDao.FindById(388);*/

             site = SiteDao.FindByIP(this.Request.UserHostAddress);

            if (site == null)
                return RedirectToAction("Logon", "Account");

            var data = new DashboardViewData.ScrollerViewData();

            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            var allSiteData = DashboardDataDao.FindAll().Where(c => c.Site.HeadOffice == site.HeadOffice).ToList();
            var indicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(site.HeadOffice);

            data.ScrollOTD = ScrollingHelper.GetScrollingOTD(allSiteData, indicatorDefinitions.ToList());

            return (View(ScrollerControllerViews.OTD, data));
        }

    }
}
