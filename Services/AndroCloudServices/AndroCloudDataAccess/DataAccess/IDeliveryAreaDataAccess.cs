namespace AndroCloudDataAccess.DataAccess
{
    using System;
    using System.Collections.Generic;

    public interface IDeliveryAreaDataAccess
    {
        string ConnectionStringOverride { get; set; }

        IEnumerable<string> GetBySiteId(Guid siteId);
    }
}