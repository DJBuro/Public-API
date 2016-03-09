using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroCloudDataAccess.Domain;
using AndroCloudHelper;

namespace AndroCloudDataAccess.DataAccess
{
    public interface ISiteDetailsDataAccess
    {
        string ConnectionStringOverride { get; set; }
        string GetBySiteId(Guid siteId, DataTypeEnum dataType, out SiteDetails siteDetails);

        string GetBySiteId3(Guid siteId, DataTypeEnum dataType, out SiteDetails3 siteDetails);
    }
}
