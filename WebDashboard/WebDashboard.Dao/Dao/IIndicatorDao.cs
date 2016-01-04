using System.Collections.Generic;
using WebDashboard.Dao.Domain;

namespace WebDashboard.Dao
{
    public interface IIndicatorDao : IGenericDao<Indicator, int>
    {
        IList<Indicator> GetIndictorsByHeadOffice(HeadOffice headOffice);
    }
}
