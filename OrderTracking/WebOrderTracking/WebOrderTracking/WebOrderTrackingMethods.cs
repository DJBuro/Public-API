using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Andromeda.WebOrderTracking
{
    public class WebOrderTrackingMethods
    {
        public static string TrackOrder(string clientKey, string customerCredentials)
        {
            int errorCode = 0;
            string response = "";

            // Build the json to return to the browser
            StringBuilder json = new StringBuilder();

            try
            {
                List<Order> orders = null;

                // Get the live orders
                errorCode = WebOrderTrackingManager.Order(clientKey, customerCredentials, out orders);

                if (errorCode == 0)
                {
                    // Build the json to return to the browser
                    json.Append("{\"orders\": [");

                    if (orders != null)
                    {
                        // If we haven't had gps co-ordinates for 20 seconds then assume the tracker has lost its gps fix
                        DateTime oldOrderCutoff = DateTime.Now.AddSeconds(-20);

                        foreach (Order order in orders)
                        {
                            if (json.Length > 12)
                            {
                                json.Append(",");
                            }

                            json.Append("{");
                            json.Append("\"status\":\"");
                            json.Append(order.Status.ToString());
                            json.Append("\", ");
                            json.Append("\"storeLat\":\"");
                            json.Append(order.StoreLatitude.ToString());
                            json.Append("\", ");
                            json.Append("\"storeLon\":\"");
                            json.Append(order.StoreLongitude.ToString());
                            json.Append("\", ");
                            json.Append("\"custLat\":\"");
                            json.Append(order.CustomerLatitude.ToString());
                            json.Append("\", ");
                            json.Append("\"custLon\":\"");
                            json.Append(order.CustomerLongitude.ToString());
                            json.Append("\", ");
                            json.Append("\"personProcessing\":\"");
                            json.Append(order.PersonProcessing);
                            json.Append("\" ");

                            // Get tracker details - the clientKey, customerCredentials have already been verified
                            Tracker tracker = CacheManager.GetTrackerFromCache(clientKey, customerCredentials, order.TrackerName);

                            // Has a tracker been assigned to the driver?
                            if (tracker != null)
                            {
                                // Output any tracker information that we have
                                json.Append(", ");
                                json.Append("\"lat\":\"");
                                if (tracker.Latitude.HasValue)
                                {
                                    json.Append(tracker.Latitude.ToString());
                                }
                                json.Append("\", ");
                                json.Append("\"lon\":\"");
                                if (tracker.Longitude.HasValue)
                                {
                                    json.Append(tracker.Longitude.ToString());
                                }
                                json.Append("\", ");
                                json.Append("\"active\":\"");
                                if (tracker.LastUpdated.Ticks > oldOrderCutoff.Ticks)
                                {
                                    json.Append("true");
                                }
                                else
                                {
                                    json.Append("false");
                                }
                                json.Append("\"");
                            }

                            json.Append("}");
                        }
                    }

                    json.Append("]}");
                }
            }
            catch (Exception exception)
            {
                DataAccess.LogEvent(exception,
                    "clientKey=\"" + (clientKey == null ? "null" : clientKey) +
                    "\" credentials=\"" + (customerCredentials == null ? "null" : customerCredentials) + "\"");

                errorCode = 1; //Internal server error
            }

            if (errorCode != 0)
            {
                // Return an error
                json = new StringBuilder();
                json.Append("{\"error\":{\"errorCode\":\"");
                json.Append(errorCode.ToString());
                json.Append("\"}}");
            }

            response = json.ToString();

            return response;
        }

        public static string TrackOrderLocation(string clientKey, string customerCredentials)
        {
            int errorCode = 0;
            string response = "";

            // Build the json to return to the browser
            StringBuilder json = new StringBuilder();

            try
            {
                List<CachedOrder> cachedOrders = null;

                // Get the live orders
                errorCode = WebOrderTrackingManager.OrderLocation(clientKey, customerCredentials, out cachedOrders);

                if (errorCode == 0)
                {
                    json.Append("{\"orders\": [");

                    if (cachedOrders != null)
                    {
                        // If we haven't had gps co-ordinates for 20 seconds then assume the tracker has lost its gps fix
                        DateTime oldOrderCutoff = DateTime.Now.AddSeconds(-20);

                        foreach (CachedOrder cachedOrder in cachedOrders)
                        {
                            if (json.Length > 12)
                            {
                                json.Append(",");
                            }

                            json.Append("{");
                            json.Append("\"status\":\"");
                            json.Append(cachedOrder.OrderStatusId.ToString());
                            json.Append("\"");

                            // Has a tracker been assigned to the driver?
                            if (cachedOrder.Tracker != null)
                            {
                                // Output any tracker information that we have
                                json.Append(", ");
                                json.Append("\"lat\":\"");
                                if (cachedOrder.Tracker.Latitude.HasValue)
                                {
                                    json.Append(cachedOrder.Tracker.Latitude.ToString());
                                }
                                json.Append("\", ");
                                json.Append("\"lon\":\"");
                                if (cachedOrder.Tracker.Longitude.HasValue)
                                {
                                    json.Append(cachedOrder.Tracker.Longitude.ToString());
                                }
                                json.Append("\", ");
                                json.Append("\"active\":\"");
                                if (cachedOrder.Tracker.LastUpdated.Ticks > oldOrderCutoff.Ticks)
                                {
                                    json.Append("true");
                                }
                                else
                                {
                                    json.Append("false");
                                }
                                json.Append("\"");
                            }

                            json.Append("}");
                        }
                    }

                    json.Append("]}");
                }
            }
            catch (Exception exception)
            {
                DataAccess.LogEvent(exception,
                    "clientKey=\"" + clientKey == null ? "null" : clientKey +
                    "\" credentials=\"" + customerCredentials == null ? "null" : customerCredentials + "\"");

                errorCode = 1;
            }

            if (errorCode != 0)
            {
                // Return an error
                json = new StringBuilder();
                json.Append("{\"error\":{ \"errorCode\":\"");
                json.Append(errorCode.ToString());
                json.Append("\"}}");
            }

            response = json.ToString();

            return response;
        }
    }
}