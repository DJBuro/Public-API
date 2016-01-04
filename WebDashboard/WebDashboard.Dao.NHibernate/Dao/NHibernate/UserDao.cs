using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class UserDao : HibernateDaoSupport, IUserDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<User,int> Members

        public IList<User> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.UserDAO.FindAll();
        }

        public User FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.UserDAO.FindById(id);
        }

        public User Create(User user)
        {
            return this.WebDashboardHibernateDaoFactory.UserDAO.Create(user);
        }

        public User Save(User user)
        {
            return this.WebDashboardHibernateDaoFactory.UserDAO.Save(user);
        }

        public void Update(User user)
        {
            this.WebDashboardHibernateDaoFactory.UserDAO.Update(user);
        }

        public void Delete(User user)
        {
            this.WebDashboardHibernateDaoFactory.UserDAO.Delete(user);
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
            this.WebDashboardHibernateDaoFactory.UserDAO.Close();
        }

        #endregion

        #region IUserDao Members

        public User FindByEmailAndPassword(string emailAddress, string password)
        {
            const string hql = "select u from User as u where u.EmailAddress =:EMAIL and u.Password =:PASSWORD and u.Active = 1";

            var query = this.WebDashboardHibernateDaoFactory.UserDAO.Session.CreateQuery(hql);

            query.SetString("EMAIL", emailAddress);
            query.SetString("PASSWORD", password);

            return this.WebDashboardHibernateDaoFactory.UserDAO.FindFirstElementByAdhocQuery(query);
        }


        public User CheckEmailExists(string emailAddress)
        {
            const string hql = "select u from User as u where u.EmailAddress =:EMAIL";

            var query = this.WebDashboardHibernateDaoFactory.UserDAO.Session.CreateQuery(hql);

            query.SetString("EMAIL", emailAddress);

            return (User)query.UniqueResult();
        }

        public IList<User> FindAllByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select u from User as u where u.HeadOffice =:HEADOFFICEID order by u.EmailAddress asc";

            var query = this.WebDashboardHibernateDaoFactory.UserDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICEID", headOffice.Id.Value);

            return this.WebDashboardHibernateDaoFactory.UserDAO.FindByAdhocQuery(query);
        }

        #endregion
    }
}
