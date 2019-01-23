using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.IO;
using System.Text;
using System.Net;
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;
using AndroCloudDataAccess;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace AndroCloudWCFServices.Services
{
    public class Order
    {
        /// <summary>
        /// Get an existing order
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalOrderId"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetOrder(DataTypes dataTypes, string externalSiteId, string externalOrderId, string externalPartnerId, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;
                string sourceId = "";

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // For backward compatibility use partner id if application id not provided
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        externalApplicationId = externalPartnerId;
                    }

                    // Get the order status
                    response = OrderService.Get(externalApplicationId, externalSiteId, externalOrderId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetOrder", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetOrder",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\",\"eoid\":\"" + externalOrderId + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalOrderId"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static async Task<string> PutOrder(DataTypes dataTypes, Stream input, string externalSiteId, string externalOrderId, string externalPartnerId, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;
                string sourceId = "";

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string order = "";

                // await wipes out the context - keep a reference to it for later
                OperationContext operationContext = OperationContext.Current;

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    order = Helper.StreamToString(input);

                    // For backward compatibility use partner id if application id not provided
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        externalApplicationId = externalPartnerId;
                    }

                    // Link the audit table to this application
                    sourceId = externalApplicationId;

                    // Send the order to the site
                    response = await OrderService.Put(
                        externalApplicationId, 
                        externalSiteId, 
                        externalOrderId, 
                        order, 
                        dataTypes.SubmittedDataType, 
                        DataAccessHelper.DataAccessFactory);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PutOrder", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PutOrder",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\",\"eoid\":\"" + externalOrderId + "\",\"o\":\"" + order + "\"}");

                // Restore the context wiped out by await
                using (new OperationContextScope(operationContext))
                {
                    // Stream the result back
                    Helper.FinishWebCall(dataTypes.WantsDataType, response);
                }

                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Gets a customers orders
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="username">Customers username</param>
        /// <param name="password">Customers password</param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetCustomerOrders(DataTypes dataTypes, string username, string password, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;
                string sourceId = "";

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the customers orders
                    response = OrderService.GetCustomerOrders(
                        externalApplicationId, 
                        username, 
                        password, 
                        dataTypes.SubmittedDataType, 
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetCustomerOrders", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetCustomerOrders",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"euid\":\"" + username + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Get a customers order
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId">External id of the required order</param>
        /// <param name="username">Customers username</param>
        /// <param name="password">Customers password</param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetCustomerOrderDetails(DataTypes dataTypes, string externalOrderId, string username, string password, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;
                string sourceId = "";

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the customers order
                    response = OrderService.GetCustomerOrder(
                        externalApplicationId,
                        externalOrderId,
                        username,
                        password,
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetCustomerOrderDetails", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetCustomerOrderDetails",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"eoid\":\"" + externalOrderId + "\",\"euid\":\"" + username + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Checks the vouchers in an order.  Does NOT send the order to a store
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalOrderId"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string CheckOrderVouchers(DataTypes dataTypes, Stream input, string externalSiteId, string externalPartnerId, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;
                string sourceId = "";

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string order = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    order = Helper.StreamToString(input);

                    // For backward compatibility use partner id if application id not provided
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        externalApplicationId = externalPartnerId;
                    }

                    // Link the audit table to this application
                    sourceId = externalApplicationId;

                    // Check the vouchers in the order
                    response = OrderService.CheckOrderVouchers(
                        externalApplicationId, 
                        externalSiteId, 
                        order, 
                        dataTypes.SubmittedDataType, 
                        DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory
                        );
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("CheckOrderVouchers", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "CheckOrderVouchers",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\",\"o\":\"" + order + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}