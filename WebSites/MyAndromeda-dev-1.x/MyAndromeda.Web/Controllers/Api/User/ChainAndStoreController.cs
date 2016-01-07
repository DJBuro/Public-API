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
                .Where(e => usersChains.Contains(e.Id))
                .OrderBy(e=> e.Name);

            var projectionQuery = chainsQuery
                .Select(e => new { 
                    Id = e.Id,
                    e.Name,
                    StoreCount = e.Stores.Count,
                    ParentId = e.Parents
                                        //.Where(parent => parent.Id != e.Id)
                                        .Select(chainChain => chainChain.ParentChainId)
                                        .FirstOrDefault()
                });

            var projectionResult = await projectionQuery.ToArrayAsync();

            var result = projectionResult.Select(e => new ChainModel
            {  
                Id = e.Id,
                Name = e.Name,
                StoreCount = e.StoreCount,
                ChildChainCount = projectionResult.Count(chain => chain.ParentId == e.Id),
                ChildStoreCount = CountRecursivelyChildStores(
                        projectionResult, 
                        e,
                        d => d.Id,
                        id => projectionResult.Where(d=> d.ParentId == id),
                        d => d.StoreCount),
                //, d => d.ParentId == e.Id, d => d.StoreCount), 
                //projectionResult.Select(e=> e.ParentId == )
                ParentId = e.ParentId == 0 
                            ? new Nullable<int>() 
                            : e.ParentId
            });

            return result;

        }

        private int CountRecursivelyChildStores<A>(
            IEnumerable<A> allModels,         
            A model,
            Func<A, int> idFunc,                              //current node in the chain 
            Func<int, IEnumerable<A>> childrenFunc, //results from all models
            Func<A, int> sum) 
        {
            var total = 0;
            var children = childrenFunc(idFunc(model));
            foreach (var item in children)
            {
                total += sum(item);
                total += CountRecursivelyChildStores(allModels, item, idFunc, childrenFunc, sum);
            }
            return total;
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

            var storesQuery = this.androAdminDataContext.Stores
                .Where(e => e.ChainId == chainId)
                .OrderBy(e=> e.Name);

            var projection = storesQuery.Select(e => new StoreModel() { 
                AndromedaSiteId = e.AndromedaSiteId,
                Name = e.Name,
                ExternalSiteId = e.ExternalId,
                HasRameses = e.StoreDevices.Count == 0 || e.StoreDevices.Any(storeDevice => storeDevice.Device.Name == "Rameses") 
            });

            var results = await projection.ToArrayAsync();

            return results;
        }


    }

    public class ChainModel 
    {
        public string Name { get; set; }

        public int StoreCount { get; set; }

        public int? ParentId { get; set; }

        public int Id { get; set; }

        public int ChildChainCount { get; set; }

        public int ChildStoreCount { get; set; }
    }

    public class StoreModel 
    {
        public string Name { get; set; }

        public string ExternalSiteId { get; set; }

        public int AndromedaSiteId { get; set; }
        public bool HasRameses { get; set; }
    }
}