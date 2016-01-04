using System;
using AndroCloudServices.Helper;
using AndroCloudHelper;
using DataWarehouseDataAccess.Domain;
using System.Linq;

namespace AndroCloudServices.Services
{
    public class FeedbackService
    {
        public static Response Put
        (
            string externalApplicationId,
            string externalSiteId,
            string data,
            DataTypeEnum dataType,
            AndroCloudDataAccess.IDataAccessFactory androCloudDataAccessFactory,
            DataWarehouseDataAccess.IDataAccessFactory dataWarehouseDataAccessFactory
        )
        {
            // Was a externalApplicationId provided?
            if (externalApplicationId == null || externalApplicationId.Length == 0)
            {
                // externalApplicationId was not provided
                return new Response(Errors.MissingApplicationId, dataType);
            }

            // Deserialize the XML/JSON into an object
            DataWarehouseDataAccess.Domain.Feedback feedback = null;
            string result = SerializeHelper.Deserialize<Feedback>(data, dataType, out feedback);

            if (result != null && result.Length > 0)
            {
                return new Response(Errors.InvalidCustomer, dataType);
            }

            // Check the application details
            int? applicationId = null;
            Guid siteId;

            if (externalSiteId.Length > 0)
            {
                Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalApplicationId, externalSiteId, androCloudDataAccessFactory, dataType, out applicationId, out siteId);

                if (response != null)
                {
                    return response;
                }
            }

            //no site selected, but still coming from the website. 
            if (!applicationId.HasValue) 
            {
                Response response = SecurityHelper.CheckSitesGetAccess(externalApplicationId, androCloudDataAccessFactory, dataType, out applicationId);

                if (response != null) 
                {
                    return response;
                }
            }

            // Put the feedback
            dataWarehouseDataAccessFactory.FeedbackDataAccess.AddFeedback(applicationId.Value, feedback, externalSiteId);

            return new Response();
        }
    }
}
