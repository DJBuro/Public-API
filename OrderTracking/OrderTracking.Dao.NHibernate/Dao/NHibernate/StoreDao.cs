using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class StoreDao : HibernateDaoSupport, IStoreDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Store,int> Members

        public IList<Store> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.StoreDAO.FindAll();
        }

        public Store FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.StoreDAO.FindById(id);
        }

        public Store Create(Store store)
        {
            return this.OrderTrackingHibernateDaoFactory.StoreDAO.Create(store);
        }

        public Store Save(Store store)
        {
            return this.OrderTrackingHibernateDaoFactory.StoreDAO.Save(store);
        }

        public void Update(Store store)
        {
            this.OrderTrackingHibernateDaoFactory.StoreDAO.Update(store);
        }

        public void Delete(Store store)
        {
            this.OrderTrackingHibernateDaoFactory.StoreDAO.Delete(store);
        }

        public void DeleteAll(Store storeType)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Store> deleteStoreSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.StoreDAO.Close();
        }

        #endregion

        #region IStoreDao Members

        public Store FindByExternalId(string id)
        {
            const string hql = "select st from Store as st where st.ExternalStoreId = :ID";

            var query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            query.SetString("ID", id);
            query.SetCacheable(true);

            var store = this.OrderTrackingHibernateDaoFactory.StoreDAO.FindFirstElementByAdhocQuery(query);

            return store;
        }

        public IList<Store> FindAllGpsEnabled()
        {
            const string hql = "select a.Store from Account as a where a.GpsEnabled = 1";

            var query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            return this.OrderTrackingHibernateDaoFactory.StoreDAO.FindByAdhocQuery(query);
        }

        #endregion
    }
}
