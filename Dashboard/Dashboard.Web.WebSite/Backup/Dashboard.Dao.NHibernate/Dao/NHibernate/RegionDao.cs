using System;
using System.Collections.Generic;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class RegionDao : HibernateDaoSupport, IRegionDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<Region,int> Members

        public IList<Region> FindAll()
        {
            return this.DashboardHibernateDAOFactory.RegionDAO.FindAll();
        }

        public Region FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.RegionDAO.FindById(id);
        }

        public Region Create(Region region)
        {
            return this.DashboardHibernateDAOFactory.RegionDAO.Create(region);
        }

        public Region Save(Region region)
        {
            return this.DashboardHibernateDAOFactory.RegionDAO.Save(region);
        }

        public void Update(Region region)
        {
            this.DashboardHibernateDAOFactory.RegionDAO.Update(region);
        }

        public void Delete(Region region)
        {
            this.DashboardHibernateDAOFactory.RegionDAO.Delete(region);
        }

        public void DeleteAll(Region type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Region> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.RegionDAO.Close();
        }

        #endregion

        #region IRegionDao Members

        public Region FindBySite(Site site)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
