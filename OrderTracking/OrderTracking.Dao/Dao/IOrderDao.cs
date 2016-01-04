using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IOrderDao : IGenericDao<Order, int>
    {
        Order FindByExternalId(string externalOrderId, Store store);
        IList<Order> FindUndelivered(Store store);
        IList<Order> FindByStore(Store store);
    }
}
