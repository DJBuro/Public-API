using System;
using System.Linq;
using MyAndromeda.Logging;
using MyAndromeda.WebApiClient.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyAndromeda.WebApiClient.Syncing
{
    public class SyncWebCallerController : ISyncWebCallerController
    {
        private readonly IWebApiClientContext context;

        private readonly IMyAndromedaLogger logger;

        public SyncWebCallerController(IWebApiClientContext context, IMyAndromedaLogger logger)
        { 
            this.context = context;
            this.logger = logger;
        }

        public async Task<bool> RequestMenuSyncAsync(int andromedaSiteId) 
        { 
            var result = false;
            var address = new Uri(context.BaseAddress);

            try
            {
                this.logger.Debug("Create client: {0}", context.BaseAddress);
 
                using (var client = ClientHelper.GetANewJsonClient(address))
                {
                    var model = new MenuSyncModel() { AndromedaSiteId = andromedaSiteId };
                    var endpoint = context.SyncingMenuEndpoint.Replace("{andromedaSiteId}", andromedaSiteId.ToString());
                    
                    this.logger.Debug("Await response form " + endpoint);
                    var response = await client.PostAsJsonAsync(endpoint, model);
                    result = response.IsSuccessStatusCode;
                    
                    this.logger.Debug("Task successful? " + response.IsSuccessStatusCode);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = await response.Content.ReadAsStringAsync();
                        
                        this.logger.Error(msg);
                    }
                }

            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }
    }
}
