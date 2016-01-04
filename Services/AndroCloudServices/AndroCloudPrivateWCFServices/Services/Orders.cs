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
using AndroCloudServices.Domain;
using System.Threading.Tasks;

namespace AndroCloudPrivateWCFServices.Services
{
    public class Orders
    {
        /// <summary>
        /// Create an order (from Rameses)
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="androSiteId"></param>
        /// <param name="orderId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <returns></returns>
        public static async Task<string> PutOrder(
            DataTypes dataTypes, 
            Stream input, 
            string androSiteId,
            string externalOrderId, 
            string licenseKey, 
            string hardwareKey)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string order = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the POST payload
                    order = Helper.StreamToString(input);

                    // Process the order status update
                    string sourceId = androSiteId;
                    response = await OrderService.PutRameses
                    (
                        androSiteId,
                        externalOrderId,
                        order,                        
                        licenseKey,
                        hardwareKey,
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory
                    );
                }
                catch (Exception exception)
                {
                    Global.Log.Error("PostOrder", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "PutOrder",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"oid\":\"" + externalOrderId + "\",\"asid\":\"" + androSiteId + "\",\"order\":\"" + order + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Update an order
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="androSiteId"></param>
        /// <param name="orderId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <returns></returns>
        public static string PostOrder(
            DataTypes dataTypes, Stream input, string androSiteId, 
            string externalOrderId, string licenseKey, string hardwareKey,
            out OrderStatusUpdate orderStatusUpdate)
        {
            orderStatusUpdate = null;
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string order = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the POST payload
                    order = Helper.StreamToString(input);

                    // Process the order status update
                    string sourceId = androSiteId;
                    response = OrderService.Post
                    (
                        order, 
                        androSiteId, 
                        externalOrderId, 
                        licenseKey, 
                        hardwareKey, 
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out orderStatusUpdate
                    );
                }
                catch (Exception exception)
                {
                    Global.Log.Error("PostOrder", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "PostOrder",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"oid\":\"" + externalOrderId + "\",\"asid\":\"" + androSiteId + "\",\"order\":\"" + order + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}