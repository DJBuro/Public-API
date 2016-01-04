using System.Collections.Generic;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class LicenseDao : HibernateDaoSupport, ILicenseDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<License,int> Members

        public IList<License> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.LicenseDAO.FindAll();
        }

        public License FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public License Create(License instance)
        {
            throw new System.NotImplementedException();
        }

        public License Save(License instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(License instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(License instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(License type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<License> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
