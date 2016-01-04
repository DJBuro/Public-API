using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    class SiteDao : HibernateDaoSupport, ISiteDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Site,int> Members

        public IList<Site> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindAll();
        }

        public Site FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindById(id);
        }

        public Site Create(Site site)
        {
            if (site.LastUpdated == null)
                site.LastUpdated = DateTime.Now;
            return this.WebDashboardHibernateDaoFactory.SiteDAO.Create(site);
        }

        public Site Save(Site site)
        {
            if (site.LastUpdated == null)
                site.LastUpdated = DateTime.Now;
            return this.WebDashboardHibernateDaoFactory.SiteDAO.Save(site);
        }

        public void Update(Site site)
        {
            if (site.LastUpdated == null)
                site.LastUpdated = DateTime.Now;
            this.WebDashboardHibernateDaoFactory.SiteDAO.Update(site);
        }

        public void Delete(Site site)
        {
            this.WebDashboardHibernateDaoFactory.SiteDAO.Delete(site);
        }

        public void DeleteAll(Site type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Site> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.SiteDAO.Close();
        }

        #endregion

        #region ISiteDao Members

        public Site FindByIp(string ipAddress)
        {
            const string hql = "select s from Site as s where s.IPAddress = :IPADDRESS and s.Enabled = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetString("IPADDRESS", ipAddress);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindFirstElementByAdhocQuery(query);
        }

        public IList<Site> FindAllActiveSitesByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select s from Site as s where s.HeadOffice.Id = :HEADOFFICE and s.SiteType.Id = 1 and s.Enabled = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICE", headOffice.Id.Value);
            query.SetCacheable(true);

#if Debug
            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query);
#endif


            var sites = this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query); //.Where(c => c.LastUpdated.Value.Date.DayOfYear == DateTime.Now.Date.DayOfYear);

            //var afterSixAmData = from k in today where (DateTime.Compare(k.LastUpdated.Value.Date , Convert.ToDateTime("06:00:00 AM")) < 0) select k;

            this.CleanupData(sites);

            return sites.ToList();


        }

        public IList FindAllActiveSitesByRegion(Region region)
        {

            const string hql = "select s from Site as s where s.Region.Id = :REGION and s.SiteType.Id = 1 and s.Enabled = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("REGION", region.Id.Value);
            query.SetCacheable(true);

#if Debug
            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query);
#endif


            var sites = this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query); //.Where(c => c.LastUpdated.Value.Date.DayOfYear == DateTime.Now.Date.DayOfYear);

            //var afterSixAmData = from k in today where (DateTime.Compare(k.LastUpdated.Value.Date, Convert.ToDateTime("06:00:00 AM")) < 0) select k;

            this.CleanupData(sites);

            return sites.ToList();
        }


        public IList<Site> FindAllActiveSitesByRegionIList(Region region)
        {
            const string hql = "select s from Site as s where s.Region.Id = :REGION and s.SiteType.Id = 1 and s.Enabled = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("REGION", region.Id.Value);
            query.SetCacheable(true);

            #if Debug
            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query);
            #endif


            var sites = this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query); //.Where(c => c.LastUpdated.Value.Date.DayOfYear == DateTime.Now.Date.DayOfYear);

//            var afterSixAmData = from k in today where (DateTime.Compare(k.LastUpdated.Value.Date, Convert.ToDateTime("06:00:00 AM")) < 0) select k;

            this.CleanupData(sites);

            return sites.ToList();
        }

        /// <summary>
        /// TODO: make sure site keys are unique in db!
        /// </summary>
        /// <param name="siteKey"></param>
        /// <returns></returns>
        public Site FindBySiteKey(int siteKey)
        {
            const string hql = "select s from Site as s where s.Key = :SITEKEY and s.SiteType.Id = 1 and s.Enabled = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("SITEKEY", siteKey);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindFirstElementByAdhocQuery(query);
        }

        public IList<Site> FindAllRegion(Region region)
        {
            const string hql = "select s from Site as s where s.Region.Id = :REGION and s.SiteType.Id = 1";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("REGION", region.Id.Value);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query);
        }


        public Site FindByRamesesId(int? ramesesId)
        {
            const string hql = "select s from Site as s where s.SiteId = :RAMESESID";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("RAMESESID", ramesesId.Value);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.SiteDAO.FindFirstElementByAdhocQuery(query);
        }


        public IList<Site> FindAllSitesByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select s from Site as s where s.HeadOffice.Id = :HEADOFFICE and s.SiteType.Id = 1 order by s.Name asc";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICE", headOffice.Id.Value);
            query.SetCacheable(true);

            var sites = this.WebDashboardHibernateDaoFactory.SiteDAO.FindByAdhocQuery(query);

            this.CleanupData(sites);
            
            return sites;
        }

        #endregion

        private void CleanupData(IList<Site> sites)
        {
            DateTime dateTimeNow = DateTime.Now;

            // Trading day is 6:30am to 6:30am the next day
            // Only use data that was uploaded during the current trading day
            DateTime startOfTradingDay = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 7, 0, 0);

            // Before 6:30am is technically the previous trading day so 
            // we only use data uploaded after 6:30am the previous day
            if (dateTimeNow.Hour < 7) // || (dateTimeNow.Hour == 6 && dateTimeNow.Minute < 30))
            {
                startOfTradingDay = startOfTradingDay.AddDays(-1);
            }

            foreach (Site site in sites)
            {
                if (site.LastUpdated < startOfTradingDay)
                {
                    site.Column1 = 0;
                    site.Column2 = 0;
                    site.Column3 = 0;
                    site.Column4 = 0;
                    site.Column5 = 0;
                    site.Column6 = 0;
                    site.Column7 = 0;
                    site.Column8 = 0;
                    site.Column9 = 0;
                    site.Column10 = 0;
                    site.Column11 = 0;
                    site.Column12 = 0;
                    site.Column13 = 100; //Orders per delivery driver is always 100 for some reason - average?
                    site.Column14 = 0;
                    site.Column15 = 0;
                    site.Column16 = 0;
                    site.Column17 = 0;
                    site.Column18 = 0;
                    site.Column19 = 0;
                    site.Column20 = 0;

                }
            }
        }
    }
}
