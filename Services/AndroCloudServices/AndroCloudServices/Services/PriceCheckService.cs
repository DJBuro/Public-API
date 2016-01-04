using AndroCloudDataAccess.DataAccess;
using AndroCloudDataAccess.Domain;
using System;
using AndroCloudServices.Domain;
using AndroCloudServices.Helper;
using AndroCloudDataAccess;
using AndroCloudHelper;

namespace AndroCloudServices.Services
{
    public class PriceCheckService
    {
        public static Response Put(
            string externalApplicationId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = externalApplicationId;

            // Was a externalPartnerId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            int applicationId = -1;
            int androSiteId = -1;

            Response response = SecurityHelper.CheckOrderPostAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out applicationId, out androSiteId);

            if (response != null)
            {
                return response;
            }

// TODO signalr implementation goes here

            // Serialize
            return new Response();
        }
    }
}
