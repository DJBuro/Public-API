using System;
using System.Linq;
using MyAndromeda.Core;
using Domain = MyAndromeda.Data.Domain;

namespace MyAndromeda.Data.DataAccess
{
    public interface ISiteDetailsDataAccess : IDependency
    {
        string GetBySiteId(int siteId, out Domain.SiteDetails siteDetails);
        string Update(int siteId, Domain.SiteDetails siteDetails);
    }
}
