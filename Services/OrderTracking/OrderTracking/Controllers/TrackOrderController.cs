using OrderTracking.Models;
using OrderTracking.Services;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OrderTracking.Controllers
{
    public class TrackOrderController : ApiController
    {
        [HttpGet]
        [ActionName("Session")]
        public async Task<HttpResponseMessage> Session(string applicationId, string customerCredentials)
        {
            Response response = await WebOrderTrackingServices.StartSession(applicationId, customerCredentials);

            HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(response.HttpStatusCode);
            httpResponseMessage.Content = new StringContent(response.ResponseJSON, Encoding.UTF8, "application/json");
            return httpResponseMessage;
        }

        [HttpGet]
        [ActionName("TrackOrder")]
        public async Task<HttpResponseMessage> TrackOrder(string sessionId)
        {
            Response response = await WebOrderTrackingServices.TrackOrder(sessionId);

            HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(response.HttpStatusCode);
            httpResponseMessage.Content = new StringContent(response.ResponseJSON, Encoding.UTF8, "application/json");
            return httpResponseMessage;
        }

        [HttpGet]
        [ActionName("TrackOrderLocation")]
        public async Task<HttpResponseMessage> TrackOrderLocation(string sessionId)
        {
            Response response = await WebOrderTrackingServices.TrackOrderLocation(sessionId);

            HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(response.HttpStatusCode);
            httpResponseMessage.Content = new StringContent(response.ResponseJSON, Encoding.UTF8, "application/json");
            return httpResponseMessage;
        }

        //public Stream TrackOrder(string clientKey, string customerCredentials)
        //{
        //    Stream responseStream = null;

        //    // Do the work
        //    string response = WebOrderTrackingMethods.TrackOrder(clientKey, customerCredentials);

        //    // Tell the caller that we're returning json
        //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

        //    // All done
        //    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

        //    try
        //    {
        //        // Stream the json to the caller
        //        byte[] returnBytes = Encoding.UTF8.GetBytes(response);
        //        responseStream = new MemoryStream(returnBytes);
        //    }
        //    catch { }

        //    return responseStream;
        //}

        //public Stream TrackOrderLocation(string clientKey, string customerCredentials)
        //{
        //    Stream responseStream = null;

        //    // Do the work
        //    string response = WebOrderTrackingMethods.TrackOrderLocation(clientKey, customerCredentials);

        //    // Tell the caller that we're returning json
        //    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

        //    // All done
        //    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

        //    try
        //    {
        //        // Stream the json to the caller
        //        byte[] returnBytes = Encoding.UTF8.GetBytes(response);
        //        responseStream = new MemoryStream(returnBytes);
        //    }
        //    catch { }

        //    return responseStream;
        //}
    }
}
