using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dashboard.Dao.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Dao
{
    public interface IRegionDao : IGenericDao<Region, int>
    {
        Region FindBySite(Site site);
    }
}
