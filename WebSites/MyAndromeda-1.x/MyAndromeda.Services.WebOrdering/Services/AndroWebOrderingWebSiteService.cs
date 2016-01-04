using System;
using System.Linq;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromedaDataAccessEntityFramework.DataAccess.WebOrdering;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Services.Data;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Services.WebOrdering.Services
{
    public class AndroWebOrderingWebSiteService : IAndroWebOrderingWebSiteService
    {
        private readonly IWebOrderingWebSiteDataService webOrderingWebSiteDataService;
        private readonly IWebOrderingThemeDataService webOrderingThemeDataService;
        private readonly IAcsMenuServiceAsync acsMenuServiceService;
        private readonly ICurrentSite currentSite;
        private readonly IGetAcsAddressesService getAcsAddressesService;
        private readonly IStoreDataService storeDataService; 

        public AndroWebOrderingWebSiteService(
            IWebOrderingWebSiteDataService webOrderingWebSiteDataService,
            IWebOrderingThemeDataService webOrderingThemeDataService,
            IAcsMenuServiceAsync acsMenuServiceService,
            ICurrentSite currentSite,
            IGetAcsAddressesService getAcsAddressesService,
            IStoreDataService storeDataService)
        {
            this.webOrderingWebSiteDataService = webOrderingWebSiteDataService;
            this.webOrderingThemeDataService = webOrderingThemeDataService;
            this.acsMenuServiceService = acsMenuServiceService;
            this.currentSite = currentSite;
            this.getAcsAddressesService = getAcsAddressesService;
            this.storeDataService = storeDataService;
        }

        public void Update(MyAndromedaDataAccessEntityFramework.Model.AndroAdmin.AndroWebOrderingWebsite website)
        {
            this.webOrderingWebSiteDataService.Update(website);
        }

        public string SerializeConfigurations(WebSiteConfigurations config)
        {
            return WebSiteConfigurations.SerializeJson(config);
        }

        public WebSiteConfigurations DeSerializeConfigurations(string config)
        {
            return WebSiteConfigurations.DeserializeJson(config);
        }

        public AndroWebOrderingWebsite GetWebOrderingWebsite(System.Linq.Expressions.Expression<Func<AndroWebOrderingWebsite, bool>> predicate)
        {
            return this.webOrderingWebSiteDataService.Get(predicate);
        }

        public AndroWebOrderingTheme Get(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            return this.webOrderingThemeDataService.Get(predicate);
        }

        public IQueryable<AndroWebOrderingTheme> List(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            return this.webOrderingThemeDataService.List(predicate);
        }

        public IQueryable<AndroWebOrderingTheme> List()
        {
            return this.webOrderingThemeDataService.List();
        }

        //public async Task<MyAndromedaMenu> GetMenuDataFromEndpointsAsync(int andromedaSiteId, string externalSiteId, IEnumerable<string> endpoints)
        //{
        //    return await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(andromedaSiteId, externalSiteId, endpoints);
        //}

        //public async Task<MyAndromedaMenu> GetMenuData()
        //{
        //    var store = this.storeDataService.Get(e => e.AndromedaSiteId == currentSite.AndromediaSiteId);
        //    var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(store);
        //    return await this.acsMenuServiceService.GetMenuDataFromEndpointsAsync(currentSite.AndromediaSiteId, currentSite.ExternalSiteId, endpoints);
        //}
    }
}
