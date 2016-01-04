using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using AndroAdminDataAccess.DataAccess;

namespace Andromeda.WebOrderTracking
{
    internal class WebOrderTrackingManager
    {              
        internal static int Order(string clientKey, string customerCredentials, out List<Order> orders)
        {
            int errorCode = 0;
            orders = new List<Order>();
            
            // If there are no calls from external clients for X amount of time then we automatically stop polling the GPSGate 
            // server for tracker locations. If we've stopped polling then we'll need to do an immediate poll now.
            CacheManager.CheckIfPollRequired();

            // Do order details DB lookup
            bool success = DataAccess.GetOrders(clientKey, customerCredentials, out orders);
            
            if (orders.Count > 0)
            {
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
                            // We haven't encountered this customer before.  Add it to the cache for this client
                            cachedCustomer = new CachedCustomer();
                            cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
                        }
                    }
                    else
                    {
                        // We haven't encountered this client before.  Add it to the cache
                        cachedClientCustomers = new CachedClientCustomers();
                        CacheManager.CustomersByClientKey.Add(clientKey, cachedClientCustomers);

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
                // Unknown client key
                errorCode = 2; // "No orders found for the customer credentials";
                
                // TODO log error

            }

            return errorCode;
        }

        internal static int OrderLocation(string clientKey, string customerCredentials, out List<CachedOrder> cachedOrders)
        {
            int errorCode = 0;

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
                        errorCode = 3; // "Invalid customer credentials";
                    
                        // We haven't encountered this customer before.  Add it to the cache for this client
 //                       cachedCustomer = new CachedCustomer();
//                        cachedClientCustomers.CustomersByCustomerCredentials.Add(customerCredentials, cachedCustomer);
                    }
                }
                else
                {
                    errorCode = 4; // "Invalid client key";
                }
                //else
                //{
                //    // We haven't encountered this client before.  Add it to the cache
                //    cachedClientCustomers = new CachedClientCustomers();
                //    CacheManager.CustomersByClientKey.Add(clientKey, cachedClientCustomers);
                //}

                // If CachedCustomer exists
                if (errorCode == 0)
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
                        bool success = DataAccess.GetPollOrders(customerCredentials, customerOrderIds, out pollOrders);

                        //  If no customer orders in db
                        //    Return error
                        if (pollOrders.Count == 0)
                        {
                            errorCode = 5; // "Customer orders not found";
                        }

                        if (errorCode == 0)
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

            return errorCode;
        }        
    }
}
