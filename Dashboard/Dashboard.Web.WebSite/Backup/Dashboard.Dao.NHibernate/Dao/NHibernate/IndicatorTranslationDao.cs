using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class IndicatorTranslationDao : HibernateDaoSupport, IIndicatorTranslationDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<IndicatorTranslation,int> Members

        public IList<IndicatorTranslation> FindAll()
        {
            throw new NotImplementedException();
        }

        public IndicatorTranslation FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.IndicatorTranslationDAO.FindById(id);
        }

        public IndicatorTranslation Create(IndicatorTranslation instance)
        {
            throw new NotImplementedException();
        }

        public IndicatorTranslation Save(IndicatorTranslation indicatorTranslation)
        {
            return this.DashboardHibernateDAOFactory.IndicatorTranslationDAO.Save(indicatorTranslation);
        }

        public void Update(IndicatorTranslation indicatorTranslation)
        {
            this.DashboardHibernateDAOFactory.IndicatorTranslationDAO.Update(indicatorTranslation);
        }

        public void Delete(IndicatorTranslation instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IndicatorTranslation type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<IndicatorTranslation> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.IndicatorTranslationDAO.Close();
        }

        #endregion
    }
}
