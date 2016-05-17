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
        private readonly DbSet<UserChain> userChains;

        public AuthorizationCheckController(ICurrentUser currentUser, MyAndromedaDbContext myAndromedaDataContext) {
            this.currentUser = currentUser;
            this.userChains = myAndromedaDataContext.UserChains;
        }

        
        [HttpGet]
        [Route("user/chain/{chainId}/count")]
        public async Task<UserCountModel> GetUserCountForChain([FromUri]int chainId)
        {
            int count = await this.userChains
                .Where(e => e.ChainId == chainId)
                .Where(e => e.UserRecord.IsEnabled)
                .CountAsync();
            
            return new UserCountModel() {
                Count = count 
            };
        }
    }

}