using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using System.Net.Http;
using MyAndromeda.Services.WebHooks.Extensions;
using MyAndromeda.WebApiClient;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System.Net;
using MyAndromeda.Services.WebHooks.Models;

namespace MyAndromeda.Services.WebHooks.Handlers
{
    public class MenuPublishedHandler : IMenuPublishEvents
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IWebApiClientContext webApiClientContext;

        private readonly WebhookEndpointManger endpoints;


        public MenuPublishedHandler(IWebApiClientContext webApiClientContext, IMyAndromedaLogger logger, WebhookEndpointManger endpoints)
        {
            this.logger = logger;
            this.endpoints = endpoints;
            this.webApiClientContext = webApiClientContext;
        }

        public async Task Publishing(MenuPublishContext context)
        {
            return;
        }

        public async Task Published(MenuPublishContext context)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(this.webApiClientContext.BaseAddress);

                    client.AddJson();
                    client.AddAuth();

                    var model = new Models.MenuChange()
                    {
                        Source = "MyAndromeda.Services.WebHooks.Handlers.OrderChangedEventHandler",
                        AndromedaSiteId = context.AndromedaSiteId,
                        Version = "Unknown"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(this.endpoints.MenuVersion, model);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format("Notify - Could not call : {0}", this.endpoints.OrderStatus);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        throw new WebException(message, new Exception(responseMessage));
                    }
                }
            }
            catch (Exception e)
            {
                this.logger.Error("Error updating the version has changed");
                this.logger.Error(e);
            }

            
        }
    }
}
