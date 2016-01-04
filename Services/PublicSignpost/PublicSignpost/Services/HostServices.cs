using AndroCloudHelper;
using PublicSignpost.Models;
using SignpostDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PublicSignpost.Services
{
    public class HostServices
    {
        public async static Task<ResponseDetails> GetHostsV1Async(ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string partnerId, string externalApplicationId)
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
                    WebApiApplication.Log.Debug("GetHostsV1Async called");

                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Was a valid applicationId provided?
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        // Application id was not provided
                        response = new Response(Errors.MissingApplicationId, dataTypes.WantsDataType);
                        response.Result = ResultEnum.BadRequest;
                    }

                    if (response == null)
                    {
                        // Get hosts V1
                        response = await HostServices.GetHostsV1(signpostDataAccess, dataTypes, externalApplicationId);
                    }

                    //                   response = HostService.GetAllForSite(androSiteId, licenseKey, hardwareKey, dataTypes.SubmittedDataType, DataAccessHelper.DataAccessFactory, out sourceId);
                }
                catch (Exception exception)
                {
                    WebApiApplication.Log.Error("GetHostsV1Async", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Stream the result back
                responseDetails.ResponseText = response.ResponseText;
                responseDetails.httpStatusCode = Helper.GetHttpStatusCode(response.Result);
            }
            catch (Exception exception)
            {
                responseDetails.ResponseText = Helper.ProcessCatastrophicException(exception);
                responseDetails.httpStatusCode = HttpStatusCode.InternalServerError;
            }

            return responseDetails;
        }

        public async static Task<ResponseDetails> GetHostsV2Async(ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string partnerId, string externalApplicationId)
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
                    WebApiApplication.Log.Debug("GetHostsV2Async called");

                    // Get the source ip address (we have to do this before reading the payload)
                    callerIPAddress = Helper.GetClientIPAddressPortString();

                    // Was a valid applicationId provided?
                    if (externalApplicationId == null || externalApplicationId.Length == 0)
                    {
                        // Application id was not provided
                        response = new Response(Errors.MissingApplicationId, dataTypes.WantsDataType);
                        response.Result = ResultEnum.BadRequest;
                    }

                    if (response == null)
                    {                       
                        // Get hosts V2
                        response = await HostServices.GetHostsV2(signpostDataAccess, dataTypes, externalApplicationId);
                    }
                }
                catch (Exception exception)
                {
                    WebApiApplication.Log.Error("GetHostsV2Async", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
                }

                // Stream the result back
                responseDetails.ResponseText = response.ResponseText;
                responseDetails.httpStatusCode = Helper.GetHttpStatusCode(response.Result);
            }
            catch (Exception exception) 
            { 
                responseDetails.ResponseText = Helper.ProcessCatastrophicException(exception);
                responseDetails.httpStatusCode = HttpStatusCode.InternalServerError;
            }

            return responseDetails;
        }

        private async static Task<Response> GetHostsV1(ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string externalApplicationId)
        {
            Response response = null;

            List<SignpostDataAccessLayer.Models.HostV2> hostsV2 = null;
            signpostDataAccess.GetServicesByApplicationId(externalApplicationId, out hostsV2);

            // We're going to return the hosts in the old V1 format
            Hosts resultHosts = new Hosts();
            if (hostsV2 != null)
            {
                foreach (SignpostDataAccessLayer.Models.HostV2 hostV2 in hostsV2)
                {
                    Host host = new Host() 
                    { 
                        Id = hostV2.Id, 
                        Order = hostV2.Order, 
                        Url = hostV2.Url 
                    };

                    resultHosts.Add(host);
                }
            }

            response = new Response(SerializeHelper.Serialize<Hosts>(resultHosts, dataTypes.WantsDataType));

            return response;
        }

        private async static Task<Response> GetHostsV2(ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string externalApplicationId)
        {
            Response response = null;

            List<SignpostDataAccessLayer.Models.HostV2> hostsV2 = null;
            signpostDataAccess.GetServicesByApplicationId(externalApplicationId, out hostsV2);

            // We're going to return the hosts in the new V2 format
            HostsV2 resultHosts = new HostsV2();

            if (hostsV2 != null)
            {
                foreach (SignpostDataAccessLayer.Models.HostV2 hostV2 in hostsV2)
                {
                    HostV2 publicHostV2 = new HostV2()
                    {
                        Id = hostV2.Id,
                        Order = hostV2.Order,
                        Type = hostV2.  Name.ToString(),
                        Url = hostV2.Url,
                        Version = hostV2.Version
                    };

                    resultHosts.Add(publicHostV2);
                }
            }

            response = new Response(SerializeHelper.Serialize<HostsV2>(resultHosts, dataTypes.WantsDataType));

            return response;
        }
    }
}