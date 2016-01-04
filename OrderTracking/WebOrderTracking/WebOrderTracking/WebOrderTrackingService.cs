using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace Andromeda.WebOrderTracking
{
    [ServiceContract]
    public class WebOrderTrackingService
    {
        [WebGet(UriTemplate = "Test")]
        [OperationContract]
        public Stream Test()
        {
            Stream responseStream = null;

            try
            {
                // Tell the caller that we're returning xml
                WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

                // Stream the xml to the caller
                byte[] returnBytes = Encoding.UTF8.GetBytes("<test>Testing 1..2..3</test>");
                responseStream = new MemoryStream(returnBytes);
            }
            catch (Exception exception)
            {
                // Return an error
                try
                {
                    byte[] returnBytes = Encoding.UTF8.GetBytes("Error!!");
                    responseStream = new MemoryStream(returnBytes);

                    //responseStream = ResponseHelper.GetResponseError<SubscriptionResponse>((IResponse)new SubscriptionResponse()); 
                }
                catch { }
            }

            return responseStream;
        }

        [WebGet(UriTemplate = "TrackOrder?clientKey={clientKey}&customerCredentials={customerCredentials}")]
        [OperationContract]
        public Stream TrackOrder(string clientKey, string customerCredentials)
        {
            Stream responseStream = null;

            // Do the work
            string response = WebOrderTrackingMethods.TrackOrder(clientKey, customerCredentials);

            // Tell the caller that we're returning json
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            // All done
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

            try
            {
                // Stream the json to the caller
                byte[] returnBytes = Encoding.UTF8.GetBytes(response);
                responseStream = new MemoryStream(returnBytes);
            }
            catch { }

            return responseStream;
        }

        [WebGet(UriTemplate = "TrackOrderLocation?clientKey={clientKey}&customerCredentials={customerCredentials}")]
        [OperationContract]
        public Stream TrackOrderLocation(string clientKey, string customerCredentials)
        {
            Stream responseStream = null;

            // Do the work
            string response = WebOrderTrackingMethods.TrackOrderLocation(clientKey, customerCredentials);

            // Tell the caller that we're returning json
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            // All done
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

            try
            {
                // Stream the json to the caller
                byte[] returnBytes = Encoding.UTF8.GetBytes(response);
                responseStream = new MemoryStream(returnBytes);
            }
            catch { }

            return responseStream;
        }
    }
}
