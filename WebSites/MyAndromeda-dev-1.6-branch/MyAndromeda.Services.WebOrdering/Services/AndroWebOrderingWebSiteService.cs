using System;
using System.Linq;
using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Data.DataAccess.WebOrdering;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Services.WebOrdering.Services
{
    public class AndroWebOrderingWebSiteService : IAndroWebOrderingWebSiteService
    {
        private readonly IWebOrderingWebSiteDataService webOrderingWebSiteDataService;
        private readonly IWebOrderingThemeDataService webOrderingThemeDataService;

        public AndroWebOrderingWebSiteService(
            IWebOrderingWebSiteDataService webOrderingWebSiteDataService, 
            IWebOrderingThemeDataService webOrderingThemeDataService)
        {
            this.webOrderingWebSiteDataService = webOrderingWebSiteDataService;
            this.webOrderingThemeDataService = webOrderingThemeDataService;
        }

        public void Delete(AndroWebOrderingWebsite website)
        {
            this.webOrderingWebSiteDataService.Delete(website);
        }

        public void Update(AndroWebOrderingWebsite website)
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

        public AndroWebOrderingTheme GetTheme(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            return this.webOrderingThemeDataService.Get(predicate);
        }

        public IQueryable<AndroWebOrderingTheme> ListThemes(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate)
        {
            return this.webOrderingThemeDataService.List(predicate);
        }

        public IQueryable<AndroWebOrderingTheme> ListThemes()
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
