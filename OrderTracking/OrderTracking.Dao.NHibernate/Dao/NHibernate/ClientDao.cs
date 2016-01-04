using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class ClientDao : HibernateDaoSupport, IClientDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Client,int> Members

        public IList<Client> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.ClientDAO.FindAll();
        }

        public Client FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientDAO.FindById(id);
        }

        public Client Create(Client client)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientDAO.Create(client);
        }

        public Client Save(Client client)
        {
            return this.OrderTrackingHibernateDaoFactory.ClientDAO.Save(client);
        }

        public void Update(Client client)
        {
            this.OrderTrackingHibernateDaoFactory.ClientDAO.Update(client);
        }

        public void Delete(Client client)
        {
            this.OrderTrackingHibernateDaoFactory.ClientDAO.Delete(client);
        }

        public void DeleteAll(Client client)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Client> clients)
        {
            this.OrderTrackingHibernateDaoFactory.ClientDAO.DeleteAll(clients);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.ClientDAO.Close();
        }

        #endregion

        #region IClientDao Members

//        public Client FindByExternalId(string id, Store store)
//        {
//            const string hql = "select ord from Order as ord where ord.ExternalOrderId =:ID and ord.Store.Id =:STOREID";

//            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

//            query.SetString("ID", id);
//            query.SetInt64("STOREID", store.Id.Value);
//            query.SetCacheable(true);
//            query.SetFirstResult(0);

//            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindFirstElementByAdhocQuery(query);
//        }


//        public IList<Order> FindUndelivered(Store store)
//        {
//            //const string hql = "select os.Order from OrderStatus as os where os.Order.Store.Id = :STOREID and os.Status.Id != 4 and os.Status.Id != 5 and os.Status.Id != 6";
//            const string hql = 
//                "select os.Order from OrderStatus as os " +
//                "where os.Order.Store.Id = :STOREID " +
////                "and os.Status.Id != 4 " +
//                "and os.Status.Id != 5 " +
//                "and os.Status.Id != 6";

//            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

//            query.SetInt64("STOREID", store.Id.Value);
//            query.SetCacheable(true);

//            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);
//        }

//        public IList<Order> FindByStore(Store store)
//        {
//            const string hql = "select os.Order from OrderStatus as os where os.Order.Store.Id = :STOREID";

//            IQuery query = this.OrderTrackingHibernateDaoFactory.OrderDAO.Session.CreateQuery(hql);

//            query.SetInt64("STOREID", store.Id.Value);

//            return this.OrderTrackingHibernateDaoFactory.OrderDAO.FindByAdhocQuery(query);
//        }

        #endregion
    }
}
