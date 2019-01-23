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

namespace AndroCloudWCFServices.Services
{
    public class PriceCheck
    {
        /// <summary>
        /// Get the price of an order as calculated by Rameses
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="siteGuid"></param>
        /// <param name="externalPartnerId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string PutPriceCheck(DataTypes dataTypes, Stream input, string siteGuid, string externalPartnerId, string externalApplicationId)
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

                string order = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // For backward compatibility use partner id if application id not provided
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        externalApplicationId = externalPartnerId;
                    }

                    // Get the PUT payload
                    order = Helper.StreamToString(input);

                    // Send the order for price checking
                    response = PriceCheckService.Put(externalApplicationId, siteGuid, order, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PutPriceCheck", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PutPriceCheck",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"o\":" + order + "}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}