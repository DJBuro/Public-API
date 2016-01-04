using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface IPermissionDao : IGenericDao<Permission, int>
    {
        IList<Permission> FindUserPermissions(User user);
    }
}
