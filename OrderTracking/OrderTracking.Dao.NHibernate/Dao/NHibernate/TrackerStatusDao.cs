using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class TrackerStatusDao : HibernateDaoSupport, ITrackerStatusDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<TrackerStatus,int> Members

        public IList<TrackerStatus> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerStatusDAO.FindAll();
        }

        public TrackerStatus FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerStatusDAO.FindById(id);
        }

        public TrackerStatus Create(TrackerStatus instance)
        {
            throw new NotImplementedException();
        }

        public TrackerStatus Save(TrackerStatus instance)
        {
            throw new NotImplementedException();
        }

        public void Update(TrackerStatus instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(TrackerStatus instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(TrackerStatus type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<TrackerStatus> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.TrackerStatusDAO.Close();
        }

        #endregion
    }
}
