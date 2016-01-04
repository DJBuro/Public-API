using System.Collections.Generic;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao
{
    public interface IOrderStatusDao : IGenericDao<OrderStatus, int>
    {
        IList<OrderStatus> GetOrdersByStore(Store store);
        IList<OrderStatus> GetByOrder(long orderId);
        IList<OrderStatus> GetByOrderAndStatus(long orderId, string statusName);
    }
}
