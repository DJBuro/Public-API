using System;
using System.Linq;
using System.Web.Http;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Dates;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.CallbackServices.Models;
using System.Net.Http;
using System.Net;
using MyAndromeda.Services.Orders;

namespace MyAndromeda.Web.Controllers.Api
{
    public class IBacsTelController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IDateServices dateServices;
        private readonly IOrderUpdateService orderUpdateService;
        private readonly IWorkContext workContext;

        public IBacsTelController(
            IMyAndromedaLogger logger,
            IDateServices dateServices,
            IWorkContext workContext, 
            IOrderUpdateService orderUpdateService)
        {
            this.orderUpdateService = orderUpdateService;
            this.workContext = workContext;
            this.logger = logger;
            this.dateServices = dateServices;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] GprsPrinterIbacs model, string andromedaSiteId, string orderId)
        {
            if (string.IsNullOrWhiteSpace(andromedaSiteId) || string.IsNullOrWhiteSpace(orderId))
            {
                this.logger.Error("andromedaSiteId is needed");
                this.logger.Error("orderId is needed");

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                this.logger.Debug("Order received by store: {0}; Db order Id {1}", andromedaSiteId, orderId);

                this.logger.Debug("printer: {0}", model.Printer_id);
                this.logger.Debug("order: {0}", model.Order_id);
                this.logger.Debug("status: {0}", model.Status);
                this.logger.Debug("msg: {0}", model.Msg);
                this.logger.Debug("delivery time: {0}", model.Delivery_time);

                var ramesesOrderId = Convert.ToInt32(orderId);

                bool acceptedOrder = model.Status.Trim().Equals("1");
                
                var currentSite = this.workContext.CurrentSite;

                if (acceptedOrder) 
                {

                    var format = "HH:mm dd-MM-yy";
                    
                    var wantedTimeUtc = this.dateServices.ConvertToUtcFromLocalStringWithCustomFormat(model.Delivery_time, format);
                    var wantedTime = this.dateServices.ConvertToLocalFromUtc(wantedTimeUtc);

                    if (wantedTime.HasValue) 
                    {
                        this.logger.Debug("Wanted time (local): {0}; UTC: {1}", wantedTime.Value.ToString("t"), wantedTimeUtc.Value.ToString("t"));

                        try
                        {
                            this.orderUpdateService.UpdateOrderWantedTime(currentSite.ExternalSiteId, ramesesOrderId, wantedTimeUtc.Value);
                        }
                        catch (Exception e) 
                        {
                            string messsage = string.Format("Could not update the order wanted time (externalSiteId: {0}; andromedaSiteId: {1}; ramesesOrderId: {2}).", 
                                currentSite.ExternalSiteId, 
                                andromedaSiteId, 
                                ramesesOrderId);
                            
                            this.logger.Error(messsage);
                            this.logger.Error(e);
                        }
                    }
                    else
                    {
                        this.logger.Error("The date time could not be passed: {0}", model.Delivery_time);
                        this.logger.Error("The expected date time format is: {0}", format);
                    }
                    
                }
                try
                {
                    //update status here, 
                    this.orderUpdateService.UpdateOrderStatus(currentSite.ExternalSiteId, ramesesOrderId, acceptedOrder, model.Msg);
                }
                catch (Exception e)
                {
                    string message = string.Format("{0} Could not update the status of the order, which means no email", andromedaSiteId);
                    
                    this.logger.Error(message);
                    this.logger.Error(e);
                }
            }
            catch (Exception e)
            {
                this.logger.Error(e);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}