using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;


namespace WebDashboard.Dao.NHibernate
{
    public class ValueTypeDao : HibernateDaoSupport, IValueTypeDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<ValueType,int> Members

        public IList<ValueType> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.ValueTypeDAO.FindAll();
        }

        public ValueType FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ValueType Create(ValueType valueType)
        {
            throw new System.NotImplementedException();
        }

        public ValueType Save(ValueType valueType)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ValueType valueType)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(ValueType valueType)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(ValueType type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<ValueType> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.ValueTypeDAO.Close();
        }

        #endregion
    }
}
