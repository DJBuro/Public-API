using System.Collections.Generic;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Helpers;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.Outgoing;
using MyAndromeda.Services.WebHooks.Events;
using MyAndromeda.Services.WebHooks.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Services.WebHooks.Models.Settings;
using MyAndromedaDataAccessEntityFramework.DataAccess;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api.WebHooks
{
    public class WebHookNotificationApiController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IAcsApplicationDataService dataService;
        private readonly IWebHooksEvent[] events;
        private readonly ICustomerOrdersDataService orderHeaderDataService;
        private readonly IStoreDataService storeDataService;

        public WebHookNotificationApiController(
            IAcsApplicationDataService dataService, 
            IMyAndromedaLogger logger, 
            IWebHooksEvent[] events, 
            ICustomerOrdersDataService orderHeaderDataService, 
            IStoreDataService storeDataService)
        {
            this.storeDataService = storeDataService;
            this.orderHeaderDataService = orderHeaderDataService;
            this.events = events;
            this.logger = logger;
            this.dataService = dataService;
            this.Flavor = WebHookType.Unset;
        }

        /// <summary>
        /// Gets or sets the flavor.
        /// </summary>
        /// <value>The flavor.</value>
        public WebHookType Flavor { get; set; }

        private async Task SendingActionHandlers<TModel>(TModel model, WebHookEnrolement enrollment) 
            where TModel : IHook
        {
            await this.events.ForEachAsync(async ev => {
                try
                {
                    await ev.SendingRequestAsync(model.AndromedaSiteId, enrollment, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Event Failed: " + ev.Name);
                    this.logger.Error(ex);
                }
            });
        }

        private async Task SentActionHandlers<TModel>(TModel model, WebHookEnrolement enrollment) 
            where TModel : IHook
        {
            await this.events.ForEachAsync(async ev => {
                try
                {
                    await ev.SentRequestAsync(model.AndromedaSiteId, enrollment, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Event Failed: " + ev.Name);
                    this.logger.Error(ex);
                }
            });
        }

        private async Task OnError<TModel>(TModel model, WebHookEnrolement r, Exception ex)
             where TModel : IHook
        {
            this.logger.Error("{0} - Failed: {0} - {1}", model.AndromedaSiteId, r.Name, r.CallBackUrl);
            this.logger.Error(ex);

            await this.events.ForEachAsync(async ev => {
                try
                {
                    await ev.FailedRequestAsync(model.AndromedaSiteId, r, model);
                }
                catch (Exception)
                {
                    this.logger.Error("Error Failed :) -" + ev.Name);
                    this.logger.Error(ex);
                }
            });
            
        }

        private async Task FillUpBasicMissingIds<TModel>(TModel model) 
            where TModel : IHook 
        {
            try
            {
                bool needExternalId = string.IsNullOrWhiteSpace(model.ExternalSiteId);
                bool needAndromedaSiteId = model.AndromedaSiteId == 0;

                if (!needAndromedaSiteId && !needExternalId)
                {
                    this.logger.Debug("All ids are here!" + model.ExternalSiteId);
                    //all good;
                    return;
                }

                if (needExternalId)
                {
                    var store = await this.storeDataService.List().Where(e => e.AndromedaSiteId == model.AndromedaSiteId)
                        .Select(e => new
                        {
                            e.ExternalId,
                            e.Name
                        }).FirstAsync();

                    model.ExternalSiteId = store.ExternalId;

                    this.logger.Debug("ADDED THE EXTERNAL SITE ID:" + model.ExternalSiteId);
                }
                if (needAndromedaSiteId)
                {
                    var store = await this.storeDataService.List().Where(e => e.AndromedaSiteId == model.AndromedaSiteId)
                        .Select(e => new
                        {
                            e.AndromedaSiteId,
                            e.Name
                        }).FirstAsync();

                    model.AndromedaSiteId = store.AndromedaSiteId;

                    this.logger.Debug("ADDED THE ANDROMEDA SITE ID:" + model.AndromedaSiteId);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Problem fixing ids...");
                this.logger.Error(ex.Message);
                throw;
            }

            
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-status")]
        public async Task<HttpResponseMessage> OnlineState(StoreOnlineStatus model)
        {
            this.Flavor = WebHookType.StoreStaus;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {
            }

            await this.CallEndpoints(model, e => e.StoreOnlineStatus);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
 
        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-estimated-delivery-time")]
        public async Task<HttpResponseMessage> UpdateDeliveryTime(UpdateDeliveryTime model)
        {
            this.Flavor = WebHookType.OrderEta;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {
            }

            await this.CallEndpoints(model, e => e.Edt);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-menu")]
        public async Task<HttpResponseMessage> MenuChange(MenuChange model)
        {
            this.Flavor = WebHookType.MenuChange;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {
            }

            await this.CallEndpoints(model, e => e.MenuVersion);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-menu-items")]
        public async Task<HttpResponseMessage> MenuItemsChanged(MenuItemsChanged model) 
        {
            this.Flavor = WebHookType.MenuItemChange | WebHookType.MenuChange;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {
            }

            await this.CallEndpoints(model, e => e.MenuItems);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/orders/update-order-status")]
        public async Task<HttpResponseMessage> OrderStatusChange(OrderStatusChange model)
        {
            this.Flavor = WebHookType.OrderStatus;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e) { }

            if (string.IsNullOrWhiteSpace(model.ExternalOrderId) && !model.RamesesOrderNum.HasValue) 
            {
                var message = "External Order Id OR RamesesOrderNum is required: " + model.Source;
                this.logger.Error(message);

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
            }

            var query = this.orderHeaderDataService.List();

            if (!string.IsNullOrWhiteSpace(model.ExternalOrderId))
            {
                query = query
                    .Where(e => e.ExternalOrderRef == model.ExternalOrderId);
            }

            if (model.RamesesOrderNum.HasValue)
            {
                query = query
                    .Where(e => e.RamesesOrderNum == model.RamesesOrderNum);
            }

            query = query
                .Where(e => e.ExternalSiteID == model.ExternalSiteId)
                .OrderByDescending(e=> e.TimeStamp);

            var orderHeaders = query
                .Select(e=> new { 
                    e.ID, 
                    e.ApplicationID,
                    e.ExternalOrderRef,
                    e.RamesesOrderNum,
                }).ToArray();

            if (orderHeaders.Length > 1) 
            {
                var obj = JsonConvert.SerializeObject(model);
                var message = "The order brought back too many records!!!!! " + obj;

                this.logger.Error(message);
                this.logger.Error(query.ToTraceQuery());

            }
            if (orderHeaders.Length == 0)
            {
                var obj = JsonConvert.SerializeObject(model); 
                var message = "No order could be found using external Order id: " + obj;
                this.logger.Error(message);

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
            }

            var orderHeader = orderHeaders.FirstOrDefault(); 

            model.InternalOrderId = orderHeader.ID;
            model.AcsApplicationId = orderHeader.ApplicationID;
            model.ExternalOrderId = orderHeader.ExternalOrderRef;
            model.RamesesOrderNum = orderHeader.RamesesOrderNum;
            

            await this.CallEndpoints(model, e=> e.OrderStatus);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/bringg/update")]
        public async Task<HttpResponseMessage> BringMessage([FromBody]OutgoingWebHookBring model) 
        {
            this.Flavor = WebHookType.BringgUpdate;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {

            }

            await this.CallEndpoints(model, e=> e.BringUpdates);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/bringg/update-eta")]
        public async Task<HttpResponseMessage> BringgEtaMessage(BringOutgoingEtaWebHook model) 
        {
            this.Flavor = WebHookType.BringgEtaUpdate;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception e)
            {
            }

            await this.CallEndpoints(model, e => e.BringEtaUpdates);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private async Task CallEndpoints<TModel>(
            TModel model, 
            Func<WebhookSettings, List<WebHookEnrolement>> fetchEnrollments)
            where TModel : IHook
        {
            var acsApplications = await dataService.Query()
                .Where(e => e.ACSApplicationSites.Any(acsApplication => acsApplication.Store.AndromedaSiteId == model.AndromedaSiteId))
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.WebHookSettings
                }).ToListAsync();

            try
            {
                if (acsApplications.All(e => string.IsNullOrEmpty(e.WebHookSettings)))
                {
                    await this.events.ForEachAsync(async ev =>
                    {
                        await ev.NoWebHooksAsync(model.AndromedaSiteId, model);
                    });
                    //return as there is nothing left to do. 
                    //return;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed processing NoWebHooksAsync");
                this.logger.Error(ex);
                throw;
            }

            try
            {
                //notify events before sending this externally
                await this.events.ForEachAsync(async ev =>
                {
                    try
                    {
                        await ev.BeforeDistributionAsync(model.AndromedaSiteId, model);
                    }
                    catch (Exception)
                    {
                        this.logger.Error(ev.Name + " exception");
                        //throw;
                    }
                });
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed processing BeforeDistributionAsync");
                this.logger.Error(ex);
                throw;
            }


            try
            {
                //notify each subscription that we are sending. 
                await acsApplications
                    .Where(e => !string.IsNullOrWhiteSpace(e.WebHookSettings))
                    .ForEachAsync(async e =>
                    {
                        if (string.IsNullOrWhiteSpace(e.WebHookSettings)) { return; }

                        var o = JsonConvert.DeserializeObject<WebhookSettings>(e.WebHookSettings);

                        var webhooks = fetchEnrollments(o) ?? new List<WebHookEnrolement>();

                        await webhooks.Where(r => r.Enabled).ForEachAsync(async enrollment =>
                        {
                            await this.SendingActionHandlers(model, enrollment);

                            await this.CreateRequest(model, enrollment, OnError);

                            await this.SentActionHandlers(model, enrollment);
                        });
                    });
            }
            catch (Exception ex) 
            {
                this.logger.Error("Failed processing each request");
                this.logger.Error(ex);
                throw;
            }

 
            //notify events after sending this externally
            await this.events.ForEachAsync(async ev =>
            {
                try
                {
                    await ev.AfterDistributionAsync(model.AndromedaSiteId, model);
                }
                catch (Exception ex)
                {
                    this.logger.Error("Failed processing: {0} - AfterDistributionAsync", ev.Name);
                    this.logger.Error(ex);
                }
            });
            
            
        }

        private async Task<WebHookEnrolement> CreateRequest<TModel>(TModel payload, WebHookEnrolement enrollment, Func<TModel, WebHookEnrolement, Exception, Task> onError)
            where TModel : IHook
        {
            Exception ex = null; 
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(enrollment.CallBackUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (enrollment.RequestHeaders != null)
                    {
                        //has not been done yet unfortunately 
                        foreach (var header in enrollment.RequestHeaders)
                        {
                            if (string.IsNullOrWhiteSpace(header.Key)) { continue; }
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    HttpResponseMessage response = await client.PostAsJsonAsync<TModel>(enrollment.CallBackUrl, payload);

                    if (!response.IsSuccessStatusCode)
                    {
                        string message = string.Format("{0} - Could not call : {1}", enrollment.Name, enrollment.CallBackUrl);
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        throw new WebException(message, new Exception(responseMessage));
                    }
                }
            }
            catch (Exception e)
            {
                ex = e;
            }

            if (ex != null) { await onError(payload, enrollment, ex); }

            return enrollment;
        }
    }
}