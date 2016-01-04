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
    public class DeliveryZoneRoadService
    {
        public static Response Get(
            string externalApplicationId,
            string postcode,
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

            // Get delivery area roads
            List<DeliveryZoneRoad> deliveryZoneRoads = null;
            dataAccessFactory.DeliveryAreaRoadDataAccess.GetByApplicationIdPostcode(applicationId.Value, postcode, out deliveryZoneRoads);

            // Wrap the results in something that serializes to nice XML
            DeliveryZoneRoads serializableDeliveryZoneRoads = new DeliveryZoneRoads(deliveryZoneRoads);

            // Success
            return new Response(SerializeHelper.Serialize<DeliveryZoneRoads>(serializableDeliveryZoneRoads, dataType));
        }

        public static string BuildDeliveryZonesList(DataTypeEnum dataType, List<string> deliveryZones)
        {
            // Need to do this the old fashion way cos the serializer doesn't work well with lists and I don't have time to figure it out...
            StringBuilder stringBuilder = new StringBuilder("");
            if (dataType == DataTypeEnum.JSON)
            {
                bool addComma = false;
                stringBuilder.Append("[");
                foreach (string deliveryZone in deliveryZones)
                {
                    if (addComma)
                    {
                        stringBuilder.Append(",");
                    }
                    else
                    {
                        addComma = true;
                    }

                    stringBuilder.Append("\"");
                    stringBuilder.Append(deliveryZone);
                    stringBuilder.Append("\"");
                }
                stringBuilder.Append("]");
            }
            else
            {
                stringBuilder.Append("<DeliveryZones>");
                foreach (string deliveryZone in deliveryZones)
                {
                    stringBuilder.Append("<Zone>");
                    stringBuilder.Append(deliveryZone);
                    stringBuilder.Append("</Zone>");
                }
                stringBuilder.Append("</DeliveryZones>");
            }

            return stringBuilder.ToString();
        }
    }
}
