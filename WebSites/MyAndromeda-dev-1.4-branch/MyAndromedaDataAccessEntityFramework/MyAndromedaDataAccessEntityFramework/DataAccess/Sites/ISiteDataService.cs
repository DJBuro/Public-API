using MyAndromeda.Core;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System.Linq.Expressions;
using MyAndromeda.Core.Data;

namespace MyAndromedaDataAccessEntityFramework.DataAccess.Sites
{
    public interface ISiteDataService : IDependency
    {
        IEnumerable<Site> List(Expression<Func<Store, bool>> query);
        IEnumerable<TResult> ListAndTransform<TResult>(Expression<Func<Store, bool>> query, Expression<Func<Store, TResult>> transform);

        IEnumerable<int> GetAcsApplicationIds(int siteId);
        IEnumerable<string> GetExternalApplicationIds(int siteId);
        IEnumerable<string> GetExternalAcsApplicationIds(int siteId);
    }

    public class SiteDataService : ISiteDataService 
    {
        private readonly Model.AndroAdmin.AndroAdminDbContext dbContext;

        public SiteDataService(Model.AndroAdmin.AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Site> List(Expression<Func<Store, bool>> query)
        {
            IEnumerable<Site> sites;

            //using (var dbContext = new Model.AndroAdmin.AndroAdminDbContext())
            {
                var table = dbContext.Stores.Include(e => e.Address);
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
                var table = dbContext.Stores;
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

            var query = dbContext.ACSApplications.Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId));
            result = query.Select(e => e.Id).ToArray();

            return result;
        }

        public IEnumerable<string> GetExternalApplicationIds(int siteId)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();


            var query = dbContext
                    .ACSApplications
                    .Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId))
                    .Select(e => e.ExternalApplicationId);

            result = query.ToArray();

            return result;
        }

        public IEnumerable<string> GetExternalAcsApplicationIds(int siteId)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();

            var table = dbContext.ACSApplications;
            var query = table
                             .Where(e => e.ACSApplicationSites.Any(site => site.SiteId == siteId))
                             .Select(e => e.ExternalApplicationId);
            result = query.ToArray();

            return result;
        }
    }
}
