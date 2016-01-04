using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;

namespace AndroCloudServices.Services
{
    public class MenuService
    {
        public static Response Get(
            string externalPartnerId,
            string externalSiteId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalPartnerId
            sourceId = externalPartnerId;

            // Was a valid partnerId provided?
            if (externalPartnerId == null || externalPartnerId.Length == 0)
            {
                // Security guid was not provided
                return new Response(Errors.MissingPartnerId, dataType);
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
            Response error = SecurityHelper.CheckMenuGetAccess(externalPartnerId, externalSiteId, dataAccessFactory, dataType, out siteId);
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

            return new Response(siteMenu.menuData);
        }

        public static Response Put(
            string externalSiteId,
            string licenseKey,
            string hardwareKey,
            string versionParameter,
            string data,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            // The source is the externalSiteId
            sourceId = externalSiteId;

            // Was a externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
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
            Response response = SecurityHelper.CheckMenuPostAccess(externalSiteId, licenseKey, dataAccessFactory, dataType, out siteId);
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
