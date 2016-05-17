using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Framework.Logging;
using MyAndromeda.Logging;
using MyAndromeda.Services.Bringg.Outgoing;
using MyAndromeda.Services.WebHooks.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using MyAndromeda.Services.WebHooks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api.WebHooks
{
    public class WebHookNotificationApiController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ISendWebHooksService sendWebHooksService;

        private readonly ICustomerOrdersDataService orderHeaderDataService;
        private readonly IStoreDataService storeDataService;

        public WebHookNotificationApiController(
            IMyAndromedaLogger logger,
            ICustomerOrdersDataService orderHeaderDataService,
            IStoreDataService storeDataService,
            ISendWebHooksService sendWebHooksService)
        {
            this.sendWebHooksService = sendWebHooksService;
            this.storeDataService = storeDataService;
            this.orderHeaderDataService = orderHeaderDataService;
            this.logger = logger;
            //this.Flavor = WebHookType.Unset;
        }

        /// <summary>
        /// Gets or sets the flavor.
        /// </summary>
        /// <value>The flavor.</value>
        //public WebHookType Flavor { get; set; }


        private async Task FillUpBasicMissingIds<TModel>(TModel model) 
            where TModel : IHook 
        {
            this.logger.Debug("Fill in the incoming message. It is a medley of differences between external id, andromeda site id");

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
                    var store = await this.storeDataService.List()
                        .Where(e => e.AndromedaSiteId == model.AndromedaSiteId)
                        .Select(e => new
                        {
                            e.AndromedaSiteId,
                            e.ExternalId,
                            e.Name
                        }).FirstAsync();

                    if (store != null)
                    {
                        this.logger.Debug("ADDED THE EXTERNAL SITE ID:" + model.ExternalSiteId);

                        model.AndromedaSiteId = store.AndromedaSiteId;
                        model.ExternalSiteId = store.ExternalId;
                    }
                    else 
                    {
                        this.logger.Error("Bringg webhooks - cant find andromeda siteId:" + model.ExternalSiteId);
                    }

                }

                if (needAndromedaSiteId)
                {
                    var store = await this.storeDataService.List()
                        .Where(e => e.ExternalId == model.ExternalSiteId)
                        .Select(e => new
                        {
                            e.AndromedaSiteId,
                            e.ExternalId,
                            e.Name
                        }).FirstAsync();

                    if (store != null) 
                    {
                        this.logger.Debug("ADD THE ANDROMEDA SITE ID:" + model.AndromedaSiteId);

                        model.AndromedaSiteId = store.AndromedaSiteId;
                        model.ExternalSiteId = store.ExternalId;
                    }
                    else
                    {
                        this.logger.Error("Bringg webhooks - cant find externalsiteid:" + model.ExternalSiteId);
                    }

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
        public async Task<HttpResponseMessage> OnlineState(OutgoingWebHookStoreOnlineStatus model)
        {
            this.logger.Info("web-hooks/store/update-status hit");
            //this.Flavor = WebHookType.StoreStaus;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {
            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.StoreOnlineStatus);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
 
        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-estimated-delivery-time")]
        public async Task<HttpResponseMessage> UpdateDeliveryTime(OutgoingWebHookUpdateDeliveryTime model)
        {
            this.logger.Info(message: "web-hooks/store/update-estimated-delivery-time hit");
            //this.Flavor = WebHookType.OrderEta;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {
            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.Edt);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-menu")]
        public async Task<HttpResponseMessage> MenuChange(OutgoingWebHookMenuChange model)
        {
            this.logger.Info(message: "web-hooks/store/update-menu hit");
            //this.Flavor = WebHookType.MenuChange;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {
            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.MenuVersion);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/store/update-menu-items")]
        public async Task<HttpResponseMessage> MenuItemsChanged(OutgoingWebHookMenuItemsChanged model) 
        {
            this.logger.Info(message: "web-hooks/store/update-menu-items hit");
            //this.Flavor = WebHookType.MenuItemChange | WebHookType.MenuChange;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {
            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.MenuItems);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
        [HttpPost, HttpPut]
        [Route("web-hooks/store/orders/update-order-status")]
        public async Task<HttpResponseMessage> OrderStatusChange(OutgoingWebHookOrderStatusChange model)
        {
            this.logger.Info(message: "web-hooks/store/orders/update-order-status hit");
            //this.Flavor = WebHookType.OrderStatus;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception) { }

            if (string.IsNullOrWhiteSpace(model.ExternalOrderId) && !model.RamesesOrderNum.HasValue) 
            {
                string message = "External Order Id OR RamesesOrderNum is required: " + model.Source;
                this.logger.Error(message);

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
            }

            IQueryable<OrderHeader> query = this.orderHeaderDataService.List();

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
                string obj = JsonConvert.SerializeObject(model);
                string message = "The order brought back too many records!!!!! " + obj;

                this.logger.Error(message);
                this.logger.Error(query.ToTraceQuery());

            }
            if (orderHeaders.Length == 0)
            {
                string obj = JsonConvert.SerializeObject(model); 
                string message = "No order could be found using external Order id: " + obj;
                this.logger.Error(message);

                return this.Request.CreateResponse(HttpStatusCode.BadRequest, message);
            }

            var orderHeader = orderHeaders.FirstOrDefault(); 

            model.InternalOrderId = orderHeader.ID;
            model.AcsApplicationId = orderHeader.ApplicationID;
            model.ExternalOrderId = orderHeader.ExternalOrderRef;
            model.RamesesOrderNum = orderHeader.RamesesOrderNum;


            await this.sendWebHooksService.CallEndpoints(model, e => e.OrderStatus);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/bringg/update")]
        public async Task<HttpResponseMessage> BringMessage([FromBody]OutgoingWebHookBringg model) 
        {
            this.logger.Info(message: "web-hooks/bringg/update hit");
            //this.Flavor = WebHookType.BringgUpdate;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {

            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.BringUpdates);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost, HttpPut]
        [Route("web-hooks/bringg/update-eta")]
        public async Task<HttpResponseMessage> BringgEtaMessage(BringOutgoingEtaWebHook model) 
        {
            this.logger.Info(message: "web-hooks/bringg/update-eta hit");
            //this.Flavor = WebHookType.BringgEtaUpdate;

            try
            {
                await this.FillUpBasicMissingIds(model);
            }
            catch (Exception)
            {
            }

            await this.sendWebHooksService.CallEndpoints(model, e => e.BringEtaUpdates);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}