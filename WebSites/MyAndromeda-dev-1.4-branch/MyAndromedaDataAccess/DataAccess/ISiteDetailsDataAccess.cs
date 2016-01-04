using System;
using System.Linq;
using MyAndromedaDataAccess.Domain;
using MyAndromeda.Core;

namespace MyAndromedaDataAccess.DataAccess
{
    public interface ISiteDetailsDataAccess : IDependency
    {
        string GetBySiteId(int siteId, out SiteDetails siteDetails);
        string Update(int siteId, SiteDetails siteDetails);
    }
}
