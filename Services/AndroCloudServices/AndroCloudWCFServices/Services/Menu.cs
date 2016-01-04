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

namespace AndroCloudWCFServices.Services
{
    public class Menu
    {
        /// <summary>
        /// Get a store menu
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetMenu(DataTypes dataTypes, string externalSiteId, string externalPartnerId, string externalApplicationId)
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
                    response = Helper.ProcessUnhandledException("GetMenu", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetMenu",
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

        /// <summary>
        /// Gets a list of menu item images
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetMenuImages(DataTypes dataTypes, string externalSiteId, string externalApplicationId)
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

                    // Get the menu from the datastore
                    response = MenuService.GetMenuImages(externalApplicationId, externalSiteId, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetMenuImages", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
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
