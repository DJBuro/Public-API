using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.ServiceModel.Activation;
using AndroCloudWCFServices.Services;
using AndroCloudHelper;

namespace AndroCloudWCFServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    partial class RESTServicesV2_Host : IRESTServicesV2
    {
        /// <summary>
        /// Gets a list of Hosts
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>A list of hosts</returns>
        [WebInvoke(Method = "GET", UriTemplate = "host?applicationId={applicationId}")]
        public Stream GetHost(string applicationId)
        {
            string responseText = "";

            try
            {
                responseText = Host.GetHostsV2(Helper.GetDataTypes(), applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                responseText = Helper.ProcessCatastrophicException(exception);
            }

            // Convert the response text to a binary stream
            return Helper.StringToStream(responseText);
        }

        /// <summary>
        /// Gets a Menu
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <returns>A menu (XML or JSON)</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/menu?applicationId={applicationId}")]
        public Stream GetMenu(string siteId, string applicationId)
        {
            try
            {
                // Pass through to v1
                RESTServices restServices = new RESTServices();
                return restServices.GetMenu(siteId, null, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Gets a list of menu item images
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <returns>A menu (XML or JSON)</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/menu/images?applicationId={applicationId}")]
        public Stream GetMenuImages(string siteId, string applicationId)
        {
            string responseText = "";

            try
            {
                responseText = AndroCloudWCFServices.Services.Menu.GetMenuImages(Helper.GetDataTypes(), siteId, applicationId);
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