using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess
{
    public interface ISiteDetailsDataAccess : IDependency
    {
        string GetBySiteId(int siteId, out SiteDetailsDomainModel siteDetails);
        string Update(int siteId, SiteDetailsDomainModel siteDetails);
    }
}
