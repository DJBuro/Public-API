using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class TrackerTypeDao : HibernateDaoSupport, ITrackerTypeDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<TrackerType,int> Members

        public IList<TrackerType> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerTypeDAO.FindAll();
        }

        public TrackerType FindById(int id)
        {
            throw new NotImplementedException();
        }

        public TrackerType Create(TrackerType instance)
        {
            throw new NotImplementedException();
        }

        public TrackerType Save(TrackerType instance)
        {
            throw new NotImplementedException();
        }

        public void Update(TrackerType instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(TrackerType instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(TrackerType type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<TrackerType> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
