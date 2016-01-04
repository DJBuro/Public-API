using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;
using System.Collections.Generic;

namespace AndroCloudServices.Services
{
    public class MenuService
    {
        public static Response Get(
            string externalApplicationId,
            string externalSiteId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Was a valid partnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was a externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            Guid partnerId = Guid.Empty;
            Guid siteId = Guid.Empty;
            Response error = SecurityHelper.CheckMenuGetAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out siteId);
            if (error != null)
            {
                // Security check failed
                return error;
            }

            // Get the menu
            SiteMenu siteMenu = null;
            dataAccessFactory.SiteMenuDataAccess.GetBySiteId(siteId, dataType, out siteMenu);

            // Was a menu returned?
            if (siteMenu == null)
            {
                return new Response(Errors.MenuNotFound, dataType);
            }

            return new Response(siteMenu.MenuData);
        }

        public static Response GetMenuImages(
            string externalApplicationId,
            string externalSiteId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalApplicationId
            sourceId = externalApplicationId;

            // Was a valid partnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // Application id was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was a externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            Guid partnerId = Guid.Empty;
            Guid siteId = Guid.Empty;
            Response error = SecurityHelper.CheckMenuGetAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out siteId);
            if (error != null)
            {
                // Security check failed
                return error;
            }

            // Get the menu images
            string menuImages = null;
            dataAccessFactory.SiteMenuDataAccess.GetMenuImagesBySiteId(siteId, dataType, out menuImages);

            // Success
            return new Response(menuImages == null ? "" : menuImages);
        }

        public static Response Post(
            string andromedaSiteIdParameter,
            string licenseKey,
            string hardwareKey,
            string versionParameter,
            string data,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalSiteId
            sourceId = andromedaSiteIdParameter;
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

            // Was a licenseKey provided?
            if (licenseKey == null || licenseKey.Length == 0)
            {
                // licenseKey was not provided
                return new Response(Errors.MissingLicenseKey, dataType);
            }

            // Was a hardwareKey provided?
            if (hardwareKey == null || hardwareKey.Length == 0)
            {
                // hardwareKey was not provided
                return new Response(Errors.MissingHardwareKey, dataType);
            }

            // Check version
            int version = 0;

            // Was a version provided?
            if (versionParameter == null || versionParameter.Length == 0)
            {
                // Version was not provided
                return new Response(Errors.MissingVersion, dataType);
            }
            else
            {
                // Is the version a valid int?
                if (!Int32.TryParse(versionParameter, out version))
                {
                    // Version is not a valid int
                    return new Response(Errors.VersionIsNotAValidInteger, dataType);
                }
            }

            // Check the details
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckMenuPostAccess(andromedaSiteId, licenseKey, dataAccessFactory, dataType, out siteId);
            if (response != null)
            {
                return response;
            }

            // Insert the menu into the datastore
            dataAccessFactory.SiteMenuDataAccess.Put(siteId, licenseKey, hardwareKey, data, version, dataType);

            // Serialize
            return new Response();
        }
    }
}
