using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;
using System;

namespace WebDashboard.Dao.NHibernate
{
    public class HistoricalDataDao : HibernateDaoSupport, IHistoricalDataDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<HistoricalData,int> Members

        public IList<HistoricalData> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public HistoricalData FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public HistoricalData Create(HistoricalData instance)
        {
            return this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Create(instance);
        }

        public HistoricalData Save(HistoricalData instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(HistoricalData instance)
        {
            this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Update(instance);
        }

        public void Delete(HistoricalData instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(HistoricalData type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<HistoricalData> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            WebDashboardHibernateDaoFactory.HistoricalDataDAO.Close();
        }

        #endregion

        #region IHistoricalDataDao Members

        public IList<HistoricalData> FindByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select hd from HistoricalData as hd where hd.HeadOffice.Id =:HEADOFFICEID";

            var query = this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICEID", headOffice.Id.Value);

            return this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.FindByAdhocQuery(query);
        }

        public HistoricalData FindBySiteIdAndTradingDay(int siteId, DateTime tradingDay)
        {
            const string hql = "select hd from HistoricalData as hd where hd.SiteId =:SITEID and hd.TradingDay = :TRADINGDAY";

            var query = this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Session.CreateQuery(hql);

            query.SetInt32("SITEID", siteId);
            query.SetDateTime("TRADINGDAY", tradingDay);

            return this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.FindFirstElementByAdhocQuery(query);
        }

        public IDictionary<int, HistoricalData> FindByHeadOfficeIdAndTradingDay(int headOfficeId, DateTime tradingDay)
        {
            const string hql = "select hd from HistoricalData as hd where hd.HeadOffice.Id =:HEADOFFICEID and hd.TradingDay = :TRADINGDAY";

            var query = this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICEID", headOfficeId);
            query.SetDateTime("TRADINGDAY", tradingDay);

            Dictionary<int, HistoricalData> historicDataBySiteId = new Dictionary<int,HistoricalData>();
            IList<HistoricalData> historicData = this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.FindByAdhocQuery(query);

            if (historicData != null)
            {
                foreach (HistoricalData historicDataItem in historicData)
                {
                    historicDataBySiteId.Add(historicDataItem.SiteId, historicDataItem);
                }
            }

            return historicDataBySiteId;
        }

        #endregion


        public IList<string> GetHistoricalDatesByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select distinct hd.TradingDay from HistoricalData as hd where hd.HeadOffice.Id =:HEADOFFICEID and hd.TradingDay is not null";

            var query = this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.Session.CreateQuery(hql);

            query.SetInt32("HEADOFFICEID", headOffice.Id.Value);

            return this.WebDashboardHibernateDaoFactory.HistoricalDataDAO.FindByAdhocQueryDateString(query);
        }
    }
}
