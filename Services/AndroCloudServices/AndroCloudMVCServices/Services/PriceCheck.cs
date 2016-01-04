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
    public class PriceCheck
    {
        public static string PutPriceCheck(DataTypes dataTypes, Stream input, string siteGuid, string externalPartnerId, string externalApplicationId, HttpContextBase httpContext)
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

                // Get the PUT payload
                string order = Helper.StreamToString(input);

                // Send the order for price checking
                response = PriceCheckService.Put(externalApplicationId, siteGuid, order, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("PutPriceCheck", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "PutPriceCheck",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            return response.ResponseText;
        }
    }
}