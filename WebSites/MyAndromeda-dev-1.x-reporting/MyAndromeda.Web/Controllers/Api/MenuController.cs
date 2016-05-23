using System;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Menus.Services.Data;
using MyAndromeda.Logging;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Web.Controllers.Api
{
    public class MenuController : ApiController
    {
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;

        private readonly IWorkContext workContext;
        private readonly IMyAndromedaLogger logger;

        private readonly IGetAcsAddressesService getAcsAddressesService;
        private readonly IAcsMenuServiceAsync acsMenuServiceService;
        private readonly IActiveMenuContext menuContext;
        private readonly IMenuItemService menuItemService;

        public MenuController(IGetAcsAddressesService getAcsAddressesService, IAcsMenuServiceAsync acsMenuServiceService, IWorkContext workContext, IMyAndromedaLogger logger, IActiveMenuContext menuContext, IMenuItemService menuItemService, INotifier notifier, IAuthorizer authorizer) 
        {
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.menuItemService = menuItemService;
            this.menuContext = menuContext;
            this.logger = logger;
            this.workContext = workContext;
            this.acsMenuServiceService = acsMenuServiceService;
            this.getAcsAddressesService = getAcsAddressesService;
        }

        private async Task<MyAndromedaMenu> BuildMenu() 
        {
            var store = this.workContext.CurrentSite.Store;
            var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(store);
            var currentMenu = await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(store.AndromedaSiteId, store.ExternalId, endpoints);

            return currentMenu;
        }

        [HttpGet, HttpPost]
        public async Task<MyAndromedaMenu> GetMenu() 
        {
            var menu = await BuildMenu();

            return menu;
        }

        [HttpPost]
        public async Task<object> SaveMenuItemsBatch(MyAndromedaMenuItemSavingModel model)
        {
            if(!this.authorizer.Authorize( MyAndromeda.Web.Areas.Menu.UserPermissions.EditStoreMenu))
            {
                return Unauthorized();
            }

            try
            {
                this.logger.Debug(message: "SaveMenuItemsBatch initiated;");
                this.logger.DebugItems(model.Models, e => string.Format(format: "Name: {0} - id: {1};", arg0: e.Name, arg1: e.Id));

                SiteMenu menu = this.menuContext.Menu;

                await this.menuItemService.UpdateMenuItemsBatchAsync(menu, model.Models, UpdateSection.Data);

                string message = string.Format(format: "Your changes have been saved for later.", arg0: model.Models.Length);

                this.notifier.Success(message, notifyOthersInStore: true);

                return Json(new
                {
                    MenuItems = model.Models,
                    Total = model.Models.Length
                });

            }
            catch (Exception e) 
            {

                this.notifier.Error(message: "There was a error while saving. Please try again. Contact support if the problem still persists.", notifyOthersInStore: true); 

                throw e;
            }
        }

        [HttpPost]
        public async Task<object> SaveMenuItemsSequence(MyAndromedaMenuItemSavingModel model)
        {
            if (!this.authorizer.Authorize(MyAndromeda.Web.Areas.Menu.UserPermissions.EditStoreMenu))
            {
                return Unauthorized();
            }

            SiteMenu menu = this.menuContext.Menu;
            await this.menuItemService.UpdateMenuItemsBatchAsync(menu, model.Models, UpdateSection.Sequence);

            return Json(new
            {
                MenuItems = model.Models,
                Total = model.Models.Length
            });
        }

    }
}
