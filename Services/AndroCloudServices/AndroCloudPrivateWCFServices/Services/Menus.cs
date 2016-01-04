using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AndroCloudPrivateWCFServices.Services
{
    public class Menus
    {
        /// <summary>
        /// Update a menu
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="androSiteId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string PostMenu(DataTypes dataTypes, Stream input, string androSiteId, string licenseKey, string hardwareKey, string version)
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

                    // Get the POST payload
                    string menu = Helper.StreamToString(input);

                    // Add the menu to the datastore
                    string sourceId = "";
                    response = MenuService.Post(androSiteId, licenseKey, hardwareKey, version, menu, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    Global.Log.Error("PostMenu", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "PostMenu",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"v\":\"" + version + "\",\"asid\":\"" + androSiteId + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;


            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        
    }
}
