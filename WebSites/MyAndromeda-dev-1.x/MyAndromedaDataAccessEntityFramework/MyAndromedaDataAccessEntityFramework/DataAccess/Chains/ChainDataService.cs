using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess.Chains
{
    public class ChainDataService : IChainDataService 
    {
        private readonly AndroAdminDbContext dbContext;

        public ChainDataService(AndroAdminDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Domain.SiteDomainModel> GetSiteList(int chainId)
        {
            IEnumerable<Domain.SiteDomainModel> sites;
        
            var table = this.dbContext.Stores.Include(e => e.Address);

            var query = table.Where(e => e.Chain.Id == chainId);
            var results = query.ToArray();

            sites = results.Select(e => e.ToDomainModel());

            return sites;
        }

        public void Update(ChainDomainModel chain)
        {
            var table = this.dbContext.Chains;
            var query = table.Where(e => chain.Id == e.Id);
            var result = query.Single();

            result.Name = chain.Name;
            result.Culture = chain.Culture;

            this.dbContext.SaveChanges();
        }

        public ChainDomainModel Get(int chainId)
        {
            ChainDomainModel entity = null;
            var table = this.dbContext.Chains;
            var query = table.Where(e => e.Id == chainId).ToArray();
            var result = query.SingleOrDefault();

            entity = new Domain.ChainDomainModel()
            {
                Id = result.Id,
                Name = result.Name,
                Culture = result.Culture,
                MasterMenuId = result.MasterMenuId
            };
            
            return entity;
        }

        public IEnumerable<ChainDomainModel> List(Expression<Func<Chain, bool>> query)
        {
            IEnumerable<ChainDomainModel> chains = Enumerable.Empty<ChainDomainModel>();

            var table = this.dbContext.Chains;
            var tableQuery = table.Where(query);
            var result = tableQuery.ToArray();

            chains = result.Select(e => new ChainDomainModel()
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