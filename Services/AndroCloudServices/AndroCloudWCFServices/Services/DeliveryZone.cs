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
using AndroCloudServices;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using AndroCloudDataAccess;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Domain;
using AndroCloudServices.Services;

namespace AndroCloudWCFServices.Services
{
    public class DeliveryZone
    {
        /// <summary>
        /// Get a list of delivery zones
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetDeliveryZones(
            DataTypes dataTypes, 
            string externalSiteId,
            string externalApplicationId)
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

                    // Get the sites from the datastore
                    response = DeliveryZoneService.Get(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetDeliveryZones", exception, dataTypes.WantsDataType);
                }

                // Log usefull stuff
                string extraInfo = "";

                if (externalSiteId != null)
                {
                    if (extraInfo.Length > 0) extraInfo += ",";
                    extraInfo += "\"sid\":\"" + externalSiteId + "\"";
                }

                if (extraInfo.Length > 0) extraInfo = "{" + extraInfo + "}";

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetDeliveryZones",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    extraInfo);

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}