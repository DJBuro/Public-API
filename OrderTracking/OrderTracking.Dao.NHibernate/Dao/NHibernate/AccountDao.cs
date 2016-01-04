using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class AccountDao: HibernateDaoSupport, IAccountDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Account,int> Members

        public IList<Account> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.AccountDAO.FindAll();
        }

        public Account FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.AccountDAO.FindById(id);
        }

        public Account Create(Account account)
        {
            return this.OrderTrackingHibernateDaoFactory.AccountDAO.Create(account);
        }

        public Account Save(Account account)
        {
            return this.OrderTrackingHibernateDaoFactory.AccountDAO.Save(account);
        }

        public void Update(Account account)
        {
            this.OrderTrackingHibernateDaoFactory.AccountDAO.Update(account);
        }

        public void Delete(Account instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Account type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Account> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.AccountDAO.Close();
        }

        #endregion

        #region IAccountDao Members

        public bool Verify(string userName, string password, out Account account)
        {
            const string hql = "select acc from Account as acc where acc.UserName=:UNAME and acc.Password=:PASS";

            IQuery query = this.OrderTrackingHibernateDaoFactory.AccountDAO.Session.CreateQuery(hql);

            query.SetString("UNAME", userName);
            query.SetString("PASS", password);

            query.SetCacheable(true);

            account = this.OrderTrackingHibernateDaoFactory.AccountDAO.FindFirstElementByAdhocQuery(query);

            if (account != null)
                return true;

            return false;
        }

 
        public Account FindByStoreRamesesId(string storeId)
        {
            const string hql = "select account from Account as account where account.Store.ExternalStoreId = :ID";

            IQuery query = this.OrderTrackingHibernateDaoFactory.StoreDAO.Session.CreateQuery(hql);

            query.SetString("ID", storeId);
            query.SetCacheable(true);

            return this.OrderTrackingHibernateDaoFactory.AccountDAO.FindFirstElementByAdhocQuery(query);
        }



        #endregion
    }
}
