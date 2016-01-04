using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class SiteTypeDao : HibernateDaoSupport, ISiteTypeDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<SiteType,int> Members

        public IList<SiteType> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.SiteTypeDAO.FindAll();
        }

        public SiteType FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public SiteType Create(SiteType instance)
        {
            throw new System.NotImplementedException();
        }

        public SiteType Save(SiteType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(SiteType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(SiteType instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(SiteType type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<SiteType> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.SiteTypeDAO.Close();
        }

        #endregion
    }
}
