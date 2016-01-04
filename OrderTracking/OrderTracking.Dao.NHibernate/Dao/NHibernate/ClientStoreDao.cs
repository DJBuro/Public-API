using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class ClientStoreDao : HibernateDaoSupport, IClientStoreDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<ClientStore,int> Members

        public IList<ClientStore> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.FindAll();
        }

        public ClientStore FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.FindById(id);
        }

        public ClientStore Create(ClientStore clientStore)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Create(clientStore);
        }

        public ClientStore Save(ClientStore clientStore)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Save(clientStore);
        }

        public void Update(ClientStore clientStore)
        {
            this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Update(clientStore);
        }

        public void Delete(ClientStore clientStore)
        {
            this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Delete(clientStore);
        }

        public void DeleteAll(ClientStore clientStore)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<ClientStore> clientStores)
        {
            this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.DeleteAll(clientStores);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.ClientDAO.Close();
        }

        #endregion

        #region IClientDao Members

        public IList<ClientStore> FindByChainId(Int64 id)
        {
            const string clientStoresHql = "select cs from ClientStore as cs where cs.ClientId =:ID";

            IQuery clientStoresQuery = this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Session.CreateQuery(clientStoresHql);

            clientStoresQuery.SetInt64("ID", id);
            clientStoresQuery.SetCacheable(true);
            clientStoresQuery.SetFirstResult(0);

            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.FindByAdhocQuery(clientStoresQuery);
        }

        public IList<ClientStore> FindByStoreId(Int64 storeId)
        {
            const string clientStoresHql = "select cs from ClientStore as cs where cs.StoreId =:ID";

            IQuery clientStoresQuery = this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Session.CreateQuery(clientStoresHql);

            clientStoresQuery.SetInt64("ID", storeId);
            clientStoresQuery.SetCacheable(true);
            clientStoresQuery.SetFirstResult(0);

            return this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.FindByAdhocQuery(clientStoresQuery);
        }

        public void DeleteByStoreId(Int64 storeId)
        {
            const string clientStoresHql = "delete from ClientStore where StoreId =:ID";

            IQuery clientStoresQuery = this.OrderTrackingHibernateDaoFactory.ClientStoreDAO.Session.CreateQuery(clientStoresHql);

            clientStoresQuery.SetInt64("ID", storeId);
            clientStoresQuery.SetCacheable(true);
            clientStoresQuery.SetFirstResult(0);

            clientStoresQuery.ExecuteUpdate();
        }

        #endregion
    }
}
