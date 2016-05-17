using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess
{
    public class SitesDataAccess : ISiteDataAccess
    {
        private readonly AndroAdminDbContext dbContext;

        public SitesDataAccess(AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<string> GetExternalAcsApplicationIds(int siteId)
        {
            var answer = Enumerable.Empty<string>();

            var table = this.dbContext.ACSApplications;
            var query = table
                                .Where(e => e.ACSApplicationSites.Any(site => site.SiteId == siteId))
                                .Select(e => e.ExternalApplicationId);
            var result = query.ToArray();

            answer = result;

            return answer;
        }

        public IEnumerable<string> GetExternalApplicationIds(int siteId)
        {
            var answer = Enumerable.Empty<string>();

            var query = this.dbContext
            .ACSApplications
                            .Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId))
                            .Select(e => e.ExternalApplicationId);
                
            var result = query.ToArray();

            answer= result;

            return answer;
        }

        public IEnumerable<int> GetAcsApplicationIds(int siteId)
        {
            var answer = Enumerable.Empty<int>();

            var query = this.dbContext
                .ACSApplications
                .Where(e => e.ACSApplicationSites.Any(acsSite => acsSite.SiteId == siteId));

            var result = query.Select(e => e.Id).ToArray();
            answer = result;

            return answer;
        }

        public Domain.SiteDomainModel GetById(int siteId)
        {
            Domain.SiteDomainModel site = null;
            var table = this.dbContext.Stores.Include(e => e.Address);
            var query = table.Where(e => e.Id == siteId);

            Store entity = query.FirstOrDefault();

            if (entity != null)
            {
                site = entity.ToDomainModel();
            }

            return site;
        }
    }
}
