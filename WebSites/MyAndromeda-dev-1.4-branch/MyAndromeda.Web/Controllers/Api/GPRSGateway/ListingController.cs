using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MyAndromeda.Logging;
using System.Net.Http;
using System.Net;
using System.Text;
using MyAndromeda.Services.GprsGateway;

namespace MyAndromeda.Web.Controllers.Api.GprsGateway
{
    public class ListingController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INewOrderService newOrderService;

        public static string TestOrder = string.Empty;

        public ListingController(IMyAndromedaLogger logger, INewOrderService newOrderService)
        { 
            this.newOrderService = newOrderService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("Api/GprsGateway/Orders.asp")]
        public async Task<HttpResponseMessage> LookingForOrders(string a, string u, string p) 
        {
            //a=AC001&u=testuser&p=test
            this.logger.Debug("Orders check: a (id):{0}, username:{1}, password:{2}", a,u,p);

            var id = Convert.ToInt32(a);
            var readyOrdersQuery = await this.newOrderService.ListActiveOrdersByAndromedaId(id);
            var readyOrders = readyOrdersQuery.ToArrayAsync(); //.ToArrayAsync();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            
            response.Content = new StringContent(TestOrder, Encoding.ASCII, "text/plain");
            




            //return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "", "text/plain"); 
            TestOrder = string.Empty;

            return response;
        }

        [HttpGet]
        [HttpPost]
        [Route("Api/GprsGateway/Callback.asp")]
        public HttpResponseMessage Callback(string a, string o, string ak, string m, string dt, string u, string p) 
        {
            this.logger.Debug("Callback check");
            this.logger.Debug("a: {0}, o: {1}, ak:{2}, m: {3}, dt: {4}, username: {5}, password: {6}", a, o, ak, m, dt, u, p);

            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, "", "text/plain"); 
        }
    }
}