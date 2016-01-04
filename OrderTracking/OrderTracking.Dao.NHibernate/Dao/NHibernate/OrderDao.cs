using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class OrderDao : HibernateDaoSupport, IOrderDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Order,int> Members

        public IList<Order> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindAll();
        }

        public Order FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindById(id);
        }

        public Order Create(Order order)
        {
            return this.OrderTrackingHibernateDaoFactory.OrderDAO.Create(order);
        }

        public Order Save(Order order)
        {
            return this.OrderTrackingHibernateDaoFactory.OrderDAO.Save(order);
        }

        public void Update(Order order)
        {
            this.OrderTrackingHibernateDaoFactory.OrderDAO.Update(order);
        }

        public void Delete(Order order)
        {
            this.OrderTrackingHibernateDaoFactory.OrderDAO.Delete(order);
        }

        public void DeleteAll(Order orderType)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Order> deleteOrderSet)
        {
            this.OrderTrackingHibernateDaoFactory.OrderDAO.DeleteAll(deleteOrderSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.OrderDAO.Close();
        }

        #endregion

        #region IOrderDao Members

        public Order FindByExternalId(string id, Store store)
        {
            const string hql = "select ord from Order as ord where ord.ExternalOrderId =:ID and ord.Store.Id =:STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

            query.SetString("ID", id);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);
            query.SetFirstResult(0);

            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindFirstElementByAdhocQuery(query);
        }


        public IList<Order> FindUndelivered(Store store)
        {
            //const string hql = "select os.Order from OrderStatus as os where os.Order.Store.Id = :STOREID and os.Status.Id != 4 and os.Status.Id != 5 and os.Status.Id != 6";
            const string hql = 
                "select os.Order from OrderStatus as os " +
                "where os.Order.Store.Id = :STOREID " +
//                "and os.Status.Id != 4 " +
                "and os.Status.Id != 5 " +
                "and os.Status.Id != 6";

            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);
        }

        public IList<Order> FindByStore(Store store)
        {
            const string hql = "select os.Order from OrderStatus as os where os.Order.Store.Id = :STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

            query.SetInt64("STOREID", store.Id.Value);

            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);
        }

        #endregion
    }
}
