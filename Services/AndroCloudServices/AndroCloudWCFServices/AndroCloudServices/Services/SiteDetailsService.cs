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
            string externalPartnerId, 
            string externalSiteId,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = "";

            // Was a partnerId provided?
            if (externalPartnerId == null || externalPartnerId.Length == 0)
            {
                // Security guid was not provided
                return new Response(Errors.MissingPartnerId, dataType);
            }

            // The source is the externalPartnerId
            sourceId = externalPartnerId;

            // Check site id
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // Security guid was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            Guid partnerId = Guid.Empty;
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalPartnerId, externalSiteId, dataAccessFactory, dataType, out partnerId, out siteId);

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
    }
}
