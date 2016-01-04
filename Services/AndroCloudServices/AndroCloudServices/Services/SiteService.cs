using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System.Xml.Serialization;
using System.IO;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;

namespace AndroCloudServices.Services
{
    public class SiteService
    {
        /// <summary>
        /// Get a list of sites matching the criteria
        /// </summary>
        /// <param name="externalApplicationId"></param>
        /// <param name="maxDistanceFilter"></param>
        /// <param name="longitudeFilter"></param>
        /// <param name="latitudeFilter"></param>
        /// <param name="deliveryZoneFilter"></param>
        /// <param name="dataType"></param>
        /// <param name="dataAccessFactory"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public static Response Get(
            string externalApplicationId, 
            string maxDistanceFilter,
            string longitudeFilter,
            string latitudeFilter,
            string deliveryZoneFilter,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = "";

            // Was a applicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Check maxDistance
            float? maxDistance = null;
            // Was an optional maxDistance provided?
            if (maxDistanceFilter != null && maxDistanceFilter.Length > 0)
            {
                // Was an optional longitude provided?
                if (longitudeFilter == null || longitudeFilter.Length == 0)
                {
                    // If a maxDistance is provided then a max distance must also be provided
                    return new Response(Errors.MissingLongitude, dataType);
                }
                // Was an optional latitude provided?
                else if (latitudeFilter == null || latitudeFilter.Length == 0)
                {
                    // If a maxDistance is provided then a max distance must also be provided
                    return new Response(Errors.MissingLatitude, dataType);
                }

                // Check maxDistance is a valid float
                float maxDistanceTemp = 0;
                if (float.TryParse(maxDistanceFilter, out maxDistanceTemp))
                {
                    maxDistance = maxDistanceTemp;
                }
                else
                {
                    return new Response(Errors.InvalidMaxDistance, dataType);
                }
            }

            // Check longitude
            float? longitude = null;
            // Was an optional longitude provided?
            if (longitudeFilter != null && longitudeFilter.Length > 0)
            {
                // Was an optional maxDistance provided?
                if (maxDistance == null)
                {
                    // If a longitude is provided then a max distance must also be provided
                    return new Response(Errors.MissingMaxDistance, dataType);
                }

                // Check longitude is a valid float
                float longitudeTemp = 0;
                if (float.TryParse(longitudeFilter, out longitudeTemp))
                {
                    longitude = longitudeTemp;
                }
                else
                {
                    return new Response(Errors.InvalidLongitude, dataType);
                }
            }

            // Check latitude
            float? latitude = null;
            // Was an optional latitude provided?
            if (latitudeFilter != null && latitudeFilter.Length > 0)
            {
                // Was an optional maxDistance provided?
                if (maxDistance == null)
                {
                    // If a latitude is provided then a max distance must also be provided
                    return new Response(Errors.MissingMaxDistance, dataType);
                }

                // Check latitude is a valid float
                float latitudeTemp = 0;
                if (float.TryParse(latitudeFilter, out latitudeTemp))
                {
                    latitude = latitudeTemp;
                }
                else
                {
                    return new Response(Errors.InvalidLatitude, dataType);
                }
            }

            // Check the application details
            int? applicationId = null;
            Response response = SecurityHelper.CheckSitesGetAccess(externalApplicationId, dataAccessFactory, dataType, out applicationId);

            if (response != null)
            {
                return response;
            }

            // Get sites
            List<AndroCloudDataAccess.Domain.Site> sites = null;
            dataAccessFactory.SiteDataAccess.GetByFilter(applicationId.Value, maxDistance, longitude, latitude, deliveryZoneFilter, dataType, out sites);

            // Success
            Sites serializableSites = new Sites();
            serializableSites.AddRange(sites);
            return new Response(SerializeHelper.Serialize<Sites>(serializableSites, dataType));
        }

        /// <summary>
        /// Gets the full details of the specified site
        /// </summary>
        /// <param name="externalApplicationId"></param>
        /// <param name="externalSiteId"></param>
        /// <param name="dataType"></param>
        /// <param name="dataAccessFactory"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public static Response Get(
            string externalApplicationId,
            string externalSiteId,
            int notVersion,
            bool? statusCheck,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = "";

            // Was a applicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Check site id
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // External site id was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the application details
            int? applicationId = null;
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out applicationId, out siteId);

            if (response != null)
            {
                return response;
            }

            AndroCloudServices.Domain.Site site = null;

            // Is this a simple status check?
            if (statusCheck.HasValue && statusCheck.Value)
            {
                // Just get the site - we don't need to return the menu or anything other than the status
                AndroCloudDataAccess.Domain.Site siteObject = null;
                dataAccessFactory.SiteDataAccess.GetById(siteId, out siteObject);

                site = new AndroCloudServices.Domain.Site()
                {
                    Details = new SiteDetails() { IsOpen = siteObject.IsOpen },
                    Menu = null,
                    DeliveryZones = null
                };
            }
            else
            {
                // Get site details
                SiteDetails siteDetails = null;
                dataAccessFactory.SiteDetailsDataAccess.GetBySiteId(siteId, dataType, out siteDetails);

                // Get the menu
                SiteMenu siteMenu = null;
                dataAccessFactory.SiteMenuDataAccess.GetMenuAndImagesBySiteIdAndNotVersion(siteId, dataType, notVersion, out siteMenu);

                // Was a menu returned?
                if (siteMenu == null)
                {
                    return new Response(Errors.MenuNotFound, dataType);
                }

                // Get delivery zones
                List<string> deliveryZones = null;
                dataAccessFactory.DeliveryZoneDataAccess.GetBySiteId(siteId, out deliveryZones);

                // Return the full site details
                site = new AndroCloudServices.Domain.Site()
                {
                    Details = siteDetails,
                    Menu = siteMenu,
                    DeliveryZones = DeliveryZoneService.BuildDeliveryZonesList(dataType, deliveryZones)
                };
            }

            // Success
            return new Response(SerializeHelper.Serialize<AndroCloudServices.Domain.Site>(site, dataType));
        }

        public static Response Post(
            string siteData,
            string andromedaSiteIdParameter,
            string licenseKey,
            string hardwareKey,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out SiteUpdate siteUpdate)
        {
            siteUpdate = null;

            int andromedaSiteId = 0;

            // Was an andromedaSiteId provided?
            if (andromedaSiteIdParameter == null || andromedaSiteIdParameter.Length == 0)
            {
                // andromedaSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Is andromedaSiteId an integer?
            if (!int.TryParse(andromedaSiteIdParameter, out andromedaSiteId))
            {
                // andromedaSiteId is not an integer
                return new Response(Errors.InvalidSiteId, dataType);
            }

            // Was a site provided?
            if (siteData == null || siteData.Length == 0)
            {
                // Site was not provided
                return new Response(Errors.MissingSite, dataType);
            }

            // Was an licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was an hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Extract the new ETD
            // Deserialize the site
            
            string errorMessage = SerializeHelper.Deserialize<SiteUpdate>(siteData, dataType, out siteUpdate);
            if (errorMessage.Length > 0)
            {
                // There was a problem deserializing
                Response response = new Response(Errors.BadData, dataType);
                response.ResponseText = response.ResponseText.Replace("{errorMessage}", errorMessage);
                return response;
            }

            // Was a new etd provided?
            if (siteUpdate == null)
            {
                // ETD was not provided
                return new Response(Errors.MissingETD, dataType);
            }


            // Check the andromedaSiteId is valid
            AndroCloudDataAccess.Domain.Site site = null;
            dataAccessFactory.SiteDataAccess.GetByAndromedaSiteIdAndLive(andromedaSiteId, out site);

            if (site == null)
            {
                return new Response(Errors.UnknownSiteId, dataType);
            }

            // Was the license key provided the correct license key for the site?
            if (site.LicenceKey != licenseKey)
            {
                return new Response(Errors.UnknownLicenseKey, dataType);
            }

            // lets not pester myandromeda if nothing has changed! This would ideally be done in Rameses but its 'fixed' already and I'm writing this here as well. 
            bool etdChanged = siteUpdate.ETD != site.EstDelivTime;
            // Change the site details in the database
            dataAccessFactory.SiteDataAccess.Update(andromedaSiteId, siteUpdate.ETD);

            if (!etdChanged) 
            {
                siteUpdate = null;
            }

            // Serialize
            return new Response();
        }
    }
}
