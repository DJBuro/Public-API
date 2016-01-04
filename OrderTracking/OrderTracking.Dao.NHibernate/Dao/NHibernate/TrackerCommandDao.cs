using System;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class TrackerCommandDao : HibernateDaoSupport, ITrackerCommandDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<TrackerCommand,int> Members

        public IList<TrackerCommand> FindAll()
        {
            throw new NotImplementedException();
        }

        public TrackerCommand FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerCommandDAO.FindById(id);
        }

        public TrackerCommand Create(TrackerCommand instance)
        {
            throw new NotImplementedException();
        }

        public TrackerCommand Save(TrackerCommand instance)
        {
            throw new NotImplementedException();
        }

        public void Update(TrackerCommand instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(TrackerCommand instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(TrackerCommand type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<TrackerCommand> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ITrackerCommandDao Members

        public IList<TrackerCommand> FindAllSetup(Tracker tracker)
        {
            //var tc = new TrackerCommand();

            const string hql = "select tc from TrackerCommand as tc where tc.TrackerType.Id=:TRACKERTYPEID and tc.CommandType.Id=1 order by tc.Priority asc";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerCommandDAO.Session.CreateQuery(hql);

            query.SetInt64("TRACKERTYPEID", tracker.Type.Id.Value);
            query.SetCacheable(true);

            return this.OrderTrackingHibernateDaoFactory.TrackerCommandDAO.FindByAdhocQuery(query);
        }

        #endregion
    }
}
