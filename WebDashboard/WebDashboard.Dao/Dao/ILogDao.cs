using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface ILogDao : IGenericDao<Log, int>
    {
        IList<Log> FindAllGrouped();
    }
}
