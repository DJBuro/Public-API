using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;
using AndroCloudDataAccess;
using AndroCloudWCFServices;

namespace AndroCloudMVCServices.Services
{
    public class Order
    {
        public static string GetOrder(DataTypes dataTypes, string externalSiteId, string orderGuid, string externalPartnerId, string externalApplicationId, HttpContextBase httpContext)
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
                callerIPAddress = Helper.GetClientIPAddressPortString(httpContext);

                // For backward compatibility use partner id if application id not provided
                if (externalApplicationId == null || externalApplicationId.Length == 0)
                {
                    externalApplicationId = externalPartnerId;
                }

                // Get the order status
                response = OrderService.Get(externalApplicationId, externalSiteId, orderGuid, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("GetOrder", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "GetOrder",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            return response.ResponseText;
        }

        public static string PutOrder(DataTypes dataTypes, Stream input, string externalSiteId, string externalOrderId, string externalPartnerId, string externalApplicationId, HttpContextBase httpContext)
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
                callerIPAddress = Helper.GetClientIPAddressPortString(httpContext);

                // Get the PUT payload
                string order = Helper.StreamToString(input);

                // For backward compatibility use partner id if application id not provided
                if (externalApplicationId == null || externalApplicationId.Length == 0)
                {
                    externalApplicationId = externalPartnerId;
                }

                // Send the order to the site
                response = OrderService.Put(externalApplicationId, externalSiteId, externalOrderId, order, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("PutOrder", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "PutOrder",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            return response.ResponseText;
        }
    }
}