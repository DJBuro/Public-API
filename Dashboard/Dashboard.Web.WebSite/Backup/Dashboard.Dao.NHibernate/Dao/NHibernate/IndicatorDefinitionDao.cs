using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class IndicatorDefinitionDao : HibernateDaoSupport, IIndicatorDefinitionDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<IndicatorDefinition,int> Members

        public IList<IndicatorDefinition> FindAll()
        {
            throw new NotImplementedException();
        }

        public IndicatorDefinition FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.IndicatorDefinitionDAO.FindById(id);
        }

        public IndicatorDefinition Create(IndicatorDefinition instance)
        {
            throw new NotImplementedException();
        }

        public IndicatorDefinition Save(IndicatorDefinition definition)
        {
            return this.DashboardHibernateDAOFactory.IndicatorDefinitionDAO.Save(definition);
        }

        public void Update(IndicatorDefinition definition)
        {
            this.DashboardHibernateDAOFactory.IndicatorDefinitionDAO.Update(definition);
        }

        public void Delete(IndicatorDefinition instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IndicatorDefinition type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<IndicatorDefinition> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.IndicatorDefinitionDAO.Close();
        }

        #endregion

        #region IIndicatorDefinitionDao Members

        public IList<IndicatorDefinition> FindByHeadOffice(HeadOffice headOffice)
        {
            return this.DashboardHibernateDAOFactory.IndicatorDefinitionDAO
                .FindAll()
                .Where(c => c.HeadOffice.Id.Value == headOffice.Id.Value)
                .OrderBy(c => c.ColumnNumber)
                .ToList(); 
        }

        #endregion
    }
}
