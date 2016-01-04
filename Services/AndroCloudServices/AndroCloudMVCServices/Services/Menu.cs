using System.IO;
using System.Net;
using System.Text;
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;
using AndroCloudWCFServices;
using System.Web;

namespace AndroCloudMVCServices.Services
{
    public class Menu
    {
        public static string GetMenu(DataTypes dataTypes, string externalSiteId, string externalPartnerId, string externalApplicationId, HttpContextBase httpContext)
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

                // Get the menu from the datastore
                response = MenuService.Get(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
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
            return response.ResponseText;
        }

        public static string PostMenu(DataTypes dataTypes, Stream input, string siteGuid, string licenseKey, string hardwareKey, string version, HttpContextBase httpContext)
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

                // Get the POST payload
                string menu = Helper.StreamToString(input);

                // Add the menu to the datastore
                response = MenuService.Post(siteGuid, licenseKey, hardwareKey, version, menu, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("PostMenu", exception);

                // Unhandled error
                response = new Response(Errors.InternalError, dataTypes.WantsDataType);
            }

            // Log the call
            DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                sourceId,
                "",
                callerIPAddress,
                "PostMenu",
                (int)stopWatch.Elapsed.TotalMilliseconds,
                (int?)response.Error.ErrorCode);

            // Stream the result back
            Helper.FinishWebCall(httpContext, dataTypes.WantsDataType, response);
            responseText = response.ResponseText;

            return responseText;
        }
    }
}
