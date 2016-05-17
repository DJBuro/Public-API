using System.Threading.Tasks;
using System.Web.Http;
using MyAndromeda.Framework.Notification;
using System.Net.Http;
using System.Net;
using System.Text;
using MyAndromeda.Logging;
using MyAndromeda.Data.DataAccess;

namespace MyAndromeda.Web.Areas.Acs.Controllers
{
    public class WebHookSetupApiController : ApiController
    {
        private readonly IAcsApplicationDataService acsApplicationDataService;
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier;

        public WebHookSetupApiController(IAcsApplicationDataService acsApplicationDataService, INotifier notifier, IMyAndromedaLogger logger) 
        {
            this.logger = logger;
            this.notifier = notifier;
            this.acsApplicationDataService = acsApplicationDataService;
        }

        [HttpGet]
        [Route("api/webhook-setup/read/{acsApplicationId}")]
        public async Task<HttpResponseMessage> Read([FromUri] int acsApplicationId)
        {
            var entity = this.acsApplicationDataService.Get(acsApplicationId);
            var rawModel = entity.WebHookSettings ?? "{}";

            //var result = JsonConvert.DeserializeObject(rawModel);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(rawModel, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [Route("api/webhook-setup/update/{acsApplicationId}")]
        public async Task<HttpResponseMessage> Update(int acsApplicationId)
        {
            var entity = this.acsApplicationDataService.Get(acsApplicationId);
            var rawModel = entity.WebHookSettings ?? "{}";
            var rawRequest = await this.Request.Content.ReadAsStringAsync();

            entity.WebHookSettings = rawRequest;
            acsApplicationDataService.Update(entity);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(rawModel, Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPost]
        [Route("api/webhook-catch")]
        public async Task<HttpResponseMessage> CatchAll() 
        {
            var content = await this.Request.Content.ReadAsStringAsync();
            this.notifier.Notify("Caught: " + content);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}