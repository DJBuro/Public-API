using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyAndromeda.Services.Orders.OrderMonitoring.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Services;
using System.Threading.Tasks;

namespace MyAndromeda.Web.Controllers.Api
{
    public class OrderMonitoringController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderSupportService orderSupportService;
        private readonly IOrderSuppertEmailService orderSupportEmailService;

        public OrderMonitoringController(IMyAndromedaLogger logger, IOrderSupportService orderSupportService, IOrderSuppertEmailService orderSupportEmailService)
        {
            this.orderSupportEmailService = orderSupportEmailService;
            this.orderSupportService = orderSupportService;
            this.logger = logger;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody]OrderList model)
        {
            if (model == null) 
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "the model is invalid" });
            }

            if (model.OrderIds == null || model.OrderIds.Count == 0) 
            {
                logger.Debug("There are no orders to update");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "There were no orders given to update" });
            }

            try
            {
                this.orderSupportService.UpdateOrders(model.OrderIds);
            
                var email = this.orderSupportEmailService.CreateEmailBasedOnOrderIds(model.OrderIds);

                await email.SendAsync();

            }
            catch (Exception e)
            {
                this.logger.Error(e);
                
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}