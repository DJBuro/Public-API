using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Menus.Services.Data;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Logging;
using MyAndromeda.Data.AcsServices.Context;

namespace MyAndromeda.Web.Controllers.Api
{
    public class MenuController : ApiController
    {
        private readonly INotifier notifier;
        private readonly IWorkContext workContext;
        private readonly IMyAndromedaLogger logger;

        private readonly IGetAcsAddressesService getAcsAddressesService;
        private readonly IAcsMenuServiceAsync acsMenuServiceService;
        private readonly IStoreDataService storeDataService;
        private readonly IActiveMenuContext menuContext;
        private readonly IMenuItemService menuItemService;

        public MenuController(IGetAcsAddressesService getAcsAddressesService,
            IAcsMenuServiceAsync acsMenuServiceService,
            IStoreDataService storeDataService,
            IWorkContext workContext,
            IMyAndromedaLogger logger,
            IActiveMenuContext menuContext,
            IMenuItemService menuItemService,
            INotifier notifier) 
        {
            this.notifier = notifier;
            this.menuItemService = menuItemService;
            this.menuContext = menuContext;
            this.logger = logger;
            this.workContext = workContext;
            this.storeDataService = storeDataService;
            this.acsMenuServiceService = acsMenuServiceService;
            this.getAcsAddressesService = getAcsAddressesService;
        }

        private async Task<MyAndromedaMenu> BuildMenu() 
        {
            //bool useMasterMenuId = this.workContext.CurrentSite.Store.Chain.MasterMenuId > 0;

            //var store = this.workContext.CurrentSite.Store;
            //if (useMasterMenuId)
            //{
            //    var id = this.workContext.CurrentSite.Store.Chain.MasterMenuId.GetValueOrDefault();
            //    store = this.storeDataService.Get(e => e.Id == id);
            //}

            var store = this.workContext.CurrentSite.Store;
            var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(store);
            var currentMenu = await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(store.AndromedaSiteId, store.ExternalId, endpoints);

            return currentMenu;
        }

        [HttpPost]
        public async Task<MyAndromedaMenu> GetMenu() 
        {
            var menu = await BuildMenu();

            return menu;
        }

        [HttpPost]
        public object SaveMenuItemsBatch(MyAndromedaMenuItemSavingModel model)
        {
            
            try
            {
                this.logger.Debug("SaveMenuItemsBatch initiated;");
                this.logger.LogWorkContext(this.workContext);
                this.logger.DebugItems(model.Models, e => string.Format("Name: {0} - id: {1};", e.Name, e.Id));

                var menu = this.menuContext.Menu;
                this.menuItemService.UpdateMenuItemsBatch(menu, model.Models, UpdateSection.Data);

                var message = string.Format("Your changes to {0} items completed.", model.Models.Length);
                this.notifier.Success(message, true);

                return Json(new
                {
                    MenuItems = model.Models,
                    Total = model.Models.Length
                });

            }
            catch (Exception e) 
            {

                this.notifier.Error("There was a error while saving. Please try again. Contact support if the problem still persists.", true); 

                throw e;
            }
        }

        [HttpPost]
        public object SaveMenuItemsSequence(MyAndromedaMenuItemSavingModel model)
        {
            var menu = this.menuContext.Menu;
            this.menuItemService.UpdateMenuItemsBatch(menu, model.Models, UpdateSection.Sequence);

            return Json(new
            {
                MenuItems = model.Models,
                Total = model.Models.Length
            });
        }

        //private async Task<MyAndromedaMenu> GetMenuFrom() 
        //{
        //    //fetch the main menuid 
        //    //could be the chain's master menu, or could be that of the store. 

        //    var store = this.currentSite.Store;

        //    bool useMasterMenuId = this.currentChain.Chain.MasterMenuId.GetValueOrDefault() > 0; 
        //    if(useMasterMenuId)
        //    {
                
        //    }

        //    var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(this.currentSite.Store);
        //    var currentMenu = await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(this.currentSite., store.ExternalId, endpoints);
        //}

        //public async Task<MyAndromedaMenu> GetMenu() 
        //{
            
        //}
    }
}
