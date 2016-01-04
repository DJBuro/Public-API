using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class OrderStatusDao : HibernateDaoSupport, IOrderStatusDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<OrderStatus,int> Members

        public IList<OrderStatus> FindAll()
        {
            throw new NotImplementedException();
        }

        public OrderStatus FindById(int id)
        {
            throw new NotImplementedException();
        }

        public OrderStatus Create(OrderStatus instance)
        {
            throw new NotImplementedException();
        }

        public OrderStatus Save(OrderStatus instance)
        {
            throw new NotImplementedException();
        }

        public void Update(OrderStatus instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(OrderStatus instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(OrderStatus type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<OrderStatus> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.OrderStatusDAO.Close();
        }

        #endregion

        #region IOrderStatusDao Members

        public IList<OrderStatus> GetOrdersByStore(Store store)
        {
            const string hql = "select os from OrderStatus as os where os.Order.Store.ExternalStoreId = :ID";

            var query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            query.SetString("ID", store.ExternalStoreId);
            query.SetCacheable(true);

            return this.OrderTrackingHibernateDaoFactory.OrderStatusDAO.FindByAdhocQuery(query);
        }

        public IList<OrderStatus> GetByOrder(long orderId)
        {
            const string hql = "select os from OrderStatus as os where os.Order.Id = :OrderId";

            var query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            query.SetInt64("OrderId", orderId);

            return this.OrderTrackingHibernateDaoFactory.OrderStatusDAO.FindByAdhocQuery(query);
        }

        public IList<OrderStatus> GetByOrderAndStatus(long orderId, string statusName)
        {
            const string hql = "select os from OrderStatus as os where os.Order.Id = :OrderId and os.Status.Name = :StatusName";

            var query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            query.SetInt64("OrderId", orderId);
            query.SetString("StatusName", statusName);
            //query.SetCacheable(true);

            return this.OrderTrackingHibernateDaoFactory.OrderStatusDAO.FindByAdhocQuery(query);
        }

        #endregion
    }
}
