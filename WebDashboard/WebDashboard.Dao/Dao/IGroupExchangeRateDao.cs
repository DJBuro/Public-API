using WebDashboard.Dao.Domain;
using System.Collections.Generic;

namespace WebDashboard.Dao
{
    public interface IGroupExchangeRateDao : IGenericDao<GroupExchangeRate, int>
    {
        IList<GroupExchangeRate> FindByGroupId(int groupId);
    }
}
