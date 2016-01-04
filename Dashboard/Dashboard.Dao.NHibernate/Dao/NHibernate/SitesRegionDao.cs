using System;
using System.Collections.Generic;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class SitesRegionDao : HibernateDaoSupport, ISitesRegionDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<SitesRegion,int> Members

        public IList<SitesRegion> FindAll()
        {
            return this.DashboardHibernateDAOFactory.SitesRegionDAO.FindAll();
        }

        public SitesRegion FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.SitesRegionDAO.FindById(id);
        }

        public SitesRegion Create(SitesRegion sitesRegion)
        {
            return this.DashboardHibernateDAOFactory.SitesRegionDAO.Create(sitesRegion);
        }

        public SitesRegion Save(SitesRegion sitesRegion)
        {
            return this.DashboardHibernateDAOFactory.SitesRegionDAO.Save(sitesRegion);
        }

        public void Update(SitesRegion sitesRegion)
        {
            this.DashboardHibernateDAOFactory.SitesRegionDAO.Update(sitesRegion);
        }

        public void Delete(SitesRegion instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(SitesRegion type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<SitesRegion> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.SitesRegionDAO.Close();
        }

        public IList<SitesRegion> FindBySite(Site site)
        {
            //var xyz = new SitesRegion();
            //xyz.Site.Id

            string hql = "select sr from SitesRegion sr where sr.Site.Id = :SITEID";
            IQuery query = this.DashboardHibernateDAOFactory.SitesRegionDAO.Session.CreateQuery(hql);

            query.SetInt32("SITEID", site.Id.Value);

           return this.DashboardHibernateDAOFactory.SitesRegionDAO.FindByAdhocQuery(query);

        }

        #endregion
    }
}
