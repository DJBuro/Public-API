using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Data.DataAccess.Sites
{
    public class SiteDataService : ISiteDataService 
    {
        private readonly AndroAdminDbContext dbContext;

        public SiteDataService(AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<SiteDomainModel> List(Expression<Func<Store, bool>> query)
        {
            IEnumerable<SiteDomainModel> sites;

            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                var table = this.dbContext.Stores.Include(e => e.Address);
                var storeQuery = table.Where(query);
                var storeResult = storeQuery.ToArray();

                sites = storeResult.Select(e => e.ToDomainModel()).ToArray();
            }

            return sites;
        }

        public IEnumerable<TResult> ListAndTransform<TResult>(Expression<Func<Store, bool>> query, Expression<Func<Store, TResult>> transform)
        {
            IEnumerable<TResult> data;

            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                var table = this.dbContext.Stores;
                var storeQuery = table.Where(query).Select(transform);
                data = storeQuery.ToArray();
            }

            return data;
        }

        //public bool SiteBelongsToChain(int siteId) 
        //{
        //    Store store;
        //    using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext()) 
        //    {
        //        var table = db
        //    }
        //}
        public IEnumerable<int> GetAcsApplicationIds(int siteId)
        {
            IEnumerable<int> result = Enumerable.Empty<int>();

            var query = this.dbContext.ACSApplications.Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId));
            result = query.Select(e => e.Id).ToArray();

            return result;
        }

        public IEnumerable<string> GetExternalApplicationIds(int siteId)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();

            var query = this.dbContext
            .ACSApplications
                            .Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId))
                            .Select(e => e.ExternalApplicationId);

            result = query.ToArray();

            return result;
        }

        public IEnumerable<string> GetExternalAcsApplicationIds(int siteId)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();

            var table = this.dbContext.ACSApplications;
            var query = table
                             .Where(e => e.ACSApplicationSites.Any(site => site.SiteId == siteId))
                             .Select(e => e.ExternalApplicationId);
            result = query.ToArray();

            return result;
        }
    }
}