namespace AndroCloudDataAccess.DataAccess
{
    using System;

    using AndroCloudDataAccess.Domain;

    using AndroCloudHelper;

    public interface ISiteMenuDataAccess
    {
        string ConnectionStringOverride { get; set; }

        string Put(Guid siteId, string licenseKey, string hardwareKey, string data, int version, DataTypeEnum dataType);

        string GetBySiteId(Guid siteId, DataTypeEnum dataType, out SiteMenu siteMenu);

        string GetMenuAndImagesBySiteIdAndNotVersion(Guid siteId, DataTypeEnum dataType, int notVersion, out AndroCloudDataAccess.Domain.SiteMenu menu);

        string GetMenuImagesBySiteId(Guid siteId, DataTypeEnum dataType, out string siteMenuImages);

        string UpdateThumbnailData(Guid siteId, string data, DataTypeEnum dataType);
    }
}
