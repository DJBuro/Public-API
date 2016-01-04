using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;
using MyAndromeda.WebApiClient;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Services.WebHooks.Extensions;

namespace MyAndromeda.Services.WebHooks.Handlers
{
    public class OrderChangedEventHandler : IOrderChangedEvent
    {
        private readonly IMyAndromedaLogger logger;

        private readonly IStoreDataService storeDataService;
        private readonly IWebApiClientContext webApiClientContext;

        private readonly WebhookEndpointManger endpoints;

        public OrderChangedEventHandler(
            IMyAndromedaLogger logger,
            IWebApiClientContext webApiClientContext, 
            IStoreDataService storeDataService,
            WebhookEndpointManger endpoints )
        {
            this.logger = logger;
            this.storeDataService = storeDataService;
            this.webApiClientContext = webApiClientContext;
            this.endpoints = endpoints;
        }

        public string Name
        {
            get
            {
                return "WebHook notification - order status";
            }
        }

        public async Task<WorkLevel> OrderStatusChangedAsync(int andromedaSiteId,  OrderHeader orderHeader, OrderStatu oldStatus)
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri(this.webApiClientContext.BaseAddress);

                client.AddJson();
                client.AddAuth();

                var model = new Models.OrderStatusChange() 
                {
                    AcsApplicationId = orderHeader.ApplicationID,
                    AndromedaSiteId = andromedaSiteId,
                    ExternalSiteId = orderHeader.ExternalSiteID,
                    InternalOrderId = orderHeader.ID,
                    ExternalOrderId = orderHeader.ExternalOrderRef,
                    Source = "MyAndromeda.Services.WebHooks.Handlers.OrderChangedEventHandler",
                    Status = orderHeader.Status,
                    StatusDescription = orderHeader.OrderStatu.Description
                };

                HttpResponseMessage response = await client.PostAsJsonAsync(this.endpoints.OrderStatus, model);

                if (!response.IsSuccessStatusCode)
                {
                    string message = string.Format("Notify - Could not call : {0}", this.endpoints.OrderStatus);
                    string responseMessage = await response.Content.ReadAsStringAsync();

                    this.logger.Error(message);
                    this.logger.Error(responseMessage);

                    throw new WebException(message, new Exception(responseMessage));
                }
            }

            return WorkLevel.CompletedWork;
        }
    }
}