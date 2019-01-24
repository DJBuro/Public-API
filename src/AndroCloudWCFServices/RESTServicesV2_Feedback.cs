using System;
using System.Linq;
using System.ServiceModel.Web;
using System.IO;
using AndroCloudHelper;

namespace AndroCloudWCFServices
{
    partial class RestServicesV2 : IRESTServicesV2
    {
        [WebInvoke(Method = "PUT", UriTemplate = "feedback?applicationId={applicationId}")]
        public Stream PutFeedback(Stream input, string applicationId)
        {
            return this.SitePutFeedback(input, "", applicationId);
        }

        [WebInvoke(Method = "PUT", UriTemplate = "sites/{siteId}/feedback?applicationId={applicationId}")]
        public Stream SitePutFeedback(Stream input, string siteId, string applicationId)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.Feedback.PutFeedback(Helper.GetDataTypes(), input, siteId, applicationId);
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