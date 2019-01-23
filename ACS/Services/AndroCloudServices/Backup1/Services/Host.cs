using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AndroCloudDataAccess;
using AndroCloudHelper;
using AndroCloudServices;
using AndroCloudServices.Services;

namespace AndroCloudWCFServices.Services
{
    public class Host
    {
        /// <summary>
        /// Get a list of hosts
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetHosts(DataTypes dataTypes, string externalPartnerId, string externalApplicationId)
        {
            string responseText = string.Empty;

            try
            {
                string callerIPAddress = string.Empty;
                Response response = null;
                string sourceId = string.Empty;

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

                    response = HostService.GetAllForApplication(externalApplicationId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    // Unhandled error
                    response = Helper.ProcessUnhandledException("GetHosts", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    string.Empty,
                    callerIPAddress,
                    "GetHosts",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    string.Empty);

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
        public static string GetHostsV2(DataTypes dataTypes, string externalApplicationId)
        {
            string responseText = string.Empty;

            try
            {
                string callerIPAddress = string.Empty;
                Response response = null;
                string sourceId = string.Empty;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the menu from the data store
                    response = HostService.GetAllForApplicationV2(externalApplicationId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    // Unhandled error
                    response = Helper.ProcessUnhandledException("GetHostsV2", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    string.Empty,
                    callerIPAddress,
                    "GetHostsV2",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    string.Empty);

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}