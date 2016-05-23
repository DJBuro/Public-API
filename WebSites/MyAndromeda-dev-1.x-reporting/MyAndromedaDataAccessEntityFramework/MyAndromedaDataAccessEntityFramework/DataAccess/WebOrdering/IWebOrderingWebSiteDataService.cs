using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Core.Data;
using MyAndromeda.Data.Model.AndroAdmin;
using System.Data.Entity;

namespace MyAndromeda.Data.DataAccess.WebOrdering
{
    public interface IWebOrderingWebSiteDataService : IDataProvider<AndroWebOrderingWebsite>, IDependency
    {
        void Update(AndroWebOrderingWebsite website);

        AndroWebOrderingWebsite GetWebOrderingSiteForOrder(int applicationId, string externalSiteId);

        void Delete(AndroWebOrderingWebsite website);

        DbSet<AndroWebOrderingWebsite> Table { get; }
    }
}
