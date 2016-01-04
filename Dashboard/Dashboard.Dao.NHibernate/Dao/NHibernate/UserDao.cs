using System;
using System.Collections.Generic;
using System.Web;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;


namespace Dashboard.Dao.NHibernate
{
    public class UserDao : HibernateDaoSupport, IUserDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<User,int> Members

        public IList<User> FindAll()
        {
            throw new NotImplementedException();
        }

        public User FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.UserDAO.FindById(id);
        }

        public User Create(User user)
        {
            return this.DashboardHibernateDAOFactory.UserDAO.Create(user);
        }

        public User Save(User user)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(User type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<User> deleteUserSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.UserDAO.Close();
        }

        #endregion

        #region IUserDao Members

        public User Login(string emailAddress, string password, HttpResponseBase response)
        {
            var user = new User();
            
            string hql = "select u from User u where u.EmailAddress = :USERNAME and u.Password = :PASSWORD";
            IQuery query = this.DashboardHibernateDAOFactory.UserDAO.Session.CreateQuery(hql);

            query.SetString("USERNAME", emailAddress);
            query.SetString("PASSWORD", password);

            user = this.DashboardHibernateDAOFactory.UserDAO.FindFirstElementByAdhocQuery(query);

            if (user == null)
                return user;

            Dashboard.Web.Mvc.Utilities.Cookie.SetAuthoriationCookie(user, response);


            return user;
        }

        #endregion
    }
}
