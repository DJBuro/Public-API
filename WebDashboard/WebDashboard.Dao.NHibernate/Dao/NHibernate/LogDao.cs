using System;
using System.Collections.Generic;
using System.Linq;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class LogDao : HibernateDaoSupport, ILogDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Log,int> Members

        public IList<Log> FindAll()
        {
            return WebDashboardHibernateDaoFactory.LogDAO.FindAll();
        }

        public Log FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Log Create(Log instance)
        {
            throw new System.NotImplementedException();
        }

        public Log Save(Log instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Log instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Log instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(Log type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Log> deleteSet)
        {
            WebDashboardHibernateDaoFactory.LogDAO.DeleteAll(deleteSet);
        }

        public void Close()
        {
            WebDashboardHibernateDaoFactory.LogDAO.Close();
        }

        #endregion

        #region ILogDao Members

        public IList<Log> FindAllGrouped()
        {
            //var log = new Log();
            const string hql = "select count(log.Id), log.StoreId, log.Severity, log.Message, log.Method, log.Source  from Log as log group by log.StoreId, log.Severity, log.Message, log.Method, log.Source";

            var query = this.WebDashboardHibernateDaoFactory.LogDAO.Session.CreateQuery(hql);

            var logs = this.WebDashboardHibernateDaoFactory.LogDAO.FindByGroupedQuery(query);

            var groupedLogs = new List<Log>();

            foreach (object[] o in logs)
            {
                groupedLogs.Add(new Log(o[0].ToString(), o[1].ToString(), o[2].ToString(), o[3].ToString(), o[4].ToString(), o[5].ToString(), DateTime.Now));
            }

            return groupedLogs;
        }

        #endregion
    }
}
