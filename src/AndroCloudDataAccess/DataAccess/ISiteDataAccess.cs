namespace AndroCloudDataAccess.DataAccess
{
    using System;
    using System.Collections.Generic;
    using AndroCloudDataAccess.Domain;
    using AndroCloudHelper;
    using AndroCloudServices.Models;

    public interface ISiteDataAccess
    {
        string ConnectionStringOverride { get; set; }

        string GetByFilter(
            int applicationId,
            float? maxDistance,
            double? longitude,
            double? latitude,
            string deliveryZoneFilter,
            DataTypeEnum dataType,
            out List<Site> sites);

        string GetById(Guid siteId, out AndroCloudDataAccess.Domain.Site site);

        string GetByExternalSiteId(string externalSiteId, out Site site);

        string GetByAndromedaSiteIdAndLive(int andromedaSiteId, out AndroCloudDataAccess.Domain.Site site);

        string GetByAndromedaSiteId(int andromedaSiteId, out AndroCloudDataAccess.Domain.Site site);

        string GetByIdAndApplication(int applicationId, Guid siteId, out Site site);

        string Update(int andromedaSiteId, int etd);

        IEnumerable<AcsSiteDetails> GetSitesWithDetailsByApplicationId(string applicationId);
    }
}
