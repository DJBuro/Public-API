using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Data.Model;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Web.ViewModels.Api;
using CloudSync;
using MyAndromeda.Web.Areas.Loyalty;
using MyAndromeda.Data.Model.AndroAdmin;
namespace MyAndromeda.Web.Controllers.Api
{
    public class LoyaltyController : ApiController
    {
        private readonly IAuthorizer authorizer;
        private readonly IWorkContext workContext;
        private readonly INotifier notifier;
        private readonly ISynchronizationTaskService synchronizationTaskService;
        
        public LoyaltyController(IWorkContext workContext, INotifier notifier, ISynchronizationTaskService synchronizationTaskService, IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.synchronizationTaskService = synchronizationTaskService;
            this.notifier = notifier;
            this.workContext = workContext;
        }

        [HttpGet]
        [Route("api/{AndromedaSiteId}/loyalty/types")]
        public IEnumerable<string> Types() 
        {
            if (!this.authorizer.Authorize(LoyaltyUserPermissions.ViewLoyalty)) 
            {
                throw new UnauthorizedAccessException();
            }
            //well there isn't a 'loyalty' repository ... 
            
            var names = new[] { "Andromeda" };
            IEnumerable<string> result = names;

            using (var dbContext = new AndroAdminDbContext())
            {
                
                var currentLoyalties = dbContext.StoreLoyalties
                    .Where(e => e.StoreId == this.workContext.CurrentSite.Store.Id)
                    .Select(e=> e.ProviderName)
                    .ToArray();

                result = names.Where(e => !currentLoyalties.Contains(e));
            }

            return result;
        }

        [HttpGet]
        [Route("api/{AndromedaSiteId}/loyalty/list")]
        public IEnumerable<LoyaltyProviderViewModel> List() 
        {
            if (!this.authorizer.Authorize(LoyaltyUserPermissions.ViewLoyalty))
            {
                throw new UnauthorizedAccessException();
            }

            IEnumerable<LoyaltyProviderViewModel> result = Enumerable.Empty<LoyaltyProviderViewModel>();

            using (var dbContext = new AndroAdminDbContext())
            {
                //var storeLoyalty = workContext.CurrentSite.Store.StoreLoyalties.Where(e => e.ProviderName == name).FirstOrDefault();

                var loyalty = dbContext.StoreLoyalties
                    .Where(e => e.StoreId == this.workContext.CurrentSite.Store.Id).ToArray();

                result = loyalty.Select(e => e.ToViewModel());
            }

            return result;
        }

        [HttpGet]
        [Route("api/{AndromedaSiteId}/loyalty/get/{Name}")]
        public LoyaltyProviderViewModel Get(string name)
        {
            if (!this.authorizer.Authorize(LoyaltyUserPermissions.ViewLoyalty))
            {
                throw new UnauthorizedAccessException();
            }

            LoyaltyProviderViewModel result;
            using (var dbContext = new AndroAdminDbContext())
            {
                var loyalty = dbContext.StoreLoyalties
                    .Where(e => e.StoreId == this.workContext.CurrentSite.Store.Id)
                    .Where(e => e.ProviderName == name).FirstOrDefault();

                result = loyalty.ToViewModel();

                if (result == null) 
                {
                    result = new LoyaltyProviderViewModel()
                    {
                        ProviderName = name,
                        Configuration = new object()
                    };
                }
            }

            return result;
        }

        // POST api/<controller>
        [Route("api/{AndromedaSiteId}/loyalty/update/{Name}")]
        public void Update([FromUri]string name, [FromBody]LoyaltyProviderViewModel viewModel)
        {
            if (!this.authorizer.Authorize(LoyaltyUserPermissions.EditLoyalty))
            {
                throw new UnauthorizedAccessException();
            }

            using (var dbContext = new AndroAdminDbContext())
            {
                var loyalty = dbContext.StoreLoyalties
                    .Where(e => e.StoreId == this.workContext.CurrentSite.Store.Id)
                    .Where(e => e.ProviderName == name).SingleOrDefault();

                loyalty = loyalty.UpdateFromViewModel(viewModel);

                if (loyalty.Id == null || loyalty.Id == default(Guid)) 
                {
                    loyalty.Id = Guid.NewGuid();
                    loyalty.StoreId = this.workContext.CurrentSite.Store.Id;
                    dbContext.StoreLoyalties.Add(loyalty);
                }

                loyalty.DataVersion = dbContext.GetNextDataVersionForEntity();

                dbContext.SaveChanges();
            }

            this.notifier.Notify("Saving changes");

            SyncHelper.ServerSync();

            this.notifier.Success("Loyalty changes are complete!");
        }
    }
}