using System;
using System.Linq;
using System.ServiceModel.Web;
using System.IO;
using AndroCloudHelper;

namespace AndroCloudWCFServices
{
    partial class RESTServicesV2_Host : IRESTServicesV2
    {
        /// <summary>
        /// Gets a list of sites
        /// </summary>
        /// <param name="groupIdFilter"></param>
        /// <param name="maxDistanceFilter"></param>
        /// <param name="longitudeFilter"></param>
        /// <param name="latitudeFilter"></param>
        /// <param name="applicationId"></param>
        /// <returns>A list of sites</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites?groupId={groupIdFilter}&maxDistance={maxDistanceFilter}&longitude={longitudeFilter}&latitude={latitudeFilter}&deliveryZone={deliveryZoneFilter}&applicationId={applicationId}")]
        public Stream GetSites(string groupIdFilter, string maxDistanceFilter, string longitudeFilter, string latitudeFilter, string deliveryZoneFilter, string applicationId)
        {
            try
            {
                string responseText = AndroCloudWCFServices.Services.Site.GetSites(Helper.GetDataTypes(), null, maxDistanceFilter, longitudeFilter, latitudeFilter, deliveryZoneFilter, applicationId);

                // Convert the response text to a binary stream
                return Helper.StringToStream(responseText);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Gets full details of a site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <param name="gotMenuVersion"></param>
        /// <returns>Site details</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}?applicationId={applicationId}&gotMenuVersion={gotMenuVersion}&statusCheck={statusCheck}")]
        public Stream GetSite(string siteId, string applicationId, int gotMenuVersion, string statusCheck)
        {
            try
            {
                // New v2 method
                bool? statusCheckBool = statusCheck == null ? (bool?)null : statusCheck.ToUpper() == "TRUE" ? true : false;
                string responseText = AndroCloudWCFServices.Services.Site.GetSite(Helper.GetDataTypes(), siteId, applicationId, gotMenuVersion, statusCheckBool);

                // Convert the response text to a binary stream
                return Helper.StringToStream(responseText);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Gets details of a site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <returns>Site details</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/details?applicationId={applicationId}")]
        public Stream GetSiteDetails(string siteId, string applicationId)
        {
            try
            {
                // Pass through to v1
                RESTServices restServices = new RESTServices();
                return restServices.GetSiteDetails(siteId, null, applicationId);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }

        /// <summary>
        /// Gets a list of delivery zones
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <returns>A list of delivery zones</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites/{siteId}/deliveryzones?applicationId={applicationId}")]
        public Stream GetDeliveryZones(string siteId, string applicationId)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.DeliveryZone.GetDeliveryZones(Helper.GetDataTypes(), siteId, applicationId);
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
        /// Gets a list of towns that the site delivers to
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>A list of delivery zones</returns>
        [WebInvoke(Method = "GET", UriTemplate = "deliverytowns?applicationId={applicationId}")]
        public Stream GetDeliveryTowns(string applicationId)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.DeliveryTown.GetDeliveryTowns(Helper.GetDataTypes(), applicationId);
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
        /// Gets a list of roads that the site delivers to
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns>A list of delivery zones</returns>
        [WebInvoke(Method = "GET", UriTemplate = "deliveryroads/{postcode}?applicationId={applicationId}")]
        public Stream GetDeliveryRoads(string applicationId, string postcode)
        {
            string responseText = "";

            try
            {
                // New v2 method
                responseText = AndroCloudWCFServices.Services.DeliveryRoad.GetDeliveryRoads(Helper.GetDataTypes(), postcode, applicationId);
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
        /// Gets full details of a site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="applicationId"></param>
        /// <param name="gotMenuVersion"></param>
        /// <returns>Site details</returns>
        [WebInvoke(Method = "GET", UriTemplate = "sites3/{siteId}?applicationId={applicationId}&gotMenuVersion={gotMenuVersion}&statusCheck={statusCheck}")]
        public Stream GetSite3(string siteId, string applicationId, int gotMenuVersion, string statusCheck)
        {
            try
            {
                // New v2 method
                bool? statusCheckBool = statusCheck == null ? (bool?)null : statusCheck.ToUpper() == "TRUE" ? true : false;
                string responseText = AndroCloudWCFServices.Services.Site.GetSite3(Helper.GetDataTypes(), siteId, applicationId, gotMenuVersion, statusCheckBool);

                // Convert the response text to a binary stream
                return Helper.StringToStream(responseText);
            }
            catch (Exception exception)
            {
                Global.Log.Error("Unhandled exception", exception);
                return Helper.StringToStream(Helper.ProcessCatastrophicException(exception));
            }
        }
    }
}