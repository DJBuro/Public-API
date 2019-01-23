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
using System.Threading.Tasks;

namespace AndroCloudWCFServices.Services
{
    public class Feedback
    {
        /// <summary>
        /// Submit feedback
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string PutFeedback(DataTypes dataTypes, Stream input, string externalSiteId, string externalApplicationId)
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

                string data = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    data = Helper.StreamToString(input);

                    // Link the audit table to this application
                    sourceId = externalApplicationId;

                    // Send the order to the site
                    response = FeedbackService.Put
                    (
                        externalApplicationId, 
                        externalSiteId, 
                        data, 
                        dataTypes.SubmittedDataType, 
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory
                    );
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PutOrder", exception, dataTypes.WantsDataType);
                }

                // Log the call
                DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PutCustomerFeedback",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"esid\":\"" + externalSiteId + "\",\"f\":\"" + data + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}