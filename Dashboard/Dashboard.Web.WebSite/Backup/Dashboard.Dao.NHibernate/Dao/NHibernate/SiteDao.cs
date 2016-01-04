using System;
using System.Collections.Generic;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class SiteDao : HibernateDaoSupport, ISiteDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<Site,int> Members

        public IList<Site> FindAll()
        {
            return this.DashboardHibernateDAOFactory.SiteDAO.FindAll();
        }

        public Site FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.SiteDAO.FindById(id);
        }

        public Site Create(Site site)
        {
            return this.DashboardHibernateDAOFactory.SiteDAO.Create(site);
        }

        public Site Save(Site site)
        {
            return this.DashboardHibernateDAOFactory.SiteDAO.Save(site);
        }

        public void Update(Site site)
        {
            this.DashboardHibernateDAOFactory.SiteDAO.Update(site);
        }

        public void Delete(Site site)
        {
            this.DashboardHibernateDAOFactory.SiteDAO.Delete(site);
        }

        public void DeleteAll(Site type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Site> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.SiteDAO.Close();
        }

        #endregion

        #region ISiteDao Members

        public Site FindByIP(string IPAddress)
        {

            string hql = "select s from Site as s where s.IPAddress = :IPADDRESS";

            IQuery query = this.DashboardHibernateDAOFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetString("IPADDRESS", IPAddress);


            query.SetCacheable(true);


            return this.DashboardHibernateDAOFactory.SiteDAO.FindFirstElementByAdhocQuery(query);
        }

        /// <summary>
        /// Because a site is treated as a user during login.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public void Login(Site site, System.Web.HttpResponseBase response)
        {
            Dashboard.Web.Mvc.Utilities.Cookie.SetAuthoriationCookie(site, response);
        }

        #endregion
    }
}
