using System;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Data;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMyAndromedaLogger loggger;
        private readonly IMenuItemService menuItemService;
        private readonly IActiveMenuContext menuContext;
        private readonly IWorkContext workContext;

        public MenuItemController(IActiveMenuContext menuContext, IMenuItemService menuItemService, IMyAndromedaLogger loggger, IWorkContext workContext)
        {
            this.workContext = workContext;
            this.loggger = loggger;
            this.menuItemService = menuItemService;
            this.menuContext = menuContext;
        }

        [HttpPost]
        public ActionResult SaveMenuItemsBatch(MyAndromedaMenuItem[] models) 
        {
            this.loggger.Debug("SaveMenuItemsBatch initiated;");
            this.loggger.LogWorkContext(this.workContext);
            this.loggger.DebugItems(models, e => string.Format("Name: {0} - id: {1};", e.Name, e.Id));

            var menu = this.menuContext.Menu;
            this.menuItemService.UpdateMenuItemsBatch(menu, models, UpdateSection.Data);

            return Json(new {
                MenuItems = models,
                Total = models.Length
            });
        }

        [HttpPost]
        public ActionResult SaveMenuItemsSequence(MyAndromedaMenuItem[] models) 
        {
            var menu = this.menuContext.Menu;
            this.menuItemService.UpdateMenuItemsBatch(menu, models, UpdateSection.Sequence);

            return Json(new
            {
                MenuItems = models,
                Total = models.Length
            });
        }

    }
}