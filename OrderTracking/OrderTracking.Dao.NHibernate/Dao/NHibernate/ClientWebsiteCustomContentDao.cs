using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class ClientWebsiteCustomContentDao : HibernateDaoSupport, IClientWebsiteCustomContentDao
    {
        public OrderTrackingHibernateDAOFactory orderTrackingHibernateDAOFactory { get; set; }

        #region IGenericDao<ClientWebsiteCustomContent,int> Members

        public IList<ClientWebsiteCustomContent> FindAll()
        {
            return this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.FindAll();
        }

        public ClientWebsiteCustomContent FindById(int id)
        {
            return this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.FindById(id);
        }

        public ClientWebsiteCustomContent Create(ClientWebsiteCustomContent clientWebsiteCustomContent)
        {
            return this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Create(clientWebsiteCustomContent);
        }

        public ClientWebsiteCustomContent Save(ClientWebsiteCustomContent clientWebsiteCustomContent)
        {
            return this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Save(clientWebsiteCustomContent);
        }

        public void Update(ClientWebsiteCustomContent clientWebsiteCustomContent)
        {
            this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Update(clientWebsiteCustomContent);
        }

        public void Delete(ClientWebsiteCustomContent clientWebsiteCustomContent)
        {
            this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Delete(clientWebsiteCustomContent);
        }

        public void DeleteAll(ClientWebsiteCustomContent clientWebsiteCustomContent)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<ClientWebsiteCustomContent> clientWebsiteCustomContent)
        {
            this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.DeleteAll(clientWebsiteCustomContent);
        }

        public void Close()
        {
            this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Close();
        }

        #endregion

        #region IClientWebsiteCustomContentDao Members

        public IList<ClientWebsiteCustomContent> FindByChainId(Int64 chainId)
        {
            const string clientStoresHql = "select cs from ClientWebsiteCustomContent as cs where cs.ClientId =:ID";

            IQuery clientStoresQuery = this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Session.CreateQuery(clientStoresHql);

            clientStoresQuery.SetInt64("ID", chainId);
            clientStoresQuery.SetCacheable(true);
            clientStoresQuery.SetFirstResult(0);

            return this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.FindByAdhocQuery(clientStoresQuery);
        }

        public void DeleteAllForChain(Int64 chainId)
        {
            const string clientStoresHql = "delete from ClientWebsiteCustomContent cs where cs.ClientId =:ID";

            IQuery clientStoresQuery = this.orderTrackingHibernateDAOFactory.ClientWebsiteCustomContentDAO.Session.CreateQuery(clientStoresHql);

            clientStoresQuery.SetInt64("ID", chainId);
            clientStoresQuery.SetCacheable(true);
            clientStoresQuery.SetFirstResult(0);

            clientStoresQuery.ExecuteUpdate();
        }

        #endregion
    }
}
