using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Marketing.Services;

namespace MyAndromeda.Web.Areas.Marketing.Controllers
{
    public class EmailTasksController : Controller
    {
        private readonly IMarketEmailTaskService marketEmailTaskService;
        private readonly WorkContextWrapper workContextWrapper;

        public EmailTasksController(IMarketEmailTaskService marketEmailTaskService, WorkContextWrapper workContextWrapper)
        {
            this.workContextWrapper = workContextWrapper;
            this.marketEmailTaskService = marketEmailTaskService;
        }

        /// <summary>
        /// List a history of email campaign tasks
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RunningTasks()
        {
            var siteId = workContextWrapper.Current.CurrentSite.Site.Id;
            var data = marketEmailTaskService.GetRunningCampaignTasksForSite(siteId).ToArray();

            return PartialView(data);
        }

        public JsonResult Read([DataSourceRequest] DataSourceRequest request) 
        {
            var siteId = workContextWrapper.Current.CurrentSite.Site.Id;
            var data = marketEmailTaskService.GetEmailCampaignTasksForSite(siteId).ToArray();

            return Json(data.ToDataSourceResult(request));
        }


    }
}
