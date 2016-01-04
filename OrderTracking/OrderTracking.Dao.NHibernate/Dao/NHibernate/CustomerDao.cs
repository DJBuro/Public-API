using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class CustomerDao : HibernateDaoSupport, ICustomerDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Customer,int> Members

        public IList<Customer> FindAll()
        {
            throw new NotImplementedException();
        }

        public Customer FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Customer Create(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer Save(Customer customer)
        {
            return this.OrderTrackingHibernateDaoFactory.CustomerDAO.Save(customer);
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void Delete(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Customer customerType)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Customer> deleteCustomerSet)
        {
            this.OrderTrackingHibernateDaoFactory.CustomerDAO.DeleteAll(deleteCustomerSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.CustomerDAO.Close();
        }

        #endregion

        #region ICustomerDao Members

        public IList<Customer> FindByStore(Store store)
        {
            const string hql = "select cusord.Customer from CustomerOrder as cusord where cusord.Order.Store.Id = :STOREID ";

            var query = this.OrderTrackingHibernateDaoFactory.CustomerDAO.Session.CreateQuery(hql);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var customers = this.OrderTrackingHibernateDaoFactory.CustomerDAO.FindByAdhocQuery(query);

            return customers; 
        }

        public Customer FindByUserCredentials(string userCredentials, Store store)
        {
            const string hql = "select cusord.Customer from CustomerOrder as cusord where cusord.Customer.Credentials =:USERCREDS and cusord.Order.Store.Id = :STOREID";

            var query = this.OrderTrackingHibernateDaoFactory.CustomerDAO.Session.CreateQuery(hql);
            query.SetString("USERCREDS", userCredentials);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);
            query.SetFirstResult(0);

            var customer = this.OrderTrackingHibernateDaoFactory.CustomerDAO.FindFirstElementByAdhocQuery(query);

            return customer; 
        }



        public Customer FindByUserCredentialsOnly(string userCredentials)
        {
            const string hql = "select cusord.Customer from CustomerOrder as cusord where cusord.Customer.Credentials =:USERCREDS";

            var query = this.OrderTrackingHibernateDaoFactory.CustomerDAO.Session.CreateQuery(hql);
            query.SetString("USERCREDS", userCredentials);
            query.SetCacheable(true);
            query.SetFirstResult(0);

            var customer = this.OrderTrackingHibernateDaoFactory.CustomerDAO.FindFirstElementByAdhocQuery(query);

            return customer; 
        }

        #endregion
    }
}
