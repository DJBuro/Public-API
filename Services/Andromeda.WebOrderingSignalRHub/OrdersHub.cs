using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Andromeda.WebOrdering.PaymentProviders;
using Andromeda.WebOrdering;

namespace Andromeda.WebOrderingSignalRHub
{
    public class OrdersHub : Hub
    {
        private static readonly IDictionary<string, OrderDetails> orders = new ConcurrentDictionary<string, OrderDetails>();

        public override Task OnConnected()
        {
            string callerIPAddress = this.GetCallerIPAddress();

            Global.Log.Debug("Client connected cid:" + Context.ConnectionId + " cip:" + callerIPAddress);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            string callerIPAddress = this.GetCallerIPAddress();

            Global.Log.Debug("Client disconnected cid:" + Context.ConnectionId + " cip:" + callerIPAddress);

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            string callerIPAddress = this.GetCallerIPAddress();

            Global.Log.Debug("Client reconnected cid:" + Context.ConnectionId + " cip:" + callerIPAddress);

            return base.OnReconnected();
        }

        /// <summary>
        /// A new order is underway
        /// </summary>
        /// <param name="orderReference"></param>
        public string NewOrder(string orderReference, string paymentProvider)
        {
            string result = "";

            try
            {
                Global.Log.Debug("NewOrder called: " + orderReference);

                string callerIPAddress = this.GetCallerIPAddress();
                if (this.IsCallerAllowed(callerIPAddress))
                {
                    // Has the order already been processed?
                    if (OrdersHub.orders.ContainsKey(orderReference))
                    {
                        // Error!  Order shouldn't already exist
                        result = "OrderAlreadyProcessed";

                        Global.Log.Error("NewOrder: Order already processed");
                    }
                    else
                    {
                        // Add the order to the list of pending orders
                        OrdersHub.orders.Add(orderReference, new OrderDetails(orderReference, paymentProvider));

                        result = "OK";
                    }
                }
                else
                {
                    // Access denied
                    result = "AccessDenied";

                    Global.Log.Error("NewOrder: Caller not allowed: " + callerIPAddress);
                }
            }
            catch (Exception exception)
            {
                result = "UnhandledException";
                Global.Log.Error("NewOrder: Unhandled exception", exception);
            }

            return result;
        }

        /// <summary>
        /// Signals that an order has been completed (successfully or not)
        /// </summary>
        /// <param name="orderReference"></param>
        public string OrderCompleted(string orderReference, int errorCode)
        {
            string result = "";


            try
            {
                Global.Log.Debug("OrderCompleted called: " + orderReference + ", " + errorCode);

                string callerIPAddress = this.GetCallerIPAddress();
                if (this.IsCallerAllowed(callerIPAddress))
                {
                    OrderDetails orderDetails = null;

                    // Find the order that has been completed
                    if (OrdersHub.orders.TryGetValue(orderReference, out orderDetails))
                    {
                        // Update the order
                        orderDetails.IsCompleted = true;
                        orderDetails.ErrorCode = errorCode;

                        // Do we need to tell a client that the order is completed?
                        if (orderDetails.ClientConnectionId != null && orderDetails.ClientConnectionId.Length > 0)
                        {
                            string json = "{ \"status\": \"Completed\", \"errorCode\":" + orderDetails.ErrorCode.Value + " }";

                            Global.Log.Debug("OrderCompleted: signalling client " + orderDetails.ClientConnectionId + " " + json);

                            // Attempt to signal the client
                            Task task = Clients.Client(orderDetails.ClientConnectionId).orderCompleted(json);

                            // Wait for the call to finish
                            task.Wait();

                            if (task.IsCompleted)
                            {
                                // The customer now knows so we don't need the order any more
                                OrdersHub.orders.Remove(orderDetails.OrderReference);
                            }

                            result = "OK";
                        }
                        else
                        {
                            result = "NoClientRegistered";
                            Global.Log.Debug("OrderCompleted: no client registered");
                        }
                    }
                    else
                    {
                        // Order not found
                        result = "OrderNotFound";
                        Global.Log.Error("OrderCompleted: Order not found " + orderReference);
                    }
                }
                else
                {
                    // Access denied
                    result = "AccessDenied";
                    Global.Log.Error("OrderCompleted: Caller not allowed: " + callerIPAddress);
                }
            }
            catch (Exception exception)
            {
                result = "UnhandledException";
                Global.Log.Error("OrderCompleted: Unhandled exception", exception);
            }

            return result;
        }

        /// <summary>
        /// A customer wants to be signalled when an order has been processed
        /// </summary>
        /// <param name="orderReference"></param>
        public string RegisterForOrder(string orderReference)
        {
            string json = "";
            OrderDetails orderDetails = null;

            try
            {
                Global.Log.Debug("RegisterForOrder called orderRef: " + orderReference + " clientId:" + Context.ConnectionId);

                string callerIPAddress = this.GetCallerIPAddress();

                // Find the order that the customer wants to monitor
                if (OrdersHub.orders.TryGetValue(orderReference, out orderDetails))
                {
                    // Has the order already been processed?  It's not impossible that the payment went through very quickly
                    if (orderDetails.IsCompleted)
                    {
                        Global.Log.Debug("RegisterForOrder: order already completed");

                        json = "{ \"status\": \"Completed\", \"errorCode\":" + orderDetails.ErrorCode.Value + " }";

                        // The customer now knows, so we don't need the order any more
                        OrdersHub.orders.Remove(orderDetails.OrderReference);
                    }
                    else
                    {
                        Global.Log.Debug("RegisterForOrder: order pending");
                        
                        // Register this client to be called back
                        orderDetails.ClientConnectionId = Context.ConnectionId;
                        json = "{ \"status\": \"Pending\" }";
                    }
                }
                else
                {
                    // Order not found
                    json = "{ \"status\": \"OrderNotFound\" }";

                    Global.Log.Error("RegisterForOrder: Order not found: " + orderReference);
                }
            }
            catch (Exception exception)
            {
                json = "{ \"status\": \"UnhandledException\" }";
                Global.Log.Error("RegisterForOrder: Unhandled exception", exception);
            }

            return json;
        }

        internal string GetCallerIPAddress()
        {
            return (string)Context.Request.Environment["server.RemoteIpAddress"];
        }

        private bool IsCallerAllowed(string callerIPAddress)
        {
            // Get a list of IP addresses that are allowed to access this web service
            string lockToIPAddresses = ConfigurationManager.AppSettings["LockPrivateToIPAddresses"];
 //           Global.Log.Debug("IsCallerAllowed: callerIPAddress: " + callerIPAddress + " lockToIPAddresses:" + lockToIPAddresses);
            string[] allowedIPAddresses = lockToIPAddresses.Split('|');

            // Check to see if the caller is allowed to access the web service
            foreach (string allowedIPAddress in allowedIPAddresses)
            {
                if (callerIPAddress.StartsWith(allowedIPAddress))
                {
                    // Caller is allowed to access this web service
                    return true;
                }
            }

            // Caller is not allowed to access this web service
            return false;
        }
    }
}