using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using MyAndromeda.Services.WebHooks.Extensions;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.WebApiClient;

namespace MyAndromeda.Services.WebHooks.Handlers
{
    public class NotifyWebHooksForMenuItems : IMenuItemChangedEvent
    {
        private readonly IWebApiClientContext webApiClientContext;

        private readonly IMyAndromedaLogger logging;

        private readonly WebhookEndpointManger endpoints;

        public NotifyWebHooksForMenuItems(IWebApiClientContext webApiClientContext,
            IMyAndromedaLogger logging,
            WebhookEndpointManger endpoints)
        {
            this.endpoints = endpoints;
            this.webApiClientContext = webApiClientContext;
            this.logging = logging;
        }

        public async Task UpdatedMenuItemsAsync(EditMenuItemsContext context)
        {
            //to-do - notify webhook notification of updated menu items. 
            OutgoingWebHookMenuItemsChanged model = new OutgoingWebHookMenuItemsChanged()
            {
                AndromedaSiteId = context.Site.AndromediaSiteId,
                ExternalSiteId = context.Site.ExternalSiteId,
                Source = "MyAndromeda Menu Editor",
                Items = context.EditedItems.Select(e => new MenuItemChange()
                {
                    Id = e.Id,
                    Name = e.Name,
                    WebDescription = e.WebDescription,
                    WebName = e.WebName,
                    Enabled = e.Enabled
                }).ToList()
            };

            await this.CreateRequestAsync(model);
        }

        public async Task UpdatedToppingsAsync(EditToppingItemsContext context)
        {
            return;
        }

        private async Task CreateRequestAsync(OutgoingWebHookMenuItemsChanged model) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.webApiClientContext.BaseAddress);

                    client.AddJson();
                    client.AddAuth();

                    HttpResponseMessage response = await client.PostAsJsonAsync(this.endpoints.MenuItemsChanged, model);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format("Notify - Could not call : {0}", this.endpoints.MenuItemsChanged);
                        string responseMessage = await response.Content.ReadAsStringAsync();

                        throw new WebException(message, new Exception(responseMessage));
                    }
                }
            }
            catch (Exception e)
            {
                this.logging.Error(e);
            }
        }
    }
}