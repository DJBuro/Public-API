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
    public class SiteService
    {
        public static Response Get(
            string externalPartnerId, 
            string externalChainId,
            string maxDistanceParameter,
            string longitudeParameter,
            string latitudeParameter,
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

            // Check maxDistance
            float? maxDistance = null;
            // Was an optional maxDistance provided?
            if (maxDistanceParameter != null && maxDistanceParameter.Length > 0)
            {
                // Was an optional longitude provided?
                if (longitudeParameter == null || longitudeParameter.Length == 0)
                {
                    // If a maxDistance is provided then a max distance must also be provided
                    return new Response(Errors.MissingLongitude, dataType);
                }
                // Was an optional latitude provided?
                else if (latitudeParameter == null || latitudeParameter.Length == 0)
                {
                    // If a maxDistance is provided then a max distance must also be provided
                    return new Response(Errors.MissingLatitude, dataType);
                }
            }

            // Check longitude
            float? longitude = null;
            // Was an optional longitude provided?
            if (longitudeParameter != null && longitudeParameter.Length > 0)
            {
                // Was an optional maxDistance provided?
                if (maxDistance == null)
                {
                    // If a longitude is provided then a max distance must also be provided
                    return new Response(Errors.MissingMaxDistance, dataType);
                }
            }

            // Check latitude
            float? latitude = null;
            // Was an optional latitude provided?
            if (latitudeParameter != null && latitudeParameter.Length > 0)
            {
                // Was an optional maxDistance provided?
                if (maxDistance == null)
                {
                    // If a latitude is provided then a max distance must also be provided
                    return new Response(Errors.MissingMaxDistance, dataType);
                }
            }

            // Check the partners details
            Guid partnerId = Guid.Empty;
            Guid? groupId = Guid.Empty;
            Response response = SecurityHelper.CheckSitesGetAccess(externalPartnerId, externalChainId, dataAccessFactory, dataType, out partnerId, out groupId);

            if (response != null)
            {
                return response;
            }

            // Get sites
            List<Site> sites = null;
            dataAccessFactory.SiteDataAccess.GetByFilter(partnerId, groupId, maxDistance, longitude, latitude, dataType, out sites);

            // Success
            Sites serializableSites = new Sites();
            serializableSites.AddRange(sites);
            return new Response(SerializeHelper.Serialize<Sites>(serializableSites, dataType));
        }
    }
}
