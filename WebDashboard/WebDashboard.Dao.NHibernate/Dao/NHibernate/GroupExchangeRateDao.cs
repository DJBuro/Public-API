using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class GroupExchangeRateDao : HibernateDaoSupport, IGroupExchangeRateDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        public IList<GroupExchangeRate> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.FindAll();
        }

        public GroupExchangeRate FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.FindById(id);
        }

        public GroupExchangeRate Create(GroupExchangeRate groupExchangeRate)
        {
            return this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Create(groupExchangeRate);
        }

        public GroupExchangeRate Save(GroupExchangeRate groupExchangeRate)
        {
            return this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Save(groupExchangeRate);
        }

        public void Update(GroupExchangeRate groupExchangeRate)
        {
            this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Update(groupExchangeRate);
        }

        public void Delete(GroupExchangeRate groupExchangeRate)
        {
            this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Delete(groupExchangeRate);
        }

        public void DeleteAll(GroupExchangeRate groupExchangeRate)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<GroupExchangeRate> groupExchangeRates)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Close();
        }

        public IList<GroupExchangeRate> FindByGroupId(int groupId)
        {
            //const string hql = "select ger from GroupExchangeRate as ger where ger.GroupId = :GROUPID";

            //var query = this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.Session.CreateQuery(hql);

            //query.SetInt32("GROUPID", groupId);

            //query.SetCacheable(true);

            //return this.WebDashboardHibernateDaoFactory.GroupExchangeRateDAO.FindByAdhocQuery(query);

            List<GroupExchangeRate> groupExchangeRates = new List<GroupExchangeRate>();
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "EUR", 1.29906f));
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "GBP", 1.5453f));
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "RUB", 0.0327281f));
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "MAD", 0.116913f));
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "INR", 0.0186168f));
            groupExchangeRates.Add(new GroupExchangeRate(groupId, "USD", 1f));
            return groupExchangeRates;
        }
    }
}
