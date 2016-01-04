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
using AndroCloudDataAccess.Domain;
using AndroCloudServices;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Services;
using AndroCloudDataAccess;
using AndroCloudServices.Domain;

namespace AndroCloudPrivateWCFServices.Services
{
    public class Sites
    {
        /// <summary>
        /// Update a site
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="androSiteId"></param>
        /// <param name="licenseKey"></param>
        /// <param name="hardwareKey"></param>
        /// <returns></returns>
        public static string PostSite(DataTypes dataTypes, Stream input, string androSiteId, string licenseKey, string hardwareKey, 
            out SiteUpdate siteUpdate)
        {
            siteUpdate = null;

            string responseText = "";

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                string site = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the POST payload
                    site = Helper.StreamToString(input);

                    // Process the site ETD update
                    string sourceId = androSiteId;
                    response = SiteService.Post(site, androSiteId, licenseKey, hardwareKey, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out siteUpdate);
                }
                catch (Exception exception)
                {
                    Global.Log.Error("PostSite", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    callerIPAddress,
                    "",
                    callerIPAddress,
                    "PostSite",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"hk\":\"" + hardwareKey + "\",\"asid\":\"" + androSiteId + "\",\"site\":\"" + site + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}