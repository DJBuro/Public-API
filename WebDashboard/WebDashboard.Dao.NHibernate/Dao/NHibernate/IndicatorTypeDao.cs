using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class IndicatorTypeDao : HibernateDaoSupport, IIndicatorTypeDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<IndicatorType,int> Members

        public IList<IndicatorType> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.IndicatorTypeDAO.FindAll();
        }

        public IndicatorType FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorType Create(IndicatorType instance)
        {
            throw new System.NotImplementedException();
        }

        public IndicatorType Save(IndicatorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(IndicatorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(IndicatorType instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IndicatorType type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<IndicatorType> deleteSet)
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
