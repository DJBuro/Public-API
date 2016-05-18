using System;
using System.Linq;
using System.Collections.Generic;
using MyAndromeda.Framework.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Logging;
using MyAndromeda.Data.Domain;
using MyAndromeda.Web.Controllers.Api.User.Models;

namespace MyAndromeda.Web.Controllers.Api.User
{

    [MyAndromedaAuthorize]
    public class AuthorizationCheckController : ApiController
    {
        private readonly ICurrentUser currentUser;
        private readonly DbSet<MyAndromeda.Data.Model.AndroAdmin.Store> stores;
        private readonly DbSet<UserChain> userChains;
        private readonly DbSet<UserStore> userStores;

        public AuthorizationCheckController(ICurrentUser currentUser, 
            AndroAdminDbContext androAdminDataContext,
            MyAndromedaDbContext myAndromedaDataContext) {
            this.currentUser = currentUser;
            this.userChains = myAndromedaDataContext.UserChains;
            this.userStores = myAndromedaDataContext.UserStores;
            this.stores = androAdminDataContext.Stores;
        }

        
        [HttpGet]
        [Route("user/chain/{chainId}/count")]
        public async Task<UserCountModel> GetUserCountForChain([FromUri]int chainId)
        {
            //preferred just a count ... will see how 
            int[] chainUsers = await this.userChains
                .Where(e => e.ChainId == chainId)
                .Where(e => e.UserRecord.IsEnabled)
                .Select(e => e.UserRecordId)
                .ToArrayAsync();

            int[] stores = this.stores.Where(e => e.ChainId == chainId).Select(e => e.Id).ToArray();

            int[] storeUsers = await this.userStores
                .Where(userStore => stores.Any(k => k == userStore.StoreId))
                .Where(e=> e.UserRecord.IsEnabled)
                .Select(e => e.UserRecordId).ToArrayAsync();
            //this.UserRecords.Where(e => e.UserStores.Any(userStore => stores.Any(k => k == userStore.StoreId)));
            
            int count = chainUsers.Union(storeUsers).Distinct().Count();

            return new UserCountModel() {
                Count = count,
                ChainUsersCount = chainUsers.Count(),
                StoreUsersCount = chainUsers.Count()
            };
        }
    }

}