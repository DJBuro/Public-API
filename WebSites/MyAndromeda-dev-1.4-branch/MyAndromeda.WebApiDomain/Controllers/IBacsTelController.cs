using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Logging;
using MyAndromeda.WebApiServices.Services;

namespace MyAndromeda.WebApiServices.Controllers
{
    public class IBacsTelController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IOrderService orderService; 
        private readonly ICurrentSite site;
        
        public IBacsTelController(IMyAndromedaLogger logger, ICurrentSite site, IOrderService orderService) 
        {
            this.orderService = orderService;
            this.logger = logger;
            this.site = site;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Models.GprsPrinterIbacs model, string andromedaSiteId, string orderId)
        {
            if (string.IsNullOrWhiteSpace(andromedaSiteId) || string.IsNullOrWhiteSpace(orderId)) 
            {
                this.logger.Error("andromedaSiteId is needed");
                this.logger.Error("orderId is needed");

                return new HttpResponseMessage(HttpStatusCode.BadRequest) { };
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

                this.orderService.UpdateOrderStatus(ramesesOrderId, model.Status.Trim().Equals("1"), model.Msg);
                var email = this.orderService.CreateEmail(ramesesOrderId, model);

                //var siteOrders = this.customerOrdersDataService.List().Where(e => e.ExternalSiteID.Equals(site.ExternalSiteId, StringComparison.InvariantCultureIgnoreCase)).ToArray();


                this.logger.Debug("an email may be sending");
                await email.SendAsync();
                this.logger.Debug("an email is sent!");

            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
