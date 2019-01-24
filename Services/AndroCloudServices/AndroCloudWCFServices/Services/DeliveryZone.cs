namespace AndroCloudWCFServices.Services
{
    using System;
    using System.Diagnostics;

    using AndroCloudDataAccess;

    using AndroCloudHelper;

    using AndroCloudServices.Services;

    internal class DeliveryZone
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
            string responseText;

            try
            {
                string callerIpAddress = string.Empty;
                Response response;

                // Measure how long this call takes
                Stopwatch stopWatch = Stopwatch.StartNew();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIpAddress = Helper.GetClientIPAddressPortString();

                    // Get the sites from the datastore
                    response = DeliveryZoneService.Get(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetDeliveryZones", exception, dataTypes.WantsDataType);
                }

                // Log usefull stuff
                string extraInfo = string.Empty;

                if (externalSiteId != null)
                {
                    if (extraInfo.Length > 0)
                    {
                        extraInfo += ",";
                    }

                    extraInfo += "\"sid\":\"" + externalSiteId + "\"";
                }

                if (extraInfo.Length > 0)
                {
                    extraInfo = "{" + extraInfo + "}";
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    externalApplicationId,
                    string.Empty,
                    callerIpAddress,
                    "GetDeliveryZones",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    extraInfo);

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception)
            {
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            return responseText;
        }
    }
}