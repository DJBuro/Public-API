using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface IRegionDao : IGenericDao<Region, int>
    {
        IList<Region> FindHeadOffice(HeadOffice headOffice, bool removeDisabledSites);
    }
}

