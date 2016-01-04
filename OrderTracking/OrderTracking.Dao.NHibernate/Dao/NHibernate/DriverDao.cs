using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class DriverDao : HibernateDaoSupport, IDriverDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Driver,int> Members

        public IList<Driver> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.DriverDAO.FindAll();
        }

        public Driver FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.DriverDAO.FindById(id);
        }

        public Driver Create(Driver driver)
        {
            return this.OrderTrackingHibernateDaoFactory.DriverDAO.Create(driver);
        }

        public Driver Save(Driver driver)
        {
            return this.OrderTrackingHibernateDaoFactory.DriverDAO.Save(driver);
        }

        public void Update(Driver driver)
        {
            this.OrderTrackingHibernateDaoFactory.DriverDAO.Update(driver);
        }

        public void Delete(Driver driver)
        {
            this.OrderTrackingHibernateDaoFactory.DriverDAO.Delete(driver);
        }

        public void DeleteAll(Driver driverType)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Driver> deleteDriverSet)
        {
            this.OrderTrackingHibernateDaoFactory.DriverDAO.DeleteAll(deleteDriverSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.DriverDAO.Close();
        }

        #endregion

        #region IDriverDao Members

        public Driver FindByExternalId(string id, Store store)
        {
            const string hql = "select dr from Driver as dr where dr.ExternalDriverId = :ID and dr.Store.Id =:STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.DriverDAO.Session.CreateQuery(hql);

            query.SetString("ID", id);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var driver = this.OrderTrackingHibernateDaoFactory.DriverDAO.FindFirstElementByAdhocQuery(query);

            return driver;  
        }

        public Driver FindByOrder(Order order, Store store)
        {
            const string hql = "select dro.Driver from DriverOrder as dro where dro.Order.Id = :ORDERID and dro.Order.Store.Id =:STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.DriverDAO.Session.CreateQuery(hql);

            query.SetInt64("ORDERID", order.Id.Value);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var driver = this.OrderTrackingHibernateDaoFactory.DriverDAO.FindFirstElementByAdhocQuery(query);

            return driver;
        }

        public IList<Driver> FindByStore(Store store)
        {
            const string hql = "select dr from Driver as dr where dr.Store.Id = :STOREID ";

            IQuery query = this.OrderTrackingHibernateDaoFactory.DriverDAO.Session.CreateQuery(hql);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var drivers = this.OrderTrackingHibernateDaoFactory.DriverDAO.FindByAdhocQuery(query);

            return drivers; 
        }

        public IList<Order> FindOrders(PremisesDriver driver, Store store)
        {

            const string hql = "select driverOrders.Order from DriverOrder as driverOrders where driverOrders.Driver.ExternalDriverId =:DRIVERID and driverOrders.Driver.Store.Id =:STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.DriverDAO.Session.CreateQuery(hql);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetString("DRIVERID", driver.ExternalId);
            query.SetCacheable(true);

            var orders = this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);

            return orders;
        }


        public IList<Order> FindOrders(Driver driver, Store store)
        {
            const string hql = "select driverOrders.Order from DriverOrder as driverOrders where driverOrders.Driver.ExternalDriverId =:DRIVERID and driverOrders.Driver.Store.Id =:STOREID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.DriverDAO.Session.CreateQuery(hql);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetString("DRIVERID", driver.ExternalDriverId);
            query.SetCacheable(true);

            var orders = this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);

            return orders;
        }

        #endregion
    }
}
