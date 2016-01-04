using System;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Data;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult> SaveMenuItemsBatch(MyAndromedaMenuItem[] models) 
        {
            this.loggger.Debug("SaveMenuItemsBatch initiated;");

            var menu = this.menuContext.Menu;
            await this.menuItemService.UpdateMenuItemsBatchAsync(menu, models, UpdateSection.Data);

            return Json(new {
                MenuItems = models,
                Total = models.Length
            });
        }

        [HttpPost]
        public async Task<ActionResult> SaveMenuItemsSequence(MyAndromedaMenuItem[] models) 
        {
            var menu = this.menuContext.Menu;
            await this.menuItemService.UpdateMenuItemsBatchAsync(menu, models, UpdateSection.Sequence);

            return Json(new
            {
                MenuItems = models,
                Total = models.Length
            });
        }

    }
}