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
            string externalPartnerId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType, 
            IDataAccessFactory dataAccessFactory,
            out string sourceId)
        {
            sourceId = externalPartnerId;

            // Was a externalPartnerId provided?
            if (externalPartnerId == null || externalPartnerId.Length == 0)
            {
                // externalPartnerId was not provided
                return new Response(Errors.MissingPartnerId, dataType);
            }

            // Was an externalSiteId provided?
            if (externalSiteId == null || externalSiteId.Length == 0)
            {
                // externalSiteId was not provided
                return new Response(Errors.MissingSiteId, dataType);
            }

            // Check the partners details
            Guid orderId = Guid.Empty;
            Response response = SecurityHelper.CheckOrderPostAccess(externalPartnerId, externalSiteId, dataAccessFactory, dataType, out orderId);

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
