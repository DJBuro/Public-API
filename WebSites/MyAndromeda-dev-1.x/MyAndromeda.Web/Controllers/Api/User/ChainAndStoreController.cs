using MyAndromeda.Framework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace MyAndromeda.Web.Controllers.Api.User
{
    public class ChainAndStoreController : ApiController
    {
        readonly ICurrentUser currentUser; 
        readonly MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDataContext;

        public ChainAndStoreController(ICurrentUser currentUser,
            MyAndromeda.Data.Model.AndroAdmin.AndroAdminDbContext androAdminDataContext)
        { 
            this.androAdminDataContext = androAdminDataContext;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route("user/chains")]
        public async Task<IEnumerable<ChainModel>> List() 
        {
            var usersChains = this.currentUser.FlattenedChains.Select(e=> e.Id).ToArray();

            var chainsQuery = this.androAdminDataContext.Chains
                .Where(e => usersChains.Contains(e.Id));

            var projection = await chainsQuery
                .Select(e => new { 
                    e.Name,
                    StoreCount = e.Stores.Count,
                    ParentId = e.Parents.Select(parent => parent.Id).FirstOrDefault()
                }).ToArrayAsync();

            var result = projection.Select(e => new ChainModel
            {  
                Name = e.Name,
                StoreCount  = e.StoreCount,
                ParentId = e.ParentId
            });

            return result;

        }

        [HttpGet]
        [Route("user/chains/{chainId}")]
        public async Task<IEnumerable<StoreModel>> ListStores([FromUri]int chainId)
        {
            var usersChains = this.currentUser.FlattenedChains.ToArray();
            
            if(!usersChains.Any(e=> e.Id == chainId))
            {
                throw new Exception("Chain is inaccessible");
            }

            var storesQuery = this.androAdminDataContext.Stores.Where(e => e.ChainId == chainId);
            var projection = storesQuery.Select(e => new StoreModel() { 
                AndromedaSiteId = e.AndromedaSiteId,
                Name = e.Name,
                ExternalSiteId = e.ExternalId
            });

            var results = await projection.ToArrayAsync();

            return results;
        }


    }

    public class ChainModel 
    {
        public string Name { get; set; }

        public int StoreCount { get; set; }

        public int ParentId { get; set; }
    }

    public class StoreModel 
    {
        public string Name { get; set; }

        public string ExternalSiteId { get; set; }

        public int AndromedaSiteId { get; set; }
    }
}