using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.IO;
using System.Text;
using System.Configuration;

namespace DemoTrackingService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class DemoTrackingService
    {
        [WebGet(UriTemplate = "TrackOrder?customerCredentials={customerCredentials}")]
        [OperationContract]
        public Stream TrackOrder(string customerCredentials)
        {
            Stream responseStream = null;
            StringBuilder json = new StringBuilder();

            // Tell the caller that we're returning xml
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            try
            {
                // Build the url
                string url = ConfigurationManager.AppSettings["AndromedaOrderTrackingUrl"] + "/Order" +
                    "?clientKey=" + ConfigurationManager.AppSettings["ClientKey"] +
                    "&customerCredentials=" + customerCredentials;

                // Do an HTTP GET
                string response = HttpHelper.HttpGet(url);

                // Extract the orders from the xml returned by Andromeda
                List<Order> orders = XmlHelper.ExtractOrdersFromXml(response);

                // Build the json to return to the browser
                json.Append("{\"orders\": [");

                foreach (Order order in orders)
                {
                    if (json.Length > 12)
                    {
                        json.Append(",");
                    }

                    json.Append("{");
                    json.Append("\"Status\":\"");
                    json.Append(order.Status.ToString());
                    json.Append("\", ");
                    json.Append("\"StoreLat\":\"");
                    json.Append(order.StoreLatitude.ToString());
                    json.Append("\", ");
                    json.Append("\"StoreLon\":\"");
                    json.Append(order.StoreLongitude.ToString());
                    json.Append("\", ");
                    json.Append("\"CustLat\":\"");
                    json.Append(order.CustomerLatitude.ToString());
                    json.Append("\", ");
                    json.Append("\"CustLon\":\"");
                    json.Append(order.CustomerLongitude.ToString());
                    json.Append("\", ");
                    json.Append("\"PersonProcessing\":\"");
                    json.Append(order.PersonProcessing);
                    json.Append("\", ");
                    json.Append("\"Lat\":\"");
                    json.Append(order.TrackerLatitude.ToString());
                    json.Append("\", ");
                    json.Append("\"Lon\":\"");
                    json.Append(order.TrackerLongitude.ToString());
                    json.Append("\"");
                    json.Append("}");
                }

                json.Append("]}");

                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                // Return an error
                json.Append("{\"error\": { \"Message\":\"");
                json.Append(exception.Message);
                json.Append("\" } }");

                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            try
            {
                // Stream the xml to the caller
                byte[] returnBytes = Encoding.UTF8.GetBytes(json.ToString());
                responseStream = new MemoryStream(returnBytes);
            }
            catch { }

            return responseStream;
        }

        [WebGet(UriTemplate = "OrderLocation?customerCredentials={customerCredentials}")]
        [OperationContract]
        public Stream OrderLocation(string customerCredentials)
        {
            Stream responseStream = null;

            // Build the json to return to the browser
            StringBuilder json = new StringBuilder();

            // Tell the caller that we're returning xml
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";

            try
            {
                // Build the url
                string url = ConfigurationManager.AppSettings["AndromedaOrderTrackingUrl"] + "/OrderLocation" +
                    "?clientKey=" + ConfigurationManager.AppSettings["ClientKey"] +
                    "&customerCredentials=" + customerCredentials;

                // Do an HTTP GET
                string response = HttpHelper.HttpGet(url);

                // Extract the orders from the xml returned by Andromeda
                List<Order> orders = XmlHelper.ExtractOrdersFromXml(response);

                json.Append("{\"orders\": [");

                foreach (Order order in orders)
                {
                    if (json.Length > 12)
                    {
                        json.Append(",");
                    }

                    json.Append("{");
                    json.Append("\"Status\":\"");
                    json.Append(order.Status.ToString());
                    json.Append("\", ");
                    json.Append("\"PersonProcessing\":\"");
                    json.Append(order.PersonProcessing);
                    json.Append("\", ");
                    json.Append("\"Lat\":\"");
                    json.Append(order.TrackerLatitude.ToString());
                    json.Append("\", ");
                    json.Append("\"Lon\":\"");
                    json.Append(order.TrackerLongitude.ToString());
                    json.Append("\"");
                    json.Append("}");
                }

                json.Append("]}");

                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                // Return an error
                json.Append("{\"error\": { \"Message\":\"");
                json.Append(exception.Message);
                json.Append("\" } }");

                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            try
            {
                // Stream the xml to the caller
                byte[] returnBytes = Encoding.UTF8.GetBytes(json.ToString());
                responseStream = new MemoryStream(returnBytes);
            }
            catch { }

            return responseStream;
        }
    }
}