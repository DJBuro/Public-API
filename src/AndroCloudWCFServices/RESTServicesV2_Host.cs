namespace AndroCloudWCFServices
{
    using System;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.ServiceModel.Web;
    using AndroCloudHelper;
    using AndroCloudWCFServices.Services;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    partial class RestServicesV2 : IRESTServicesV2
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
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/menu?applicationId={applicationId}&version={version}")]
        public Stream GetMenu(string siteId, string applicationId, string version)
        {
            try
            {
                // Pass through to v1
                var restServices = new RESTServices();
                return restServices.GetMenu(siteId, null, applicationId, version);
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
                responseText = AndroCloudWCFServices.Services.MenuService.GetMenuImages(Helper.GetDataTypes(), siteId, applicationId);
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