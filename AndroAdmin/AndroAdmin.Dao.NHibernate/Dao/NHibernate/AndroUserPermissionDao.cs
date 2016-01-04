using System;
using System.Collections.Generic;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{
    class AndroUserPermissionDao : HibernateDaoSupport, IAndroUserPermissionDao
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }

        #region IGenericDao<AndroUserPermission,int> Members

        public IList<AndroUserPermission> FindAll()
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.FindAll();
        }

        public AndroUserPermission FindById(int id)
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.FindById(id);
        }

        public AndroUserPermission Create(AndroUserPermission instance)
        {
            throw new NotImplementedException();
        }

        public AndroUserPermission Save(AndroUserPermission userPermission)
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.SaveOrUpdate(userPermission);
        }

        public void Update(AndroUserPermission userPermission)
        {
            this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.Update(userPermission);
        }

        public void Delete(AndroUserPermission userPermission)
        {
            this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.Delete(userPermission);
        }

        public void DeleteAll(AndroUserPermission type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<AndroUserPermission> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.AndroAdminHibernateDaoFactory.AndroUserPermissionDAO.Close();
        }

        #endregion
    }
}
