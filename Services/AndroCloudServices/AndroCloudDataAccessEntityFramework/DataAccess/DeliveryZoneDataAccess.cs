namespace AndroCloudDataAccessEntityFramework.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AndroCloudDataAccess.DataAccess;

    using AndroCloudDataAccessEntityFramework.Model;

    internal sealed class DeliveryZoneDataAccess : IDeliveryAreaDataAccess
    {
        public string ConnectionStringOverride { get; set; }

        public IEnumerable<string> GetBySiteId(Guid siteId)
        {
            using (var acsEntities = new ACSEntities())
            {
                DataAccessHelper.FixConnectionString(acsEntities, ConnectionStringOverride);

                return acsEntities.DeliveryAreas.Where(da => da.SiteId == siteId).Select(da => da.DeliveryArea1).ToList();
            }
        }
    }
}