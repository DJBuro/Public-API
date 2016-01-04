using AndroCloudHelper;
using PrivateSignpost.Helper;
using PrivateSignpost.Models;
using SignpostDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PrivateSignpost.Services
{
    public class SyncServices
    {
        public async static Task<ResponseDetails> GetDataVersionAsync(ISignpostDataAccess signpostDataAccess, string secretKey)
        {
            ResponseDetails responseDetails = new ResponseDetails();

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    WebApiApplication.Log.Debug("GetDataVersion called");

                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = AndroCloudHelper.Helper.GetClientIPAddressPortString();

                    // Was an secretKey provided?
                    if (secretKey != "CDEF9B9357CD47B7A87CDF510674C327")
                    {
                        // Wrong secret key
                        response = new Response(Errors.Sync_InvalidSecretKey, DataTypeEnum.JSON);
                    }

                    if (response == null)
                    {
                        // Get data version
                        int dataVersion = -1;
                        string message = signpostDataAccess.GetDataVersion(out dataVersion);

                        if (String.IsNullOrEmpty(message))
                        {
                            response = new Response("{ \"version\":" + dataVersion.ToString() + " }");
                        }
                    }
                }
                catch (Exception exception)
                {
                    WebApiApplication.Log.Error("GetDataVersion", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, DataTypeEnum.JSON);
                }

                // Stream the result back
                responseDetails.ResponseText = response.ResponseText;
                responseDetails.httpStatusCode = AndroCloudHelper.Helper.GetHttpStatusCode(response.Result);
            }
            catch (Exception exception)
            {
                responseDetails.ResponseText = AndroCloudHelper.Helper.ProcessCatastrophicException(exception);
                responseDetails.httpStatusCode = HttpStatusCode.InternalServerError;
            }

            return responseDetails;
        }

        public async static Task<ResponseDetails> SyncAsync(ISignpostDataAccess signpostDataAccess, string secretKey, Sync sync)
        {
            ResponseDetails responseDetails = new ResponseDetails();

            try
            {
                string callerIPAddress = "";
                Response response = null;

                // Measure how long this call takes
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                try
                {
                    WebApiApplication.Log.Debug("SyncAsync called");

                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = AndroCloudHelper.Helper.GetClientIPAddressPortString();

                    // Was an secretKey provided?
                    if (secretKey != "CDEF9B9357CD47B7A87CDF510674C327")
                    {
                        // Wrong secret key
                        response = new Response(Errors.Sync_InvalidSecretKey, DataTypeEnum.JSON);
                        response.Result = ResultEnum.BadRequest;
                    }

                    // Was any JSON provided?
                    if (sync == null)
                    {
                        // No sync JSON
                        response = new Response(Errors.Sync_MissingSyncJson, DataTypeEnum.JSON);
                        response.Result = ResultEnum.BadRequest;
                    }

                    if (response == null)
                    {
                        // Do the sync
                        response = await SyncServices.Sync(signpostDataAccess, sync);

                  //      response = new Response("All good");
                    }
                }
                catch (Exception exception)
                {
                    WebApiApplication.Log.Error("SyncAsync", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, DataTypeEnum.JSON);
                }

                // Stream the result back
                responseDetails.ResponseText = response.ResponseText;
                responseDetails.httpStatusCode = AndroCloudHelper.Helper.GetHttpStatusCode(response.Result);
            }
            catch (Exception exception)
            {
                responseDetails.ResponseText = AndroCloudHelper.Helper.ProcessCatastrophicException(exception);
                responseDetails.httpStatusCode = HttpStatusCode.InternalServerError;
            }

            return responseDetails;
        }

        private async static Task<Response> Sync(ISignpostDataAccess signpostDataAccess, Sync sync)
        {
            Response response = null;

            try
            {
                //Sync sync = null;
                string error = null; // SerializeHelper.Deserialize<Sync>(syncJson, DataTypeEnum.JSON, out sync);

                //if (error != null && error.Length > 0)
                //{
                //    response = new Response(Errors.Sync_InvalidSyncJson, DataTypeEnum.JSON);
                //}

                List<SignpostDataAccessLayer.Models.ACSApplication> acsApplications = new List<SignpostDataAccessLayer.Models.ACSApplication>();

                if (response == null)
                {
                    // We have sync data!
                    foreach (ACSApplicationSync acsApplicationSync in sync.acsApplicationSyncs)
                    {
                        // Convert the ACSApplications environment id to a GUID
                        Guid environmentId = Guid.Empty;
                        if (!Guid.TryParse(acsApplicationSync.environmentId, out environmentId))
                        {
                            response = new Response(Errors.Sync_InvalidACSApplicationEnvironmentId, DataTypeEnum.JSON);
                            break;
                        }

                        if (response == null)
                        {
                            // Add or update the ACS application
                            SignpostDataAccessLayer.Models.ACSApplication acsApplication = new SignpostDataAccessLayer.Models.ACSApplication()
                            {
                                id = acsApplicationSync.id,
                                name = acsApplicationSync.name,
                                externalApplicationId = acsApplicationSync.externalApplicationId,
                                environmentId = environmentId,
                                acsApplicationSites = acsApplicationSync.acsApplicationSites
                            };

                            // We've got enough data to sync this acs application
                            acsApplications.Add(acsApplication); 
                        }
                    }
                }

                if (response == null)
                {
                    // Do the sync
                    error = signpostDataAccess.AddUpdateACSApplications(sync.fromVersion, sync.toVersion, acsApplications);

                    if (!String.IsNullOrEmpty(error))
                    {
                        response = new Response(new Error(error, Errors.InternalError.ErrorCode, ResultEnum.InternalServerError), DataTypeEnum.JSON);
                    }
                }

                if (response == null)
                {
                    response = new Response() { Result = ResultEnum.NoError };
                }
            }
            catch (Exception exception)
            {
                response = new Response(Errors.InternalError, DataTypeEnum.JSON);
            }

            return response;
        }
    }
}