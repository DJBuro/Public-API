using System.Collections.Generic;
using System.Linq;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class IndicatorDao : HibernateDaoSupport, IIndicatorDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<Indicator,int> Members

        public IList<Indicator> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.IndicatorDAO.FindAll();
        }

        public Indicator FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.IndicatorDAO.FindById(id);
        }

        public Indicator Create(Indicator instance)
        {
            throw new System.NotImplementedException();
        }

        public Indicator Save(Indicator indicator)
        {
            return this.WebDashboardHibernateDaoFactory.IndicatorDAO.Save(indicator);
        }

        public void Update(Indicator indicator)
        {
            this.WebDashboardHibernateDaoFactory.IndicatorDAO.Update(indicator);
        }

        public void Delete(Indicator indicator)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(Indicator type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(System.Collections.Generic.IList<Indicator> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.IndicatorDAO.Close();
        }

        #endregion

        #region IIndicatorDao Members

        public IList<Indicator> GetIndictorsByHeadOffice(HeadOffice headOffice)
        {
            const string hql = "select i from Indicator as i where i.HeadOffice.Id = :HOID";

            var query = this.WebDashboardHibernateDaoFactory.SiteDAO.Session.CreateQuery(hql);

            query.SetInt32("HOID", headOffice.Id.Value);

            query.SetCacheable(true);

            return this.WebDashboardHibernateDaoFactory.IndicatorDAO.FindByAdhocQuery(query); ;

            /*
            return this.WebDashboardHibernateDaoFactory.IndicatorDAO
                .FindAll()
                .Where(c => c.HeadOffice.Id.Value == headOffice.Id.Value)
                .ToList(); 
            */
        }

        #endregion
    }
}
