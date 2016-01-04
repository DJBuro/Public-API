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
    public class Site
    {
        /// <summary>
        /// Gets the full details of the specific site
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetSite
        (
            DataTypes dataTypes, 
            string externalSiteId, 
            string externalApplicationId,
            int gotMenuVersion,
            bool? statusCheck
        )
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

                    // Get the site details from the datastore
                    response = SiteService.Get(externalApplicationId, externalSiteId, gotMenuVersion, statusCheck, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetSite", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetSite",
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
        /// Get a list of sites
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="maxDistanceFilter"></param>
        /// <param name="longitudeFilter"></param>
        /// <param name="latitudeFilter"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string GetSites(
            DataTypes dataTypes, 
            string externalPartnerId, 
            string maxDistanceFilter, 
            string longitudeFilter, 
            string latitudeFilter, 
            string deliveryZoneFilter,
            string externalApplicationId
        )
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

                    // Get the sites from the datastore
                    response = SiteService.Get(externalApplicationId, maxDistanceFilter, longitudeFilter, latitudeFilter, deliveryZoneFilter, dataTypes.WantsDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetSites", exception, dataTypes.WantsDataType);
                }

                // Log usefull stuff
                string extraInfo = "";

                if (maxDistanceFilter != null)
                {
                    if (extraInfo.Length > 0) extraInfo += ",";
                    extraInfo += "\"mdf\":\"" + maxDistanceFilter + "\"";
                }

                if (longitudeFilter != null)
                {
                    if (extraInfo.Length > 0) extraInfo += ",";
                    extraInfo +=  "\"lof\":\"" + longitudeFilter + "\"";
                }

                if (latitudeFilter != null)
                {
                    if (extraInfo.Length > 0) extraInfo += ",";
                    extraInfo += "\"laf\":\"" + latitudeFilter + "\"";
                }

                if (extraInfo.Length > 0) extraInfo = "{" + extraInfo + "}";

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetSites",
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