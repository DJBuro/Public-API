using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class DivisorTypeDao : HibernateDaoSupport, IDivisorTypeDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<DivisorType,int> Members

        public IList<DivisorType> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.DivisorTypeDAO.FindAll();
        }

        public DivisorType FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public DivisorType Create(DivisorType instance)
        {
            throw new System.NotImplementedException();
        }

        public DivisorType Save(DivisorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(DivisorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(DivisorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(DivisorType type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<DivisorType> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.DivisorTypeDAO.Close();
        }

        #endregion
    }
}
