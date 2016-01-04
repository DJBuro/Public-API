using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IDriverDao : IGenericDao<Driver, int>
    {
        Driver FindByExternalId(string externalDriverId, Store store);
        Driver FindByOrder(Order order, Store store);
        IList<Driver> FindByStore(Store store);
        IList<Order> FindOrders(Driver driver, Store store);
        IList<Order> FindOrders(PremisesDriver driver, Store store);
    }
}
