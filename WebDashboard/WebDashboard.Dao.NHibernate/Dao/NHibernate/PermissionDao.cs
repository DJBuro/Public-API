using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class PermissionDao : HibernateDaoSupport, IPermissionDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Permission,int> Members

        public IList<Permission> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Permission FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Permission Create(Permission permission)
        {
            throw new System.NotImplementedException();
        }

        public Permission Save(Permission permission)
        {
            return this.WebDashboardHibernateDaoFactory.PermissionDAO.Save(permission);
        }

        public void Update(Permission permission)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Permission permission)
        {
            this.WebDashboardHibernateDaoFactory.PermissionDAO.Delete(permission);
        }

        public void DeleteAll(Permission type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Permission> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.PermissionDAO.Close();
        }

        #endregion

        #region IPermissionDao Members

        public IList<Permission> FindUserPermissions(User user)
        {
            const string hql = "select p from Permission as p where p.User.Id = :USERID";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("USERID", user.Id.Value);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.PermissionDAO.FindByAdhocQuery(query); ;
        }

        #endregion
    }
}
