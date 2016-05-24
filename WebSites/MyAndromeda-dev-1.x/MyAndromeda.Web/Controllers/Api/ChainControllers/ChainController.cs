using MyAndromeda.Data.Domain;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Controllers.Api.ChainControllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Core.Linq;
using System.Data.Entity;

namespace MyAndromeda.Web.Controllers.Api.ChainControllers
{
    public class ChainController : ApiController 
    {
        private readonly AndroAdminDbContext androAdminDataContext;
        private readonly IAuthorizer authorizer;
        private readonly ICurrentUser currentUser;

        public ChainController(IAuthorizer authorizer, ICurrentUser currentUser, AndroAdminDbContext androAdminDataContext) {
            this.authorizer = authorizer;
            this.currentUser = currentUser;
            this.androAdminDataContext = androAdminDataContext;
        }

        [HttpGet]
        [Route("data/admin/chains")]
        public ChainDomainModel[] ListEditModels()
        {
            //good enough to throw from the controller: 
            ChainDomainModel[] accessibleChains = this.currentUser.AccessibleChains;

            return accessibleChains;
        }

        [HttpPost]
        [Route("data/admin/chains/update")]

        public async Task<ChainDomainModel> AddOrUpdate(ChainDomainModel viewModel)
        {
            if (viewModel.Id == 0)
            {

            }

            return viewModel;
        }

        [HttpPost]
        [Route("admin/chains/destroy")]
        public async Task<object> Destroy(ChainDomainModel viewModel)
        {
            IEnumerable<ChainDomainModel> branchNodes = viewModel.Items.Flatten(e => e.Items);
            int[] branchIds = branchNodes.Select(e => e.Id).ToArray();
            
            string[] storesInTheWay = await this.androAdminDataContext.Stores.Where(e => branchIds.Contains(e.ChainId))
                .Select(e=> e.Chain.Name).ToArrayAsync();

            if (storesInTheWay.Any())
            {
                return this.BadRequest("The chain cannot be removed as the following chains still have stores:" + string.Join(",", storesInTheWay));
            }

            return viewModel;
        }
    }
}
