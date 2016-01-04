using System;
using System.Collections.Generic;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{
    public class ProjectDao : HibernateDaoSupport, IProjectDao
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }

        #region IGenericDao<Project,int> Members

        public IList<Project> FindAll()
        {
            return this.AndroAdminHibernateDaoFactory.ProjectDAO.FindAll();
        }

        public Project FindById(int id)
        {
            return this.AndroAdminHibernateDaoFactory.ProjectDAO.FindById(id);
        }

        public Project Create(Project instance)
        {
            throw new NotImplementedException();
        }

        public Project Save(Project instance)
        {
            throw new NotImplementedException();
        }

        public void Update(Project instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Project project)
        {
            this.AndroAdminHibernateDaoFactory.ProjectDAO.Delete(project);
        }

        public void DeleteAll(Project type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Project> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.AndroAdminHibernateDaoFactory.ProjectDAO.Close();
        }

        #endregion
    }
}
