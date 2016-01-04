using AndroAdminDataAccess.Domain.WebOrderingSetup;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAndromeda.Services.WebOrdering.Services 
{
    public interface IAndroWebOrderingWebSiteService : IDependency
    {
        void Delete(AndroWebOrderingWebsite website);

        void Update(AndroWebOrderingWebsite website);

        string SerializeConfigurations(WebSiteConfigurations config);

        WebSiteConfigurations DeSerializeConfigurations(string config);

        AndroWebOrderingWebsite GetWebOrderingWebsite(System.Linq.Expressions.Expression<Func<AndroWebOrderingWebsite, bool>> predicate);

        AndroWebOrderingTheme GetTheme(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate);
        
        IQueryable<AndroWebOrderingTheme> ListThemes(System.Linq.Expressions.Expression<Func<AndroWebOrderingTheme, bool>> predicate);
        
        IQueryable<AndroWebOrderingTheme> ListThemes();
        //Task<MyAndromedaMenu> GetMenuDataFromEndpointsAsync(int andromedaSiteId, string externalSiteId, IEnumerable<string> endpoints);
        //Task<MyAndromedaMenu> GetMenuData();
    }
}
