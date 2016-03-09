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
    public class SiteDetailsService
    {
        public static Response Get(
            string externalApplicationId,
            string externalSiteId,
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

            // Get site details
            SiteDetails siteDetails = null;
            dataAccessFactory.SiteDetailsDataAccess.GetBySiteId(siteId, dataType, out siteDetails);

            // Success
            return new Response(SerializeHelper.Serialize<SiteDetails>(siteDetails, dataType));
        }

        public static Response Get3(
            string externalApplicationId, 
            string externalSiteId,
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

            // Get site details
            SiteDetails3 siteDetails3 = null;
            dataAccessFactory.SiteDetailsDataAccess.GetBySiteId3(siteId, dataType, out siteDetails3);

            // Success
            return new Response(SerializeHelper.Serialize<SiteDetails3>(siteDetails3, dataType));
        }
    }
}
