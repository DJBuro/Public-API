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

namespace PrivateSignpost.Services
{
    public class HostServices
    {
        public async static Task<ResponseDetails> GetHostsV1Async(
            ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string andromedaSiteIdParameter, string licenseKey, string hardwareKey)
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
                    callerIPAddress = AndroCloudHelper.Helper.GetClientIPAddressPortString();

                    // Was an andromedaSiteId provided?
                    if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
                    {
                        // andromedaSiteId was not provided
                        response = new Response(Errors.MissingSiteId, dataTypes.WantsDataType);
                        response.Result = ResultEnum.BadRequest;
                    }

                    // Is andromedaSiteId an integer?
                    int andromedaSiteId = 0;
                    if (response == null)
                    {
                        if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
                        {
                            // andromedaSiteId is not an integer
                            response = new Response(Errors.InvalidSiteId, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    // Was a licenseKey provided?
                    if (response == null)
                    {
                        if (licenseKey == null || licenseKey.Length == 0)
                        {
                            // licenseKey was not provided
                            response = new Response(Errors.MissingLicenseKey, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    // Was a hardwareKey provided?
                    if (response == null)
                    {
                        if (hardwareKey == null || hardwareKey.Length == 0)
                        {
                            // hardwareKey was not provided
                            response = new Response(Errors.MissingHardwareKey, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    if (response == null)
                    {
                        // Get hosts V1
                        response = await HostServices.GetHostsV1(signpostDataAccess, dataTypes, andromedaSiteId, licenseKey, hardwareKey);
                    }
                }
                catch (Exception exception)
                {
                    WebApiApplication.Log.Error("GetHostsV1Async", exception);

                    // Unhandled error
                    response = new Response(Errors.InternalError, dataTypes.WantsDataType);
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

        public async static Task<ResponseDetails> GetHostsV2Async(
            ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, string andromedaSiteIdParameter, string licenseKey, string hardwareKey)
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
                    callerIPAddress = AndroCloudHelper.Helper.GetClientIPAddressPortString();

                    // Was an andromedaSiteId provided?
                    if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
                    {
                        // andromedaSiteId was not provided
                        response = new Response(Errors.MissingSiteId, dataTypes.WantsDataType);
                        response.Result = ResultEnum.BadRequest;
                    }

                    // Is andromedaSiteId an integer?
                    int andromedaSiteId = 0;
                    if (response == null)
                    {
                        if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
                        {
                            // andromedaSiteId is not an integer
                            response = new Response(Errors.InvalidSiteId, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    // Was a licenseKey provided?
                    if (response == null)
                    {
                        if (licenseKey == null || licenseKey.Length == 0)
                        {
                            // licenseKey was not provided
                            response = new Response(Errors.MissingLicenseKey, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    // Was a hardwareKey provided?
                    if (response == null)
                    {
                        if (hardwareKey == null || hardwareKey.Length == 0)
                        {
                            // hardwareKey was not provided
                            response = new Response(Errors.MissingHardwareKey, dataTypes.WantsDataType);
                            response.Result = ResultEnum.BadRequest;
                        }
                    }

                    if (response == null)
                    {
                        // Get hosts V2
                        response = await HostServices.GetHostsV2(signpostDataAccess, dataTypes, andromedaSiteId, licenseKey, hardwareKey);
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
                responseDetails.httpStatusCode = AndroCloudHelper.Helper.GetHttpStatusCode(response.Result);
            }
            catch (Exception exception)
            {
                responseDetails.ResponseText = AndroCloudHelper.Helper.ProcessCatastrophicException(exception);
                responseDetails.httpStatusCode = HttpStatusCode.InternalServerError;
            }

            return responseDetails;
        }

        private async static Task<Response> GetHostsV1(
            ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, int andromedaSiteId, string licenseKey, string hardwareKey)
        {
            Response response = null;

            List<SignpostDataAccessLayer.Models.HostV1> hostsV1 = null;
            signpostDataAccess.GetLegacyPrivateHostsBySiteId(andromedaSiteId, out hostsV1);

            // We're going to return the hosts in the old V1 format
            Hosts resultHosts = new Hosts();
            if (hostsV1 != null)
            {
                // Get the private web ordering servers
                foreach (SignpostDataAccessLayer.Models.HostV1 hostV1 in hostsV1)
                {
                    Host host = new Host()
                    {
                        Order = hostV1.Order,
                        SignalRUrl = hostV1.SignalRUrl,
                        Url = hostV1.Url
                    };

                    resultHosts.Add(host);
                }
            }

            response = new Response(SerializeHelper.Serialize<Hosts>(resultHosts, dataTypes.WantsDataType));

            return response;
        }

        private async static Task<Response> GetHostsV2(
            ISignpostDataAccess signpostDataAccess, DataTypes dataTypes, int andromedaSiteId, string licenseKey, string hardwareKey)
        {
            Response response = null;

            List<SignpostDataAccessLayer.Models.HostV2> hostsV2 = null;
            signpostDataAccess.GetServicesBySiteId(andromedaSiteId, out hostsV2);

            // We're going to return the hosts in the old V1 format
            HostsV2 resultHosts = new HostsV2();
            if (hostsV2 != null)
            {
                // Get the private web ordering servers
                foreach (SignpostDataAccessLayer.Models.HostV2 hostV2 in hostsV2)
                {
                    HostV2 publicHostV2 = new HostV2()
                    {
                        Id = hostV2.Id,
                        Order = hostV2.Order,
                        Type = hostV2.Name.ToString(),
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