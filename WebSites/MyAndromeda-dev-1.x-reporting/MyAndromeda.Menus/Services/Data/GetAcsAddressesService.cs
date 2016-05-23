using System;
using System.Collections.Generic;
using System.Linq;
using System.Monads;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Services;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Logging;
using Newtonsoft.Json.Linq;

namespace MyAndromeda.Menus.Services.Data
{
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

        //[WebInvoke(Method = "POST", UriTemplate = "customers/{username}/updateloyalty/{action}/{externalOrderRef}?applicationId={applicationId}")]
        public async Task<IEnumerable<string>> GetLoyaltyEndpointsAsync(string userName, string action, string externalOrderRef, string applicationId) 
        {
            var publicEndpointList = await this.GetPublicAcsEndpointsAsync(applicationId, 2);
            var customerLoyalty = string.Format("/customers/{0}/updateloyalty/{1}/{2}?applicationId={3}", userName, action, externalOrderRef, applicationId);
            var specific = publicEndpointList.Select(e => string.Concat(e, customerLoyalty)).ToArray();

            return specific;
        }

        private async Task<IEnumerable<string>> GetPublicAcsEndpointsAsync(string applicationId, int version) 
        {
            this.logger.Debug("Query signpost for endpoints. [ApplicationId : {0};]", applicationId);

            var endpoints = await this.GetPublicServiceEndpointsAsync(applicationId, version);
            endpoints.Check(e => endpoints.Count() > 0, e => { return new Exception("no service endpoints were provided"); });

            return endpoints;
        }

        private async Task<IEnumerable<string>> GetMenuEndpointsAsync(string externalSiteId, string applicationId)
        {
            var endpoints = await this.GetPublicAcsEndpointsAsync(applicationId, 1);

            //finish menu service endpoint. 
            var siteSpecific = string.Format("/menu/{0}?applicationId={1}", externalSiteId, applicationId);
            var menuEndpoints = endpoints.Select(e => string.Concat(e, siteSpecific)).ToArray();

            return menuEndpoints;
        }

        private async Task<IEnumerable<string>> GetPublicServiceEndpointsAsync(string applicationId, int version)
        {
            applicationId.Check(e => !string.IsNullOrWhiteSpace(e), (_) => { throw new ArgumentException("applicationId cannot be null/empty"); });

            //wait here for:
            //1. get pointer servers 
            var serverAddressesRaw = await apiServiceDataAccess.FetchServerAddressesAsync(applicationId);
            //translate them to have the correct ids 
            var serverAddresses = TranslateJson(serverAddressesRaw, version).ToArray();

            return serverAddresses;
        }

        private static IEnumerable<string> TranslateJson(string output, int version)
        {
            dynamic element = JArray.Parse(output);

            foreach (dynamic host in element)
            {
                if (host.type != "WebOrderingAPI")
                {
                    continue;
                }
                if (host.version != version)
                {
                    continue;
                }

                yield return host.url.Value;
            }

            yield break;
        }
    }
}