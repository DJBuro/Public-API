using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;


namespace WebDashboard.Dao.NHibernate
{
    public class DefinitionDao : HibernateDaoSupport, IDefinitionDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Definition,int> Members

        public IList<Definition> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.DefinitionDAO.FindAll();
        }

        public Definition FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Definition Create(Definition instance)
        {
            throw new System.NotImplementedException();
        }

        public Definition Save(Definition instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Definition instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Definition instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(Definition type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Definition> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.DefinitionDAO.Close();
        }

        #endregion
    }
}
