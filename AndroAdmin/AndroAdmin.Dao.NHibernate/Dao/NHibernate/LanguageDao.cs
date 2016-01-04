using System;
using System.Collections.Generic;
using AndroAdmin.Dao.Domain;
using AndroAdmin.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace AndroAdmin.Dao.NHibernate
{

    public class LanguageDao : HibernateDaoSupport, ILanguageDao
    {
        public AndroAdminHibernateDAOFactory AndroAdminHibernateDaoFactory { get; set; }

        #region IGenericDao<Language,int> Members

        public IList<Language> FindAll()
        {
            return this.AndroAdminHibernateDaoFactory.LanguageDAO.FindAll();
        }

        public Language FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Language Create(Language instance)
        {
            throw new NotImplementedException();
        }

        public Language Save(Language instance)
        {
            throw new NotImplementedException();
        }

        public void Update(Language instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Language instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Language type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Language> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
