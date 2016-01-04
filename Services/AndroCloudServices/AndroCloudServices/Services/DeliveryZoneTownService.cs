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
    public class DeliveryZoneTownService
    {
        public static Response Get(
            string externalApplicationId,
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

            // Check the application details
            int? applicationId = null;
            Guid siteId = Guid.Empty;
            Response response = SecurityHelper.CheckSitesGetAccess(externalApplicationId, dataAccessFactory, dataType, out applicationId);

            if (response != null)
            {
                return response;
            }

            // Get delivery area towns
            List<DeliveryZoneTown> deliveryZoneTowns = null;
            dataAccessFactory.DeliveryAreaTownDataAccess.GetByApplicationId(applicationId.Value, out deliveryZoneTowns);

            // Wrap the results in something that serializes to nice XML
            DeliveryZoneTowns serializableDeliveryZoneTowns = new DeliveryZoneTowns(deliveryZoneTowns);
            
            // Success
            return new Response(SerializeHelper.Serialize<DeliveryZoneTowns>(serializableDeliveryZoneTowns, dataType));
        }
    }
}
