using System.Web.Mvc;
using WebDashboard.Dao;
using WebDashboard.Mvc;
using WebDashboard.Mvc.Filters;
using WebDashboard.Web.Models;

namespace WebDashboard.Admin.Controllers
{
    public class GlobalController : SiteController
    {
        public IDefinitionDao DefinitionDao { get; set; }
        public ILogDao LogDao { get; set; }
        

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new WebDashboardViewData.PageViewData();

            //var xyz = LogDao.FindAll();

            return View(AdminControllerViews.Index, data);
        }

        [RequiresAuthorisation]
        public ActionResult ValueTypes(int? id)
        {
            var data = new WebDashboardViewData.PageViewData();

            return View(AdminControllerViews.ValueTypes, data);
        }

        [RequiresAuthorisation]
        public ActionResult Logs()
        {
            var data = new WebDashboardViewData.PageViewData();

            data.Logs = LogDao.FindAllGrouped();

            return View(AdminControllerViews.Logs, data);
        }

    }
}
