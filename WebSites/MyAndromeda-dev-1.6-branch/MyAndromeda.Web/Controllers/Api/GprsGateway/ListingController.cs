using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Logging;
using System.Net.Http;
using System.Net;
using System.Text;
using MyAndromeda.Services.GprsGateway;
using MyAndromeda.Data.DataAccess.Sites;

namespace MyAndromeda.Web.Controllers.Api.GprsGateway
{
    public class ListingController : ApiController
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INewOrderService newOrderService;
        private readonly IGenerateTicketService generateTicketService;
        private readonly IStoreDataService storeDataService;

        public static string TestOrder = string.Empty;

        public ListingController(IMyAndromedaLogger logger,
            INewOrderService newOrderService,
            IGenerateTicketService generateTicketService,
            IStoreDataService storeDataService)
        { 
            this.storeDataService = storeDataService;
            this.newOrderService = newOrderService;
            this.generateTicketService = generateTicketService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("api/GprsGateway/reprint/{Id}")]
        public async Task<HttpResponseMessage> Print([FromUri]string id) 
        {
            Guid g = default(Guid);
            
            if (!Guid.TryParse(id, out g)) 
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
            var order = await this.newOrderService.Query().Where(e => e.ID == g).SingleOrDefaultAsync();
            var externalSiteId = order.ExternalSiteID;
            var store = await this.storeDataService.List().Where(e => e.ExternalId == externalSiteId).SingleOrDefaultAsync();

            var printMe = await this.generateTicketService.Generate(store, order);
            TestOrder = printMe;

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            return response;
        }

        [HttpGet]
        [Route("Api/GprsGateway/Orders.asp")]
        public async Task<HttpResponseMessage> LookingForOrders(string a, string u, string p) 
        {
            //a=AC001&u=testuser&p=test
            this.logger.Debug("Orders check: a (id):{0}, username:{1}, password:{2}", a,u,p);

            var id = Convert.ToInt32(a);


            var store = await this.storeDataService
                .List()
                .Include(e=> e.Address)
                .Where(e=> e.AndromedaSiteId == id).SingleOrDefaultAsync();

            var readyOrdersQuery = await this.newOrderService.ListActiveOrdersByStoreAsync(store);
            var readyOrders = await readyOrdersQuery.ToArrayAsync(); //.ToArrayAsync();

            var headers = this.Request.Headers;

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            
            response.Content = new StringContent(TestOrder, Encoding.Unicode, "text/plain");
            
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