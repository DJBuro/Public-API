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
    public class DeliveryZoneService
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

            // Get delivery zones
            List<string> deliveryZones = null;
            dataAccessFactory.DeliveryZoneDataAccess.GetBySiteId(siteId, out deliveryZones);

            // Success
            return new Response(DeliveryZoneService.BuildDeliveryZonesList(dataType, deliveryZones));
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
