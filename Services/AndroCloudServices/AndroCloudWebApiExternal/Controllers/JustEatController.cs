using System.Web.Http.Results;
using AndroCloudWebApiExternal.Extensions;
using AndroCloudWebApiExternal.Models.Andromeda;
using AndroCloudWebApiExternal.Models.JustEat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Http.Description;
using AndroCloudWebApiExternal.Attributres;
using log4net;

namespace AndroCloudWebApiExternal.Controllers
{
    [JustEatAuthorize]
    public class JustEatController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost, HttpPut]
        [Route("api/justeat/order")]
        public async Task<HttpResponseMessage> Post()
        {
            string body = await this.Request.Content.ReadAsStringAsync();
            JustEatOrder order = null;
            try
            {
                order = JsonConvert.DeserializeObject<JustEatOrder>(body);
            }
            catch (Exception e) 
            {
                log.Error(e);
                throw;
            }

            var andromedaOrder = order.Convert();

            var applicationId = Models.JustEatConfiguration.ApplicationId;
            var externalSiteId = order.RestaurantInfo.Id.ToString();
            var orderId = order.Id;

            List<Models.Services.ErrorResponse> errorResponses = new List<Models.Services.ErrorResponse>();

            var success = await Services.OrderService.PostOrder(andromedaOrder, applicationId, externalSiteId, orderId, (server) => {

                var json = JsonConvert.SerializeObject(andromedaOrder);
                log.DebugFormat("Sent to ACS: {0}", server);
                log.Debug(json);

            }, (error) => {
                errorResponses.Add(error);
            });

            if (!success && errorResponses.Count > 0)
            {
                //var json = JsonConvert.SerializeObject(andromedaOrder);
                var firstError = errorResponses.FirstOrDefault();
                
                var justEatErrorResponse = firstError.CreateErrorResponse(order);


                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, justEatErrorResponse);
            }

            if (success) 
            {
                //success
                log.DebugFormat("Success: {0}; Customer: {1}", order.Id, order.CustomerInfo.Name);
            }

            return this.Request.CreateResponse(HttpStatusCode.Created, andromedaOrder);
        }
    }
}
