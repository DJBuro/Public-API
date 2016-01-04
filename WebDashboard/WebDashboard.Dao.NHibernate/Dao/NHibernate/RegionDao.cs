using System.Collections.Generic;
using NHibernate;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;
using System;

namespace WebDashboard.Dao.NHibernate
{
    public class RegionDao : HibernateDaoSupport, IRegionDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Region,int> Members

        public IList<Region> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.RegionDAO.FindAll();
        }

        public Region FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.RegionDAO.FindById(id);
        }

        public Region Create(Region region)
        {
            return this.WebDashboardHibernateDaoFactory.RegionDAO.Create(region);
        }

        public Region Save(Region region)
        {
            return this.WebDashboardHibernateDaoFactory.RegionDAO.Save(region);
        }

        public void Update(Region region)
        {
            this.WebDashboardHibernateDaoFactory.RegionDAO.Update(region);
        }

        public void Delete(Region region)
        {
            this.WebDashboardHibernateDaoFactory.RegionDAO.Delete(region);
        }

        public void DeleteAll(Region type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Region> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.RegionDAO.Close();
        }

        #endregion

        #region IRegionDao Members

        public IList<Region> FindHeadOffice(HeadOffice headOffice, bool removeDisabledSites)
        {
            const string hql = "select r from Region as r where r.HeadOffice.Id = :HEADOFFICE order by r.Name";

            IQuery query = this.WebDashboardHibernateDaoFactory.RegionDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICE", headOffice.Id.Value);

            query.SetCacheable(true);

            IList<Region> regions = this.WebDashboardHibernateDaoFactory.RegionDAO.FindByAdhocQuery(query);

            foreach (Region region in regions)
            {
                this.CleanupData(region.RegionalSites, removeDisabledSites);
            }

            return regions;
        }

        #endregion

        private void CleanupData(IList<Site> sites, bool removeDisabledSites)
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

            List<Site> removeSites = new List<Site>();

            foreach (Site site in sites)
            {
                if (removeDisabledSites && !site.Enabled)
                {
                    removeSites.Add(site);
                }
                else if (site.LastUpdated < startOfTradingDay)
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
            
            foreach (Site removeSite in removeSites)
            {
                sites.Remove(removeSite);
            }
        }
    }
}
