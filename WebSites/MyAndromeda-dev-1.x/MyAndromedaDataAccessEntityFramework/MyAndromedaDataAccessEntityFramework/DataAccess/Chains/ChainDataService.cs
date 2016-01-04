using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model.AndroAdmin;

namespace MyAndromeda.Data.DataAccess.Chains
{
    public class ChainDataService : IChainDataService 
    {
        private readonly AndroAdminDbContext dbContext;

        public ChainDataService(AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Domain.Site> GetSiteList(int chainId)
        {
            IEnumerable<Domain.Site> sites;
        
            var table = this.dbContext.Stores.Include(e => e.Address);

            var query = table.Where(e => e.Chain.Id == chainId);
            var results = query.ToArray();

            sites = results.Select(e => e.ToDomainModel());

            return sites;
        }

        public void Update(Domain.Chain chain)
        {
            var table = this.dbContext.Chains;
            var query = table.Where(e => chain.Id == e.Id);
            var result = query.Single();

            result.Name = chain.Name;
            result.Culture = chain.Culture;

            this.dbContext.SaveChanges();
        }

        public Data.Domain.Chain Get(int chainId)
        {
            Domain.Chain entity = null;
            var table = this.dbContext.Chains;
            var query = table.Where(e => e.Id == chainId).ToArray();
            var result = query.SingleOrDefault();

            entity = new Domain.Chain()
            {
                Id = result.Id,
                Name = result.Name,
                Culture = result.Culture,
                MasterMenuId = result.MasterMenuId
            };
            
            return entity;
        }

        public IEnumerable<Domain.Chain> List(Expression<Func<Chain, bool>> query)
        {
            IEnumerable<Domain.Chain> chains = Enumerable.Empty<Domain.Chain>();

            var table = this.dbContext.Chains;
            var tableQuery = table.Where(query);
            var result = tableQuery.ToArray();

            chains = result.Select(e => new Domain.Chain()
            {
                Id = e.Id,
                Name = e.Name,
                Culture = e.Culture,
                MasterMenuId = e.MasterMenuId
            }).ToArray();

            return chains;
        }
    }
}