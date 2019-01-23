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
    public class DeliveryRoad
    {
        /// <summary>
        /// Get a list of roads
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="postcode"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetDeliveryRoads(
            DataTypes dataTypes, 
            string postcode,
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

                    // Get the roads from the datastore
                    response = DeliveryZoneRoadService.Get(externalApplicationId, postcode, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetDeliveryRoads", exception, dataTypes.WantsDataType);
                }

                // Log usefull stuff
                string extraInfo = "";

                if (postcode != null)
                {
                    if (extraInfo.Length > 0) extraInfo += ",";
                    extraInfo += "\"p\":\"" + postcode + "\"";
                }

                if (extraInfo.Length > 0) extraInfo = "{" + extraInfo + "}";

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetDeliveryRoads",
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