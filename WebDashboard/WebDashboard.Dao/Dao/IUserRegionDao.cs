using WebDashboard.Dao.Domain;
using System.Collections.Generic;

namespace WebDashboard.Dao
{
    public interface IUserRegionDao : IGenericDao<UserRegion, int>
    {
        IList<UserRegion> FindByUserId(int userId);    
    }
}
