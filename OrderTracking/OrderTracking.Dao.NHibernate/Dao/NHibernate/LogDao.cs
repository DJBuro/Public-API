using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class LogDao : HibernateDaoSupport, ILogDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Log,int> Members

        public IList<Log> FindAll()
        {
            throw new NotImplementedException();
        }

        public Log FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Log Create(Log instance)
        {
            throw new NotImplementedException();
        }

        public Log Save(Log instance)
        {
            throw new NotImplementedException();
        }

        public void Update(Log instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Log instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Log type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Log> deleteSet)
        {
            this.OrderTrackingHibernateDaoFactory.LogDAO.DeleteAll(deleteSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.LogDAO.Close();
        }

        #endregion

        #region ILogDao Members

        public IList<Log> Last20Logs(string externalStoreId)
        {
            const string hql = "select log from Log as log where log.StoreId = :STOREID ";

            var query = this.OrderTrackingHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);
            query.SetString("STOREID", externalStoreId);

            var logs = this.OrderTrackingHibernateDaoFactory.LogDAO.FindByAdhocQuery(query).Reverse().ToList();

            return logs;
        }


        public IList<Log> TodaysLogs(string externalStoreId)
        {
            const string hql = "select log from Log as log where log.StoreId = :STOREID ";

            var query = this.OrderTrackingHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);
            query.SetString("STOREID", externalStoreId);

            var logs = this.OrderTrackingHibernateDaoFactory.LogDAO.FindByAdhocQuery(query).Reverse().Where(c => c.Created.Value.Date.DayOfYear == DateTime.Now.Date.DayOfYear).ToList();

           var afterSixAmData = from k in logs where (DateTime.Compare(k.Created.Value.Date, Convert.ToDateTime("06:00:00 AM")) < 0) select k;

           return afterSixAmData.ToList();
        }

        public IList<Log> WeeksLogs(string externalStoreId)
        {
            const string hql = "select log from Log as log where log.StoreId = :STOREID ";

            var query = this.OrderTrackingHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);
            query.SetString("STOREID", externalStoreId);

            var logs = this.OrderTrackingHibernateDaoFactory.LogDAO.FindByAdhocQuery(query).Reverse().ToList();


            var weeksLogs = from t in logs
                            where t.Created.Value.Date > DateTime.Today.AddDays(-7)
                            select t;



            return weeksLogs.ToList();
        }

        /// <summary>
        /// Global logs, not assigned to any site
        /// </summary>
        /// <returns></returns>
        public IList<Log> GlobalLogs()
        {
            //wierd...
            const string hql = "select log from Log as log where log.StoreId = '(null)' or log.StoreId = ''";

            var query = this.OrderTrackingHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);

            var logs = this.OrderTrackingHibernateDaoFactory.LogDAO.FindByAdhocQuery(query).Reverse().ToList();

            var lastWeeksGlobal = from k in logs where (k.Created.Value.Date > DateTime.Today.AddDays(-7)) select k;

            return lastWeeksGlobal.ToList();

        }

        public IList<Log> AllAccountLogs(string externalStoreId)
        {
            const string hql = "select log from Log as log where log.StoreId = :STOREID ";

            var query = this.OrderTrackingHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);
            query.SetString("STOREID", externalStoreId);

            var logs = this.OrderTrackingHibernateDaoFactory.LogDAO.FindByAdhocQuery(query);

            return logs;
        }

        #endregion
    }
}
