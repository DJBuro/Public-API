using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IStoreDao : IGenericDao<Store, int>
    {
        Store FindByExternalId(string id);
        IList<Store> FindAllGpsEnabled();
    }
}
