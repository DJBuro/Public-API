using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Helpers;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;
using MyAndromeda.Framework.Services.WebServices;

namespace MyAndromeda.Data.AcsServices.Services
{
    public interface IAndroCloudApiServiceDataAccess : IDependency
    {
        Task<string> FetchServerAddressesAsync(string applicationId); 
    }

    public class AndroClousApiServiceDataAccess : IAndroCloudApiServiceDataAccess
    {
        private readonly ISignPostUrlService signPostService;
        private readonly IMyAndromedaLogger logger;
        private readonly IDelay delay;

        public AndroClousApiServiceDataAccess(ISignPostUrlService signPostUrlService, IMyAndromedaLogger logger, IDelay delay) 
        {
            this.delay = delay;
            this.logger = logger;
            this.signPostService = signPostUrlService;
        }

        public Task<string> FetchServerAddressesAsync(string applicationId)
        {
            var signpostUrls = signPostService.SignPostUrls;
            var urls = signpostUrls.Select(e=> string.Format(e, applicationId));

            var queryServicesTask = this.QueryServices(urls);

            if (queryServicesTask.Exception != null) 
            { 
                throw queryServicesTask.Exception;
            }

            return queryServicesTask;
        }

        private async Task<string> QueryServices(IEnumerable<string> urls)
        {
            logger.Debug("Query url should look signpost: https://signpost1.androcloudservices.com/AndroCloudAPI/weborderapi/host?applicationId={0}");
            logger.Debug("Query urls in turn:");
            logger.DebugItems(urls, (url) => string.Format("Url: {0}", url));

            foreach (var url in urls) 
            {
                logger.Debug("query signpost service {0} with a timeout: {1} seconds", url, delay.DelayInSeconds);
                var fetchTask = WebRequestHelper.GrabARequestAsync(url, this.delay, ResponseType.Json);
                
                var result = await fetchTask;
                if (fetchTask.Exception != null) 
                {
                    logger.Error(fetchTask.Exception.Message);
                    continue;
                }

                logger.Info(string.Format("{0}", result));

                return result;
            }

            throw new ArgumentOutOfRangeException("urls", "Service Urls failed to provide any information in a timely manner.");
        }
    }
}