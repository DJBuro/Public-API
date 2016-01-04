using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;

namespace AndroCloudPrivateWCFServices.Services
{
    public class Hosts
    {
        /// <summary>
        /// Gets a list of hosts
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="androSiteId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <returns></returns>
        public static string GetHosts(DataTypes dataTypes, string androSiteId, string licenseKey, string hardwareKey)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the hosts the datastore
                    string sourceId = "";
                    response = HostService.GetAllForSite(androSiteId, licenseKey, hardwareKey, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    Global.Log.Error("GetHosts", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "GetHosts",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"asid\":\"" + androSiteId + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Gets a list of hosts
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="androSiteId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <returns></returns>
        public static string GetHostsV2(DataTypes dataTypes, string androSiteId, string licenseKey, string hardwareKey)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the hosts from the datastore
                    string sourceId = "";
                    response = HostService.GetAllForSiteV2(androSiteId, licenseKey, hardwareKey, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    Global.Log.Error("GetHostsV2", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "GetHostsV2",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"asid\":\"" + androSiteId + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}
