using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class UserRegionDao : HibernateDaoSupport, IUserRegionDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<UserRegion,int> Members

        public IList<UserRegion> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.UserRegionDAO.FindAll();
        }

        public UserRegion FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.UserRegionDAO.FindById(id);
        }

        public UserRegion Create(UserRegion userRegion)
        {
            return this.WebDashboardHibernateDaoFactory.UserRegionDAO.Create(userRegion);
        }

        public UserRegion Save(UserRegion userRegion)
        {
            return this.WebDashboardHibernateDaoFactory.UserRegionDAO.Save(userRegion);
        }

        public void Update(UserRegion userRegion)
        {
            this.WebDashboardHibernateDaoFactory.UserRegionDAO.Update(userRegion);
        }

        public void Delete(UserRegion userRegion)
        {
            this.WebDashboardHibernateDaoFactory.UserRegionDAO.Delete(userRegion);
        }

        public void DeleteAll(UserRegion type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<UserRegion> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.UserRegionDAO.Close();
        }

        #endregion

        public IList<UserRegion> FindByUserId(int userId)
        {
            const string hql = "select u from UserRegion as u where u.UserId =:USERID";

            var query = this.WebDashboardHibernateDaoFactory.UserDAO.Session.CreateQuery(hql);

            query.SetInt32("USERID", userId);

            return this.WebDashboardHibernateDaoFactory.UserRegionDAO.FindByAdhocQuery(query);
        }
    }
}
