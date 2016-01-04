using System.Collections.Generic;
using NHibernate;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class UserDao : HibernateDaoSupport, IUserDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<User,int> Members

        public IList<User> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.UserDAO.FindAll();
        }

        public User FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public User Create(User user)
        {
            return this.OrderTrackingHibernateDaoFactory.UserDAO.Create(user);
        }

        public User Save(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(User type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<User> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
