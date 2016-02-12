using OrderTracking.Models;
using OrderTracking.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderTracking.Services
{
    public class WebOrderTrackingServices
    {
        public async static Task<Response> StartSession(string applicationId, string customerCredentials)
        {
            Response response = new Response();

            StringBuilder json = new StringBuilder();
            try
            {
                // Check that the applicationId is correct and there is an one or more order for the customer credentials
                List<int> applicationStores = ACSDataAccess.GetApplicationStores(applicationId);

                if (applicationStores == null || applicationStores.Count == 0)
                {
                    // Either the application id is wrong or there are no stores allocated to it
                    response.SetError(ResponseErrorCodes.InvalidApplicationId);
                }

                List<Order> orders = null;
                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    bool success = OrderTrackingDataAccess.GetOrders(applicationStores, customerCredentials, out orders);

                    if (orders == null || orders.Count == 0)
                    {
                        // There are no orders for this customer (for this applicationId)
                        response.SetError(ResponseErrorCodes.CustomerHasNoOrders);
                    }
                }

                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    // Start the session
                    string sessionId = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
                    OrderTrackingSession orderTrackingSession = new OrderTrackingSession()
                    {
                        SessionId = sessionId,
                        ApplicationId = applicationId,
                        CustomerCredentials = customerCredentials
                    };
                    CacheManager.OrderTrackingSessionLookup.Add(sessionId, orderTrackingSession);

                    json = new StringBuilder();
                    json.Append("{\"sessionId\":\"");
                    json.Append(sessionId);
                    json.Append("\"}");
                }
            }
            catch (Exception exception)
            {
                //DataAccess.LogEvent(exception,
                //    "applicationId=\"" + (applicationId == null ? "null" : applicationId) +
                //    "\" credentials=\"" + (customerCredentials == null ? "null" : customerCredentials) + "\"");

                response.SetError(ResponseErrorCodes.InternalError);
            }

            return response;

        }

        public async static Task<Response> TrackOrder(string sessionId)
        {
            Response response = new Response();

            try
            {
                // Get the application id and customer credentias using the sessionId
                OrderTrackingSession orderTrackingSession = null;
                if (!CacheManager.OrderTrackingSessionLookup.TryGetValue(sessionId, out orderTrackingSession))
                {
                    // TODO LOG ERROR - better errors...
                    response.SetError(ResponseErrorCodes.InvalidSessionId);
                }

                List<Order> orders = null;
                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    // Get the live orders
                    response = WebOrderTrackingServices.Order(orderTrackingSession.ApplicationId, orderTrackingSession.CustomerCredentials, out orders);
                }

                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    // Build the json to return to the browser
                    StringBuilder json = new StringBuilder();
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

                            // Get tracker details
                            Tracker tracker = CacheManager.GetTrackerFromCache(orderTrackingSession.ApplicationId, orderTrackingSession.CustomerCredentials, order.TrackerName);

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

                    response.ResponseJSON = json.ToString();
                }
            }
            catch (Exception exception)
            {
                // TODO Log exception
                response.SetError(ResponseErrorCodes.InternalError);
            }

            return response;
        }

        public async static Task<Response> TrackOrderLocation(string sessionId)
        {
            Response response = new Response();

            try
            {
                List<CachedOrder> cachedOrders = null;

                // Get the application id and customer credentias using the sessionId
                OrderTrackingSession orderTrackingSession = null;
                if (!CacheManager.OrderTrackingSessionLookup.TryGetValue(sessionId, out orderTrackingSession))
                {
                    // TODO LOG ERROR - better errors...
                    response.SetError(ResponseErrorCodes.InvalidSessionId);
                }

                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {

                    // Get the live orders
                    response = WebOrderTrackingServices.OrderLocation(orderTrackingSession.ApplicationId, orderTrackingSession.CustomerCredentials, out cachedOrders);
                }

                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    StringBuilder json = new StringBuilder();

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

                    response.ResponseJSON = json.ToString();
                }
            }
            catch (Exception exception)
            {
                //DataAccess.LogEvent(exception,
                //    "applicationId=\"" + applicationId == null ? "null" : applicationId +
                //    "\" credentials=\"" + customerCredentials == null ? "null" : customerCredentials + "\"");

                response.SetError(ResponseErrorCodes.InternalError);
            }

            return response;
        }

        internal static Response Order(string applicationId, string customerCredentials, out List<Order> orders)
        {
            Response response = new Response();

            orders = new List<Order>();

            // If there are no calls from external clients for X amount of time then we automatically stop polling the GPSGate 
            // server for tracker locations. If we've stopped polling then we'll need to do an immediate poll now.
            CacheManager.CheckIfPollRequired();

            List<int> applicationStores = ACSDataAccess.GetApplicationStores(applicationId);

            // Do order details DB lookup
            bool success = OrderTrackingDataAccess.GetOrders(applicationStores, customerCredentials, out orders);

            if (orders.Count > 0)
            {
                lock (CacheManager.CacheLock)
                {
                    // Get customer data from cache
                    CachedClientCustomers cachedClientCustomers = null;
                    CachedCustomer cachedCustomer = null;

                    // Do we already have a client cached with the specified clientKey?
                    if (CacheManager.CustomersByClientKey.TryGetValue(applicationId, out cachedClientCustomers))
                    {
                        // Do we already have a customer cached for this client with the specified customerCredentials?
                        if (!cachedClientCustomers.CustomersByCustomerCredentials.TryGetValue(customerCredentials, out cachedCustomer))
                        {
                            // We haven't encountered this customer before.  Add it to the cache for this client
                            cachedCustomer = new CachedCustomer();
                            cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
                        }
                    }
                    else
                    {
                        // We haven't encountered this client before.  Add it to the cache
                        cachedClientCustomers = new CachedClientCustomers();
                        CacheManager.CustomersByClientKey.Add(applicationId, cachedClientCustomers);

                        // We haven't encountered this customer before.  Add it to the cache for this client
                        cachedCustomer = new CachedCustomer();
                        cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
                    }

                    // Ensure the customers orders are upto date in the cache so we don't have to keep getting them from the database
                    foreach (Order order in orders)
                    {
                        // Do we already have the order in the cache?
                        CachedOrder cachedOrder = null;
                        if (!cachedCustomer.OrdersByOrderId.TryGetValue(order.OrderId, out cachedOrder))
                        {
                            // We haven't encountered this order before.  Add it to the cache
                            cachedOrder = new CachedOrder();
                            cachedCustomer.OrdersByOrderId.Add(order.OrderId, cachedOrder);
                        }

                        // Make sure that the order details are upto-date
                        cachedOrder.OrderStatusId = order.Status;
                        cachedOrder.OrderId = order.OrderId;

                        // Do we need to update the tracker associated with this order?
                        if (cachedOrder.OrderStatusId == 4 &&
                            (cachedOrder.Tracker == null || cachedOrder.Tracker.Name != order.TrackerName))
                        {
                            // Does the tracker exist?
                            Tracker tracker = null;
                            if (!CacheManager.TrackersByTrackerName.TryGetValue(order.TrackerName, out tracker))
                            {
                                tracker = new Tracker(order.TrackerName, null, null);

                                // Add the tracker to the cache - the background thread will populate it's lat/lon
                                CacheManager.TrackersByTrackerName.Add(order.TrackerName, tracker);
                            }

                            cachedOrder.Tracker = tracker;
                        }

                        // Has the order been completed?
                        if ((cachedOrder.OrderStatusId == 5 || cachedOrder.OrderStatusId == 6) && cachedOrder.CompletedDateTime == null)
                        {
                            cachedOrder.CompletedDateTime = order.CompletedDateTime;
                        }
                        else
                        {
                            cachedOrder.CompletedDateTime = null;
                        }

                        if (cachedOrder.OrderStatusId != 4)
                        {
                            cachedOrder.Tracker = null;
                        }
                    }

                    // Keep track of when the customer last polled us
                    // We can use this to detect when the customer has not polled for a while and we can remove 
                    // the customer details from the cache
                    cachedCustomer.LastPolledDateTime = DateTime.Now;
                }
            }
            else
            {
                response.SetError(ResponseErrorCodes.CustomerHasNoOrders); // "No orders found for the customer credentials";

                // TODO log error

            }

            return response;
        }

        internal static Response OrderLocation(string clientKey, string customerCredentials, out List<CachedOrder> cachedOrders)
        {
            Response response = new Response();

            cachedOrders = new List<CachedOrder>();

            lock (CacheManager.CacheLock)
            {
                // Get customer data from cache
                CachedClientCustomers cachedClientCustomers = null;
                CachedCustomer cachedCustomer = null;

                // Do we already have a client cached with the specified clientKey?
                if (CacheManager.CustomersByClientKey.TryGetValue(clientKey, out cachedClientCustomers))
                {
                    // Do we already have a customer cached for this client with the specified customerCredentials?
                    if (!cachedClientCustomers.CustomersByCustomerCredentials.TryGetValue(customerCredentials, out cachedCustomer))
                    {
                        response.SetError(ResponseErrorCodes.UnknownCustomer);

                        // We haven't encountered this customer before.  Add it to the cache for this client
                        //                       cachedCustomer = new CachedCustomer();
                        //                        cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
                    }
                }
                else
                {
                    response.SetError(ResponseErrorCodes.UnknownCustomer); // "Invalid client key";
                }
                //else
                //{
                //    // We haven't encountered this client before.  Add it to the cache
                //    cachedClientCustomers = new CachedClientCustomers();
                //    CacheManager.CustomersByClientKey.Add(clientKey, cachedClientCustomers);
                //}

                // If CachedCustomer exists
                if (response.ErrorCode == ResponseErrorCodes.NoError)
                {
                    if (cachedCustomer == null)
                    {
                        //Else
                        //  Get CustomerOrders from db by CustomerId (RETURN EXTRA INFO)
                        //    inner join OrderStatus (on order id)
                        //    left join driverorders (on order id)
                        //    left join tracker (on driver id) 

                        //  If no customer orders in db
                        //    Return error

                        //  For each customerorder in db 
                        //    If status = 5 or 6
                        //      If CompletedDateTime > 10 mins ago
                        //        Skip order
                        //      Set CompletedDateTime
                        //    Create CachedOrder
                        //    If status = 4 
                        //      Check if tracker in the tracker cache
                        //      If in cache set CachedOrder.Tracker object
                        //      Else create Tracker object & add to tracker cache and CachedOrder
                    }
                    else
                    {
                        //  Set CachedCustomer.LastPolledDateTime
                        cachedCustomer.LastPolledDateTime = DateTime.Now;

                        //  Get CustomerOrders from db by CustomerId 

                        // We shouldn't really embed SQL in here but there's no point iterating through the orders twice
                        string customerOrderIds = "";
                        foreach (int orderId in cachedCustomer.OrdersByOrderId.Keys)
                        {
                            if (customerOrderIds.Length > 0)
                            {
                                customerOrderIds += ",";
                            }
                            customerOrderIds += orderId.ToString();
                        }

                        // Get order details
                        List<PollOrder> pollOrders = null;
                        bool success = OrderTrackingDataAccess.GetPollOrders(customerCredentials, customerOrderIds, out pollOrders);

                        //  If no customer orders in db
                        //    Return error
                        if (pollOrders.Count == 0)
                        {
                            response.SetError(ResponseErrorCodes.CustomerHasNoOrders); // "Customer orders not found";
                        }

                        if (response.ErrorCode == ResponseErrorCodes.NoError)
                        {
                            // For each customerorder in db
                            foreach (PollOrder pollOrder in pollOrders)
                            {
                                CachedOrder cachedOrder = null;

                                //    If CachedOrder exists
                                if (cachedCustomer.OrdersByOrderId.TryGetValue(pollOrder.OrderId, out cachedOrder))
                                {
                                    //       Update order status
                                    cachedOrder.OrderStatusId = pollOrder.Status;

                                    //       If status = 4 and CachedOrder.Tracker is null or != db tracker
                                    if (pollOrder.TrackerName.Length > 0 &&
                                        cachedOrder.OrderStatusId == 4 &&
                                        (cachedOrder.Tracker == null ||
                                        cachedOrder.Tracker.Name != pollOrder.TrackerName))
                                    {
                                        //         Check if tracker in the tracker cache
                                        Tracker tracker = null;
                                        if (!CacheManager.TrackersByTrackerName.TryGetValue(pollOrder.TrackerName, out tracker))
                                        {
                                            //         Else create Tracker object & add to tracker cache and CachedOrder
                                            tracker = new Tracker(pollOrder.TrackerName, null, null);

                                            // Add the tracker to the cache - the background thread will populate it's lat/lon
                                            CacheManager.TrackersByTrackerName.Add(pollOrder.TrackerName, tracker);
                                        }

                                        cachedOrder.Tracker = tracker;
                                    }

                                    // Has the order been completed?
                                    if ((cachedOrder.OrderStatusId == 5 || cachedOrder.OrderStatusId == 6) && cachedOrder == null)
                                    {
                                        cachedOrder.CompletedDateTime = pollOrder.OrderCompletedDateTime;
                                    }
                                    else
                                    {
                                        cachedOrder.CompletedDateTime = null;
                                    }

                                    cachedOrders.Add(cachedOrder);
                                }
                                else
                                {
                                    // Cached order does not exist
                                    //       If status = 5 or 6
                                    if (pollOrder.Status == 5 || pollOrder.Status == 6)
                                    {
                                        //         If CompletedDateTime > 10 mins ago
                                        TimeSpan timeSpan = DateTime.Now - pollOrder.OrderCompletedDateTime.Value;
                                        //                                   if (pollOrder
                                        //           Skip order
                                        //         Set CompletedDateTime
                                        //       Create CachedOrder
                                        //       If status = 4 
                                        //         Check if tracker in the tracker cache
                                        //         If in cache set CachedOrder.Tracker object
                                        //         Else create Tracker object & add to tracker cache and CachedOrder
                                    }
                                }
                            }
                        }
                    }
                }
            }



            //If LastExternalCallDateTime > 10 minutes
            //  Poll now
            //Update LastExternalCallDateTime

            //            cachedCustomer.LastPolledDateTime = DateTime.Now;

            return response;
        }
    }
}