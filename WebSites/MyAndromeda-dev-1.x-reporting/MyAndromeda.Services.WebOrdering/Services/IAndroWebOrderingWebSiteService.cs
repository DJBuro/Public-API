using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.AndroAdmin;
using System;
using System.Linq;

namespace MyAndromeda.Services.WebOrdering.Services 
{
    public interface IAndroWebOrderingWebSiteService : IDependency
    {
        /// <summary>
        /// Deletes the specified website.
        /// </summary>
        /// <param name="website">The website.</param>
        void Delete(AndroWebOrderingWebsite website);

        /// <summary>
        /// Updates the specified website.
        /// </summary>
        /// <param name="website">The website.</param>
        void Update(AndroWebOrderingWebsite website);

        /// <summary>
        /// Serializes the configurations.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        string SerializeConfigurations(WebSiteConfigurations config);

        /// <summary>
        /// Des the serialize configurations.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        WebSiteConfigurations DeSerializeConfigurations(string config);

        /// <summary>
        /// Gets the web ordering website.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        AndroWebOrderingWebsite GetWebOrderingWebsite(System.Linq.Expressions.Expression<Func<AndroWebOrderingWebsite, bool>> predicate);

        /// <summary>
        /// Gets the theme.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        AndroWebOrderingTheme GetTheme(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate);
        
        /// <summary>
        /// Lists the themes.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<AndroWebOrderingTheme> ListThemes(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate);
        
        /// <summary>
        /// Lists the themes.
        /// </summary>
        /// <returns></returns>
        IQueryable<AndroWebOrderingTheme> ListThemes();
        //Task<MyAndromedaMenu> GetMenuDataFromEndpointsAsync(int andromedaSiteId, string externalSiteId, IEnumerable<string> endpoints);
        //Task<MyAndromedaMenu> GetMenuData();
    }
}
