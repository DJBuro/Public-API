using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudServices;
using AndroCloudServices.Services;
using AndroCloudWCFServices;

namespace AndroCloudMVCServices.Services
{
    public class Host
    {
        public static string GetAllHosts(DataTypes dataTypes, string externalPartnerId, string externalApplicationId, HttpContextBase httpContext)
        {
            string responseText = "";
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

                // Get the menu from the datastore
                response = HostService.GetAllForApplication(externalApplicationId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("GetMenu", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "GetMenu",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            responseText = response.ResponseText;

            return responseText;
        }
    }
}