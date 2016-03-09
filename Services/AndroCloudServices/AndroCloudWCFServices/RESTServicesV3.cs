using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Net;
using AndroCloudServices;
using System.ServiceModel.Activation;
using AndroCloudWCFServices.Services;
using System.Text;
using System.Configuration;
using AndroCloudHelper;
using System.Threading.Tasks;

namespace AndroCloudWCFServices
{
    partial class RESTServicesV2_Host : IRESTServicesV2, IRESTServicesV3
    {
        /// <summary>
        /// Gets details of a site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <returns>Site details</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/details3?applicationId={applicationId}")]
        public Stream GetSiteDetails3(string siteId, string applicationId)
        {
            try
            {
                // Pass through to v1
                RESTServices restServices = new RESTServices();
                return restServices.GetSiteDetails2(siteId, null, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }
    }
}