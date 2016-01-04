using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dashboard.Dao.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Dao
{
    public interface IDashboardDataDao : IGenericDao<DashboardData, int>
    {
        DashboardData FindBySiteId(int? id);
        IList<DashboardData> FindAll(Site site);
        IList<DashboardData> FindAll(Region region);
        IList<DashboardData> FindAll(HeadOffice headOffice);
    }
}
