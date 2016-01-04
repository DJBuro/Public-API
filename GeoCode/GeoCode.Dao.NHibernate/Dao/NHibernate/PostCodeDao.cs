using System;
using System.Collections.Generic;
using GeoCode.Dao.Domain;
using GeoCode.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace GeoCode.Dao.NHibernate
{
    public class PostCodeDao : HibernateDaoSupport, IPostCodeDao
    {
        public GeoCodeHibernateDAOFactory GeoCodeHibernateDaoFactory { get; set; }

        #region IGenericDao<PostCode,int> Members

        public IList<PostCode> FindAll()
        {
            throw new NotImplementedException();
        }

        public PostCode FindById(int id)
        {
            throw new NotImplementedException();
        }

        public PostCode Create(PostCode instance)
        {
            throw new NotImplementedException();
        }

        public PostCode Save(PostCode instance)
        {
            throw new NotImplementedException();
        }

        public void Update(PostCode instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(PostCode instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(PostCode type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<PostCode> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPostCodeDao Members

        public PostCode Find(string postCode)
        {
            const string hql = "select p from PostCode as p where p.PCode= :PC";

            var query = this.GeoCodeHibernateDaoFactory.PostCodeDAO.Session.CreateQuery(hql);

            query.SetString("PC", postCode);
            //query.SetCacheable(true);

            return this.GeoCodeHibernateDaoFactory.PostCodeDAO.FindFirstElementByAdhocQuery(query);
        }

        #endregion
    }
}
