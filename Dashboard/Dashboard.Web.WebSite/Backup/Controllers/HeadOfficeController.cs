using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Web.Mvc;
using Dashboard.Web.WebSite.Models;

namespace Dashboard.Web.WebSite.Controllers
{
    public class HeadOfficeController : SiteController
    {
        public IDashboardDataDao DashboardDataDao { get; set; }
        public IIndicatorDefinitionDao IndicatorDefinitionDao { get; set; }

        public ActionResult Index()
        {
            //Note: we are assuming that the user is a headoffice (global) user 
            var user = this.User;

            if (user == null)
                return RedirectToAction("Logon", "Account");

            if (!user.HeadOfficeUser)
                return RedirectToAction("Logon", "Account");

            var data = new DashboardViewData.HeadOfficeViewData();

            var regions = RegionDao.FindAll().Where(c => c.HeadOffice == user.HeadOffice).ToList();

            foreach (SiteMessage message in user.HeadOffice.SiteMessages)
            {
                data.HeadOfficeMessage = message.Message;
            }

            data.Regions = regions;

            data.IndicatorDefinitions = IndicatorDefinitionDao.FindByHeadOffice(user.HeadOffice).OrderByDescending(c => c.UseFormula).ToList();

            return(View(HeadOfficeControllerViews.Index, data));
        }

    }
}
