using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using NHibernate;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class DashboardDataDao : HibernateDaoSupport, IDashboardDataDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<DashboardData,int> Members

        public IList<DashboardData> FindAll()
        {
            //note: only for testing!!
           //return this.DashboardHibernateDAOFactory.DashboardDataDAO.FindAll().ToList();
            //only bring back data from today
            var todaysData = this.DashboardHibernateDAOFactory.DashboardDataDAO.FindAll().Where(c => c.LastUpdated.Value.Date.DayOfYear == DateTime.Now.Date.DayOfYear).Where(c => c.Site.Enabled).ToList();

            var afterSixAmData = from k in todaysData where (DateTime.Compare(k.LastUpdated.Value.Date , Convert.ToDateTime("06:00:00 AM")) < 0) select k;

            return afterSixAmData.ToList();

        }

        public DashboardData FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.DashboardDataDAO.FindById(id);
        }

        public DashboardData Create(DashboardData dashboardData)
        {
            throw new NotImplementedException();
        }

        public DashboardData Save(DashboardData dashboardData)
        {
            throw new NotImplementedException();
        }

        public void Update(DashboardData dashboardData)
        {
            throw new NotImplementedException();
        }

        public void Delete(DashboardData dashboardData)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(DashboardData type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<DashboardData> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
           this.DashboardHibernateDAOFactory.DashboardDataDAO.Close();
        }

        #endregion

        #region IDashboardDataDao Members

        public IList<DashboardData> FindAll(Site site)
        {
            var dd = new DashboardData();
            //bool b = dd.LastUpdated.Value > DateTime.Now.Date.Date;

            string hql = "select dd from DashboardData dd where dd.Site.Id = :SiteId";

            IQuery query = this.DashboardHibernateDAOFactory.DashboardDataDAO.Session.CreateQuery(hql);

           query.SetInt32("SiteId", site.Id.Value);

            return this.DashboardHibernateDAOFactory.DashboardDataDAO.FindByAdhocQuery(query);            
        }


        //todo:
        public IList<DashboardData> FindAll(Region region)
        {

            //var dd = new List<DashboardData>();

            //foreach (SitesRegion p in region.SitesRegions)
            //{
            //    dd.Add((DashboardData) p.Site.DashboardData);

            //}

            //return dd;



            //string hql = "select dd from DashboardData dd where dd.Site.SitesRegions = :REGION";

            //IQuery query = this.DashboardHibernateDAOFactory.DashboardDataDAO.Session.CreateQuery(hql);

            ////query.SetInt32("REGION", region);

            //return this.DashboardHibernateDAOFactory.DashboardDataDAO.FindByAdhocQuery(query);

            throw new NotImplementedException();
        }

        public IList<DashboardData> FindAll(HeadOffice headOffice)
        {
            throw new NotImplementedException();
        }

        #endregion

        public DashboardData FindBySiteId(int? id)
        {
            const string hql = "select d from DashboardData as d where d.SiteId = :ID";

            var query = this.DashboardHibernateDAOFactory.DashboardDataDAO.Session.CreateQuery(hql);

            query.SetInt32("ID", id.Value);
            query.SetCacheable(true);

            var store = this.DashboardHibernateDAOFactory.DashboardDataDAO.FindFirstElementByAdhocQuery(query);

            return store;
        }
    }
}
