using System;
using System.Linq;
using System.ServiceModel.Web;
using System.IO;
using AndroCloudHelper;

namespace AndroCloudWCFServices
{
    partial class RESTServicesV2_Host : IRESTServicesV2
    {
        [WebInvoke(Method = "PUT", UriTemplate = "sites/{siteId}/telemetrysession?applicationId={applicationId}")]
        public Stream PutTelemetrySession(Stream input, string siteId, string applicationId)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.Telemetry.PutTelemetrySession(Helper.GetDataTypes(), input, siteId, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "sites/{siteId}/telemetry?applicationId={applicationId}")]
        public Stream PutTelemetry(Stream input, string siteId, string applicationId)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.Telemetry.PutTelemetry(Helper.GetDataTypes(), input, siteId, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }
    }
}