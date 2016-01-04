using System;
using System.Collections.Generic;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{
    public class AndroUserDao: HibernateDaoSupport, IAndroUserDao
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }

        #region IGenericDao<AndroUser,int> Members

        public IList<AndroUser> FindAll()
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserDAO.FindAll();
        }

        public AndroUser FindById(int id)
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserDAO.FindById(id);
        }

        public AndroUser Create(AndroUser androUser)
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserDAO.Create(androUser);
        }

        public AndroUser Save(AndroUser androUser)
        {
            return this.AndroAdminHibernateDaoFactory.AndroUserDAO.SaveOrUpdate(androUser);
        }

        public void Update(AndroUser instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(AndroUser androUser)
        {
            this.AndroAdminHibernateDaoFactory.AndroUserDAO.Delete(androUser);
        }

        public void DeleteAll(AndroUser type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<AndroUser> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.AndroAdminHibernateDaoFactory.AndroUserDAO.Close();
        }

        #endregion


        #region IAndroUserDao Members

        public AndroUser FindByEmailAddressPassword(string emailAddress, string password)
        {

            var user = new AndroUser();

            string hql = "select u from AndroUser u where u.EmailAddress = :USERNAME and u.Password = :PASSWORD and u.Active = 1";
            IQuery query = this.AndroAdminHibernateDaoFactory.AndroUserDAO.Session.CreateQuery(hql);

            query.SetString("USERNAME", emailAddress);
            query.SetString("PASSWORD", password);

            user = this.AndroAdminHibernateDaoFactory.AndroUserDAO.FindFirstElementByAdhocQuery(query);

            if (user == null)
                return user;

            return user;
        
        }

        #endregion
    }
}
