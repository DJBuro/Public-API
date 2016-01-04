using System.Collections.Generic;
using Dashboard.Dao.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Dao
{
    public interface IIndicatorDefinitionDao : IGenericDao<IndicatorDefinition, int>
    {
        IList<IndicatorDefinition> FindByHeadOffice(HeadOffice headOffice);
    }
}
