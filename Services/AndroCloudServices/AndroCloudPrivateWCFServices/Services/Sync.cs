using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudServices;
using AndroCloudServices.Services;

namespace AndroCloudPrivateWCFServices.Services
{
    public class Sync
    {
        /// <summary>
        /// Get the current version of the data held in ACS
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDataVersion(DataTypes dataTypes, string key)
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

                    // Check that the caller is allowed to access the web service
                    if (Sync.IsCallerAllowed(callerIPAddress))
                    {
                        // Do the import
                        response = SyncService.GetDataVersion(key, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory);
                    }
                    else
                    {
                        Global.Log.Error("GetDataVersion: Unauthorized access blocked for IP " + callerIPAddress);

                        // Tell the user that they've been blocked
                        response = new Response(Errors.SyncAccessDenied, dataTypes.WantsDataType);
                    }
                }
                catch (Exception exception)
                {
                    Global.Log.Error("GetDataVersion", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "GetDataVersion",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Upgrade the data in ACS
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string PutSync(DataTypes dataTypes, Stream input, string key)
        {
            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string syncXml = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Check that the caller is allowed to access the web service
                    if (Sync.IsCallerAllowed(callerIPAddress))
                    {
                        // Get the POST payload
                        syncXml = Helper.StreamToString(input);

                        // Log the sync xml for debugging purposes
                        Global.Log.Debug(syncXml);

                        // Do the import
                        response = SyncService.ImportSyncXml(key, syncXml, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory);
                    }
                    else
                    {
                        Global.Log.Error("PutSync: Unauthorized access blocked for IP " + callerIPAddress);

                        // Tell the user that they've been blocked
                        response = new Response(Errors.SyncAccessDenied, dataTypes.WantsDataType);
                    }
                }
                catch (Exception exception)
                {
                    Global.Log.Error("PutSync", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "PostSync",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"sxml\":\"" + syncXml + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        private static bool IsCallerAllowed(string callerIPAddress)
        {
            bool enableLock = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSyncToIPLocking"]);
            if (!enableLock)
                return true;

            // Get a list of IP addresses that are allowed to access this web service
            string lockToIPAddresses = ConfigurationManager.AppSettings["LockSyncToIPAddresses"];
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