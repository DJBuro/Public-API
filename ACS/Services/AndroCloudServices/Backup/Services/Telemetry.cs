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
    public class Telemetry
    {
        /// <summary>
        /// Start session
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string PutTelemetrySession(DataTypes dataTypes, Stream input, string externalSiteId, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                Response response = null;

                string data = "";

                try
                {
                    // Get the PUT payload
                    data = Helper.StreamToString(input);

                    // Send the order to the site
                    response = TelemetryService.PutSession
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
                    response = Helper.ProcessUnhandledException("PutTelemetrySession", exception, dataTypes.WantsDataType);
                }

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }

        /// <summary>
        /// Add to session
        /// </summary>
        /// <param name="dataTypes"></param>
        /// <param name="input"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="externalApplicationId"></param>
        /// <returns></returns>
        public static string PutTelemetry(DataTypes dataTypes, Stream input, string externalSiteId, string externalApplicationId)
        {
            string responseText = "";

            try
            {
                Response response = null;

                string data = "";

                try
                {
                    // Get the PUT payload
                    data = Helper.StreamToString(input);

                    // Send the order to the site
                    response = TelemetryService.Put
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
                    response = Helper.ProcessUnhandledException("PutTelemetry", exception, dataTypes.WantsDataType);
                }

                // Stream the result back
                Helper.FinishWebCall(dataTypes.WantsDataType, response);
                responseText = response.ResponseText;
            }
            catch (Exception exception) { responseText = Helper.ProcessCatastrophicException(exception); }

            return responseText;
        }
    }
}