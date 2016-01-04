using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Helpers;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Data;
using MyAndromedaDataAccessEntityFramework.DataAccess;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;

namespace MyAndromeda.Services.Loyalty
{
    public interface ICommitLoyaltyChangesService : IDependency
    {
        Task DeclineLoyaltyPoints(OrderHeader orderHeader);

        Task CommitLoyaltyPoints(OrderHeader orderHeader);
    }

    public class CommitLoyaltyChangesService : ICommitLoyaltyChangesService
    {
        private readonly IGetAcsAddressesService getAcsAddressesService;
        private readonly IAcsApplicationDataService acsApplicationDataService;

        private readonly IDelay delay;

        private readonly IMyAndromedaLogger logger;

        public CommitLoyaltyChangesService(IGetAcsAddressesService getAcsAddressesService, IDelay delay, IMyAndromedaLogger logger, IAcsApplicationDataService acsApplicationDataService) 
        {
            this.acsApplicationDataService = acsApplicationDataService;
            this.delay = delay;
            this.logger = logger;
            this.getAcsAddressesService = getAcsAddressesService;
        }

        private async Task<IEnumerable<string>> GetEndpoints(string userName, string action, string externalOrderRef, string applicationId) 
        {
            var endpoints = await this.getAcsAddressesService.GetLoyaltyEndpointsAsync(userName, action, externalOrderRef, applicationId);
            return endpoints;
        }

        public async Task DeclineLoyaltyPoints(OrderHeader orderHeader)
        {
            var application = acsApplicationDataService.Get(orderHeader.ApplicationID);
            var endpoints = await this.GetEndpoints(orderHeader.Customer.CustomerAccount.Username, "Reject", orderHeader.ExternalOrderRef, application.ExternalApplicationId);
            
            foreach (var endpoint in endpoints) 
            {
                var requestTask = WebRequestHelper.Post(endpoint, this.delay, MyAndromeda.Data.AcsServices.Models.ResponseType.Json);
                var result = await requestTask;

                if (requestTask.Exception != null) { break; }
                else { logger.Error(requestTask.Exception); }
            }
        }

        public async Task CommitLoyaltyPoints(OrderHeader orderHeader)
        {
            var application = acsApplicationDataService.Get(orderHeader.ApplicationID);
            var endpoints = await this.GetEndpoints(orderHeader.Customer.CustomerAccount.Username, "Commit", orderHeader.ExternalOrderRef, application.ExternalApplicationId);

            foreach (var endpoint in endpoints)
            {
                var requestTask = WebRequestHelper.Post(endpoint, this.delay, MyAndromeda.Data.AcsServices.Models.ResponseType.Json);
                var result = await requestTask;

                if (requestTask.Exception != null) { logger.Error(requestTask.Exception); }
                else { break; }
            }
        }
    }
}
