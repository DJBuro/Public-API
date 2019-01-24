namespace AndroCloudServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using AndroCloudDataAccess;

    using AndroCloudHelper;

    using AndroCloudServices.Helper;

    public class DeliveryZoneService
    {
        public static Response Get(
            string externalApplicationId,
            string externalSiteId,
            DataTypeEnum dataType,
            IDataAccessFactory dataAccessFactory)
        {
            if (string.IsNullOrEmpty(externalApplicationId))
            {
                return new Response(Errors.MissingApplicationId, dataType);
            }

            if (string.IsNullOrEmpty(externalSiteId))
            {
                return new Response(Errors.MissingSiteId, dataType);
            }

            int? appId = null;
            Response response = SecurityHelper.CheckSiteDetailsGetAccess(externalApplicationId, externalSiteId, dataAccessFactory, dataType, out appId, out Guid siteId);

            if (response != null)
            {
                return response;
            }

            IEnumerable<string> deliveryZones = dataAccessFactory.DeliveryZoneDataAccess.GetBySiteId(siteId);

            return new Response(BuildDeliveryZonesList(dataType, deliveryZones));
        }

        internal static string BuildDeliveryZonesList(DataTypeEnum dataType, IEnumerable<string> deliveryZones)
        {
            // Need to do this the old fashion way cos the serializer doesn't work well with lists and I don't have time to figure it out...
            var stringBuilder = new StringBuilder();

            if (dataType == DataTypeEnum.JSON)
            {
                var addComma = false;
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