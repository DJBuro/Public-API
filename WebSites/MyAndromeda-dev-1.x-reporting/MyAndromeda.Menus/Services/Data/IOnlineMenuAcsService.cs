using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.MenuDatabase;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Menu;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyAndromeda.Menus.Services.Data
{
    //public interface IGetOnlineMenuFromAcsService : IDependency
    //{
    //    /// <summary>
    //    /// Gets my Andromeda menu async.
    //    /// </summary>
    //    /// <param name="endpoints">The endpoints.</param>
    //    /// <returns></returns>
    //    Task<MyAndromedaMenu> GetMyAndromedaMenuAsync(string[] endpoints);

    //    /// <summary>
    //    /// Gets the menu endpoints async.
    //    /// </summary>
    //    /// <returns></returns>
    //    //Task<IEnumerable<string>> GetMenuEndpointsAsync();
    //}

    //public class OnlineMenuAcsService : IGetOnlineMenuFromAcsService
    //{
    //    private readonly WorkContextWrapper workContextWrapper;
    //    private readonly IActiveMenuContext activeMenuContext;

    //    private readonly IAccessMenuItemDataService accessMenuItemDataService;
    //    private readonly IAccessPriceDataService accessPriceDataService;
    //    private readonly IAccessDbMenuVersionDataService accessMenuVersionService;
        

    //    private readonly IMenuItemService accessMenuItemService;

    //    private readonly IAcsMenuServiceAsync acsMenuService;
    //    private readonly IMyAndromedaLogger logger;
        
    //    public OnlineMenuAcsService(
    //        WorkContextWrapper workContextWrapper, 
    //        IActiveMenuContext activeMenuContext,
    //        IMyAndromedaLogger logger,
    //        IAccessMenuItemDataService accessMenuItemDataService, 
    //        IAcsMenuServiceAsync acsMenuService,              
    //        IAccessDbMenuVersionDataService accessMenuVersionService, 
    //        IWebOrderingService webOrderingService,
    //        IMenuItemService accessMenuItemService, 
    //        IAccessPriceDataService accessPriceDataService)
    //    {
    //        this.accessPriceDataService = accessPriceDataService;
    //        this.accessMenuVersionService = accessMenuVersionService;
    //        this.accessMenuItemService = accessMenuItemService;
    //        this.workContextWrapper = workContextWrapper;
    //        this.activeMenuContext = activeMenuContext;
    //        this.logger = logger;
    //        this.acsMenuService = acsMenuService;
    //        this.accessMenuItemDataService = accessMenuItemDataService;
    //    }

    //    public async Task<MyAndromedaMenu> GetMyAndromedaMenuAsync(string[] endpoints) 
    //    {
    //        System.Diagnostics.Trace.WriteLine("Getting asc menu");
    //        var menu = await this.GetMenuFromAcsServiceAsync(endpoints);

    //        //this.accessMenuItemService.UpdateMenuItems(context.Menu, context.Menu.MenuItems);
            
    //        return menu;
    //    }     

    //    private async Task<MyAndromedaMenu> GetMenuFromAcsServiceAsync(string[] endpoints) 
    //    {
    //        var menu = await acsMenuService.GetMenuDataFromEndpointsAsync(
    //            this.workContextWrapper.Current.CurrentSite.AndromediaSiteId, 
    //            this.workContextWrapper.Current.CurrentSite.ExternalSiteId,
    //            endpoints);

    //        return menu;
    //    }
    //}
}