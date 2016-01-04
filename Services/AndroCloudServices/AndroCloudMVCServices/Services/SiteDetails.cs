using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Net;
using AndroCloudServices;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;
using System.Diagnostics;
using AndroCloudServices.Services;
using AndroCloudDataAccess;
using AndroCloudWCFServices;

namespace AndroCloudMVCServices.Services
{
    public class SiteDetails
    {
        public static string GetSiteDetails(DataTypes dataTypes, string externalPartnerId, string externalSiteId, string externalApplicationId, HttpContextBase httpContext)
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

                // Get the site details from the datastore
                response = SiteDetailsService.Get(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("GetSiteDetails", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "GetSiteDetails",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            return response.ResponseText;
        }
    }
}