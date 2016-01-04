using System.Collections.Generic;
using System.Monads;
using System.Threading.Tasks;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Logging;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System;
using MyAndromeda.Data.AcsServices.Services;
using Newtonsoft.Json.Linq;

namespace MyAndromeda.Menus.Services.Data
{
    public interface IGetAcsAddressesService : IDependency 
    {
        /// <summary>
        /// Gets the menu endpoints async.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        Task<IEnumerable<string>> GetMenuEndpointsAsync(Store store);
    }

    public class GetAcsAddressesService : IGetAcsAddressesService 
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IAndroCloudApiServiceDataAccess apiServiceDataAccess;

        public GetAcsAddressesService(IMyAndromedaLogger logger, IAndroCloudApiServiceDataAccess apiServiceDataAccess)
        { 
            this.apiServiceDataAccess = apiServiceDataAccess;
            this.logger = logger;
        }

        public async Task<IEnumerable<string>> GetMenuEndpointsAsync(Store store)
        {
            var applications = store.ACSApplicationSites.Select(e => e.ACSApplication.ExternalApplicationId);

            var first = applications.FirstOrDefault();

            if (first == null) 
            {
                logger.Error("Store does not have a application: ", store.AndromedaSiteId);
                return Enumerable.Empty<string>();
            }

            var result = await this.GetMenuEndpointsAsync(store.ExternalId, first);

            return result;
        }

        private async Task<IEnumerable<string>> GetMenuEndpointsAsync(string externalSiteId, string applicationId)
        {
            this.logger.Debug("Query services for endpoint. [ApplicationId : {0}; ExternalSiteSiteId: {1}]", applicationId, externalSiteId);

            var endpoints = await this.GetMenuServiceEndpointsAsync(applicationId);
            endpoints.Check(e => endpoints.Count() > 0, e => { return new Exception("no service endpoints were provided"); });

            //finish menu service endpoint. 
            var siteSpecific = string.Format("/menu/{0}?applicationId={1}", externalSiteId, applicationId);
            var menuEndpoints = endpoints.Select(e => string.Concat(e, siteSpecific)).ToArray();

            return menuEndpoints;
        }

        private async Task<IEnumerable<string>> GetMenuServiceEndpointsAsync(string applicationId)
        {
            applicationId.Check(e => !string.IsNullOrWhiteSpace(e), (_) => { throw new ArgumentException("applicationId cannot be null/empty"); });

            //wait here for:
            //1. get pointer servers 
            var serverAddressesRaw = await apiServiceDataAccess.FetchServerAddressesAsync(applicationId);
            //translate them to have the correct ids 
            var serverAddresses = TranslateJson(serverAddressesRaw).ToArray();

            return serverAddresses;
        }

        private static IEnumerable<string> TranslateJson(string output)
        {
            dynamic element = JArray.Parse(output);

            foreach (dynamic host in element)
            {
                yield return host.url.Value;
            }

            yield break;
        }
    }
}