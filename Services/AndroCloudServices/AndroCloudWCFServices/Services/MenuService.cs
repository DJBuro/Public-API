namespace AndroCloudWCFServices.Services
{
    using System;
    using System.Diagnostics;
    using AndroCloudDataAccess;
    using AndroCloudHelper;

    internal class MenuService
    {
        public static string GetMenu(DataTypes dataTypes, string externalSiteId, string externalPartnerId, string externalApplicationId, int version)
        {
            string responseText;

            try
            {
                string callerIpAddress = string.Empty;
                Response response;
                string sourceId = string.Empty;

                // Measure how long this call takes
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIpAddress = Helper.GetClientIPAddressPortString();

                    // For backward compatibility use partner id if application id not provided
                    if (string.IsNullOrEmpty(externalApplicationId))
                    {
                        externalApplicationId = externalPartnerId;
                    }

                    if (version == 2)
                    {
                        dataTypes.WantsDataType = DataTypeEnum.JsonV2;
                    }

                    // Get the menu from the datastore
                    response = AndroCloudServices.Services.MenuService.Get(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetMenu", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    string.Empty,
                    callerIpAddress,
                    "GetMenu",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\"}");

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

        public static string GetMenuImages(DataTypes dataTypes, string externalSiteId, string externalApplicationId)
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

                    // Get the menu from the datastore
                    response = AndroCloudServices.Services.MenuService.GetMenuImages(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetMenuImages", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    string.Empty,
                    callerIPAddress,
                    "GetMenuImages",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);

                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}