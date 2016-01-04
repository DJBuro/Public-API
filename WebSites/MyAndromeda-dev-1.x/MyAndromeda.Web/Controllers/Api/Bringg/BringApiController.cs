using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.IncomingWebHooks;
using MyAndromeda.Services.Bringg.Outgoing;
using MyAndromeda.Services.WebHooks;
using MyAndromeda.Services.WebHooks.Models;
using MyAndromeda.WebApiClient;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace MyAndromeda.Web.Controllers.Api.Bringg
{
    public class BringApiController : ApiController
    {
        private class StoreDetails 
        {
            public string ExternalSiteId { get; set; }
            public int AndromedaSiteId { get; set; }
        }

        private readonly IWebApiClientContext webApiClientContext;
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderHeaderDataService orderHeaderService;
        private readonly IStoreDataService storeDataService;
        private readonly WebhookEndpointManger webhookEndpointManger;

        public BringApiController(IWebApiClientContext webApiClientContext, IMyAndromedaLogger logger, IOrderHeaderDataService orderHeaderService, IStoreDataService storeDataService, WebhookEndpointManger webhookEndpointManger) 
        {
            this.webhookEndpointManger = webhookEndpointManger;
            this.storeDataService = storeDataService;
            this.orderHeaderService = orderHeaderService;
            this.logger = logger;
            this.webApiClientContext = webApiClientContext;
        }

        private async Task<OrderHeader> GetOrderIdsAsync(BringWebhook model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var message = "The Bringg task id is missing!";
                    this.logger.Error(message);

                }

                var order = await this.orderHeaderService.OrderHeaders
                    .Where(e => e.BringgTaskId == model.Id)
                    .ToArrayAsync();
                //.SingleOrDefaultAsync();

                if (order.Length == 0)
                {
                    this.logger.Error("There is no order with the bringg id : " + model.Id);
                    throw new NullReferenceException("order");
                }
                else if (order.Length > 1)
                {
                    this.logger.Error("There are too many orders with the Bringg id: {0}", model.Id);
                }

                return order.Single();
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
            }
            return null;
        }

        private async Task<StoreDetails> GetStoreIdsAsync(OrderHeader model) 
        {
            try
            {
                var query = this.storeDataService.Table
                    .Where(e => e.ExternalId == model.ExternalSiteID && e.ACSApplicationSites.Any(s => s.ACSApplicationId == model.ApplicationID))
                    .Select(e => new { e.ExternalId, e.AndromedaSiteId });//.ToArrayAsync();

                var queryResult = await query.ToArrayAsync();

                if (queryResult.Length > 1)
                {
                    this.logger.Error("There were more than one store returned by ACSApplicationId: {0} and ExternalSiteID: {1}", model.ApplicationID, model.ExternalSiteID);
                }
                if (queryResult.Length == 0)
                {
                    this.logger.Error("There were more than one store returned by ACSApplicationId: {0} and ExternalSiteID: {1}", model.ApplicationID, model.ExternalSiteID);
                }

                var store = queryResult
                                .Select(e => new StoreDetails
                                {
                                    ExternalSiteId = e.ExternalId,
                                    AndromedaSiteId = e.AndromedaSiteId
                                }
                                )
                                .SingleOrDefault();

                return store;
            }
            catch (Exception ex)
            {
                this.logger.Error(ex);
            }

            return null;
        }

        private async Task<OrderStatusChange> GetOrderStatusModel(BringWebhook model, UsefulOrderStatus orderStatus) 
        {
            var order = await this.GetOrderIdsAsync(model);
            var store = await this.GetStoreIdsAsync(order);

            var sendModel = new OrderStatusChange()
            {
                AndromedaSiteId = store.AndromedaSiteId,
                ExternalSiteId = store.ExternalSiteId,
                ExternalOrderId = order.ExternalOrderRef,
                //InternalOrderId = order.Id,
                Source = "Bringg -> ACS",
                AcsApplicationId = order.ApplicationID,
                Status = (int)orderStatus,
                StatusDescription = orderStatus.Describe(),
            };

            return sendModel;
        }

        private async Task<BringOutgoingWebHook> GetOrderHeader(BringWebhook model) 
        {
            logger.Debug("Creating new outward notification based on the Bringg webhook.");

            var order = await this.GetOrderIdsAsync(model);
            var store = await this.GetStoreIdsAsync(order);

            if (order == null) 
            {
                var message = "Order could not be matched. Bringg will blow up."; 
                logger.Error(message);
                throw new Exception(message);
            }

            if(store == null)
            {
                var message = "The store could not be matched. Bringg will blow up.";
                logger.Error(message);
                throw new Exception(message);
            }

            BringOutgoingWebHook sendModel = new BringOutgoingWebHook()
            {
                AndromedaSiteId = store.AndromedaSiteId,
                ExternalSiteId = store.ExternalSiteId,
                AndromedaOrderId = order.ID.ToString(),
                ExternalId = order.ExternalOrderRef,
                Id = model.Id,
                Source = "Bringg -> ACS",
                Status = model.Status,
                AndromedaOrderStatusId = order.Status,
                UserId = model.UserId
            };

            return sendModel;
        }

        [HttpPost]
        [Route("bringg/notifyTaskCreatedUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskCreated([FromUri]int taskId, [FromBody]BringWebhook model) 
        {
            this.logger.Debug("notifyTaskCreatedUrl: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);
                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): Task created error");
                this.logger.Error(ex);
            }
            
            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskAssignedUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskAssigned([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - notifyTaskAssignedUrl: " + taskId);

            try
            {
                var sendModel = await this.GetOrderHeader(model);
                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskAssigned error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskOnTheWayUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskOnTheWay([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - notifyTaskOnTheWayUrl: " + taskId);

            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
                await this.UpdateOrderSatatus(model, UsefulOrderStatus.OrderIsOutForDelivery);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskOnTheWay error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskCheckedInUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskCheckedIn([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - notifyTaskCheckedInUrl: " + taskId);

            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskCheckedIn error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyArrivedOnLocationUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskArrived([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - NotifyTaskArrived: " + taskId);

            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskArrived error");
                this.logger.Error(ex);
            }
            
            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskDoneUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskDone([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - NotifyTaskDone: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
                await this.UpdateOrderSatatus(model, UsefulOrderStatus.OrderHasBeenCompleted);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskDone error");
                this.logger.Error(ex);
            }

            

            return Ok();
        }

        [HttpPost] 
        [Route("bringg/notifyTaskAcceptedUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskAccepted([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - NotifyTaskAccepted: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskAccepted error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskCancelledUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskCancelled([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - NotifyTaskCancelled: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskCancelled error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyTaskRejectedUrl/{taskId}")]
        public async Task<OkResult> NotifyTaskRejected([FromUri]int taskId, [FromBody]BringWebhook model)
        {
            this.logger.Debug("bringg - NotifyTaskRejected: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyTaskRejected error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        [HttpPost]
        [Route("bringg/notifyLateUrl/{taskId}")]
        public async Task<OkResult> NotifyLate([FromUri]string taskId, [FromBody]BringWebhook model) 
        {
            this.logger.Debug("bringg - NotifyLate: " + taskId);
            try
            {
                var sendModel = await this.GetOrderHeader(model);

                await this.CreateBringgWebhookRequest(sendModel);
            }
            catch (Exception ex)
            {
                this.logger.Error("Bringg (from webhook): NotifyLate error");
                this.logger.Error(ex);
            }

            return Ok();
        }

        //[Route("bringg/notifyETAChangedUrl")]
        //public async Task<OkResult> NotifyEtaChanged([FromUri]string taskId, [FromBody]BringgWebhookWayPointUpdate model)
        //{
        //    var sendModel = await this.GetEtaModel(model);

        //    //await this.CreateRequest(sendModel);

        //    return Ok();
        //}

        private async Task<bool> UpdateOrderSatatus(BringWebhook model, UsefulOrderStatus orderStatus) 
        {
            bool result = false;

            var sendModel = this.GetOrderStatusModel(model, orderStatus);

            try
            {
                this.logger.Debug("Calling: " + webApiClientContext.BaseAddress + this.webhookEndpointManger.OrderStatus);
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(webApiClientContext.BaseAddress);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await
                        client.PostAsJsonAsync(this.webhookEndpointManger.OrderStatus, sendModel);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format("Problem calling: {0}", this.webhookEndpointManger.OrderStatus);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        throw new WebException(message, new Exception(responseMessage));
                    }

                    result = true;
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return result;
        }

        private async Task<bool> CreateBringgWebhookRequest(BringOutgoingWebHook bringWebook)
        {
            bool result = false;

            try
            {
                this.logger.Debug("Calling: " + webApiClientContext.BaseAddress + this.webhookEndpointManger.BringgEndpoint);
                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(webApiClientContext.BaseAddress);
                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsJsonAsync(this.webhookEndpointManger.BringgEndpoint, bringWebook);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format("Could not call : {0}", this.webhookEndpointManger.BringgEndpoint);
                        string responseMessage = await response.Content.ReadAsStringAsync();

                        throw new WebException(message, new Exception(responseMessage));
                    }

                    result = true; 
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return result;
        }

    }
}