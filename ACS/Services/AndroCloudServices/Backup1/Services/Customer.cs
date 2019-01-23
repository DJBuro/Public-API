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
    public class Customer
    {
        public static string Get(
            DataTypes dataTypes,
            string username,
            string password,
            string externalStoreId,
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

                    // Get the customer from the datastore
                    response = CustomerService.Get(
                        externalApplicationId, 
                        username, 
                        password, 
                        externalStoreId,
                        dataTypes.WantsDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory, 
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("GetCustomer", exception, dataTypes.WantsDataType);
                }

                // Log the call
                AndroCloudDataAccess.DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "GetCustomer",
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
        /// Create a new customer
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string Put(DataTypes dataTypes, Stream input, string username, string password, string externalSiteId, string externalApplicationId)
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

                string customer = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    customer = Helper.StreamToString(input);

                    // Create the customer
                    response = CustomerService.Put(
                        externalApplicationId, 
                        username, 
                        password, 
                        customer,
                        externalSiteId,
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
                    "{\"u\":\"" + username + "\",\"c\":\"" + customer + "\"}");

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
        public static string Post(DataTypes dataTypes, Stream input, string username, string password, string externalApplicationId, string newPassword)
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

                string customer = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    customer = Helper.StreamToString(input);

                    // Update the customer
                    response = CustomerService.Post(
                        externalApplicationId, 
                        username, 
                        password, 
                        newPassword,
                        customer, 
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
                    "{\"u\":\"" + username + "\",\"c\":\"" + customer + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        public static string PostCustomerLoyalty(DataTypes dataTypes, Stream input, 
            string username, string externalApplicationId
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

                string customer = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    customer = Helper.StreamToString(input);

                    // Update the customer
                    response = CustomerService.PostLoyalty(
                        externalApplicationId,
                        username,
                        customer,
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PostCustomerLoyalty", exception, dataTypes.WantsDataType);
                }

                // Log the call
                AndroCloudDataAccess.DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PostCustomerLoyalty",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"u\":\"" + username + "\",\"c\":\"" + customer + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }


        public static string PostApplyCustomerLoyalty(
            DataTypes dataTypes,
            Stream input,
            string username,
            string externalOrderRef,
            string action,
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

                string customer = "";

                try
                {
                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Get the PUT payload
                    //customer = Helper.StreamToString(input);

                    // Update the customer
                    response = CustomerService.PostApplyCustomerLoyalty(
                        externalApplicationId,
                        username,
                        externalOrderRef,
                        action,
                        dataTypes.SubmittedDataType,
                        AndroCloudDataAccess.DataAccessHelper.DataAccessFactory,
                        DataWarehouseDataAccess.DataAccessHelper.DataAccessFactory,
                        out sourceId);
                }
                catch (Exception exception)
                {
                    response = Helper.ProcessUnhandledException("PostApplyCustomerLoyalty", exception, dataTypes.WantsDataType);
                }

                // Log the call
                AndroCloudDataAccess.DataAccessHelper.DataAccessFactory.AuditDataAccess.Add(
                    sourceId,
                    "",
                    callerIPAddress,
                    "PostApplyCustomerLoyalty",
                    (int)stopWatch.Elapsed.TotalMilliseconds,
                    (int?)response.Error.ErrorCode,
                    "{\"u\":\"" + username + "\",\"c\":\"" + customer + "\"}");

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}