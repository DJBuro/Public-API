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
using DataWarehouseDataAccess;
using System.Diagnostics;
using AndroCloudHelper;
using AndroCloudServices.Domain;
using AndroCloudServices.Services;

namespace AndroCloudWCFServices.Services
{
    public class PasswordResetRequest
    {
        /// <summary>
        /// Create a new password reset request
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string Put(DataTypes dataTypes, string username, string externalApplicationId)
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

                    // Create the customer
                    response = PasswordResetRequestService.Put(
                        externalApplicationId, 
                        username, 
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory, 
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory, 
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PutCustomer", exception, dataTypes.WantsDataType);
                }

                // Log the call
                AndroCloudDataAccess.DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PutCustomer",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"u\":\"" + username + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string Post(DataTypes dataTypes, Stream input, string username, string newPassword, string externalApplicationId)
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

                string passwordResetRequest = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    passwordResetRequest = Helper.StreamToString(input);

                    // Update the customer
                    response = PasswordResetRequestService.Post(
                        externalApplicationId,
                        username,
                        passwordResetRequest,
                        newPassword,
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PostCustomer", exception, dataTypes.WantsDataType);
                }

                // Log the call
                AndroCloudDataAccess.DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PostCustomer",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"u\":\"" + username + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}