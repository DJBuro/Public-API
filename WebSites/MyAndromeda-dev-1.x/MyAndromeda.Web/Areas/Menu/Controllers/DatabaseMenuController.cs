using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.MenuDatabase.Context;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Menus.Services.Menu;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Menu.ViewModels;
using System.IO;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class DatabaseMenuController : Controller
    {
        private readonly ICurrentSite currentSite;
        private readonly IFtpMenuManagerService menuManagerService;
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;
        private readonly IAccessDbMenuVersionDataService accessDatabase;
        public DatabaseMenuController(ICurrentSite currentSite,
            IFtpMenuManagerService menuManagerService,
            IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService,
            IAccessDbMenuVersionDataService accessDatabase)
        {
            this.accessDatabase = accessDatabase;
            this.currentSite = currentSite;
            this.menuManagerService = menuManagerService;
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
        }

        [ChildActionOnly]
        public PartialViewResult Debug()
        {
            if (accessDatabase.IsAvailable(currentSite.AndromediaSiteId))
            {
                var current = accessDatabase.GetMenuVersionRow(currentSite.AndromediaSiteId);
                DebugViewModel o = new DebugViewModel { AccessVersion = current.nVersion };
                return PartialView(o);
            }

            return PartialView();
        }

        public JsonResult QueryForNewMenu(bool? force) 
        {
            var siteMenu = this.myAndromedaSiteMenuDataService.GetMenu(this.currentSite.AndromediaSiteId);

            bool enQueued = force.GetValueOrDefault() ?
                menuManagerService.AddTaskToDownloadTheMenu(siteMenu, TimeSpan.FromHours(-1)) :
                //30 min throttle for checking menus.
                menuManagerService.AddTaskToDownloadTheMenu(siteMenu, TimeSpan.FromMinutes(30)) ;
            
            return Json(new {
                InQueue = enQueued
            }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult ForceDeleteDatabaseMenu() 
        {
            var siteMenu = this.myAndromedaSiteMenuDataService.GetMenu(this.currentSite.AndromediaSiteId);

            this.myAndromedaSiteMenuDataService.Delete(siteMenu);

            return Json(new { Hello = true });
        }

        [HttpPost]
        public JsonResult ForceDeleteAccessMenu() 
        {
            MenuConnectionStringContext context = new MenuConnectionStringContext(this.currentSite.AndromediaSiteId);

            string localPath = context.LocalFullPath;

            if (System.IO.File.Exists(localPath)) 
            {
                System.IO.File.Delete(localPath);
            }

            return Json(new { Hello = true });
        }
    }
}
