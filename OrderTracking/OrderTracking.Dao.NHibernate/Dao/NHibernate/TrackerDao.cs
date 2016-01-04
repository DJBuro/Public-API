using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class TrackerDao : HibernateDaoSupport, ITrackerDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Tracker,int> Members

        public IList<Tracker> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindAll();
        }

        public Tracker FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindById(id);
        }

        public Tracker Create(Tracker tracker)
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerDAO.Create(tracker);
        }

        public Tracker Save(Tracker tracker)
        {
            return this.OrderTrackingHibernateDaoFactory.TrackerDAO.Save(tracker);
        }

        public void Update(Tracker tracker)
        {
            this.OrderTrackingHibernateDaoFactory.TrackerDAO.Update(tracker);
        }

        public void Delete(Tracker tracker)
        {
            this.OrderTrackingHibernateDaoFactory.TrackerDAO.Delete(tracker);
        }

        public void DeleteAll(Tracker type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Tracker> deleteTrackerSet)
        {
            this.OrderTrackingHibernateDaoFactory.TrackerDAO.DeleteAll(deleteTrackerSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.TrackerDAO.Close();
        }

        #endregion

        #region ITrackerDao Members


        public Tracker FindByName(string name)
        {
            const string hql = "select t from Tracker as t where t.Name = :ID";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);

            query.SetString("ID", name);
            query.SetCacheable(true);

            var tracker = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindFirstElementByAdhocQuery(query);

            return tracker;
        }


        public Tracker FindByName(string name, Store store)
        {
            const string hql = "select t from Tracker as t where t.Name = :ID and t.Store.Id = :STOREID";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);

            query.SetString("ID", name);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var tracker = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindFirstElementByAdhocQuery(query);

            return tracker;
        }


        public IList<Tracker> FindByStore(Store store)
        {
            const string hql = "select trac from Tracker as trac where trac.Store.Id = :STOREID and trac.Active = 1";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);
            query.SetInt64("STOREID", store.Id.Value);
            query.SetCacheable(true);

            var trackers = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindByAdhocQuery(query);

            return trackers;
        }

        public void RemoveTrackerFromDriver(ref Tracker tracker)
        {
            const string hql = "select trac from Tracker as trac where trac.Id = :TRACKERID ";

             var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);
            query.SetInt64("TRACKERID", tracker.Id.Value);
            query.SetCacheable(true);

            var trackers = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindByAdhocQuery(query);

            foreach (var trac in trackers)
            {
                trac.Driver = null;

                this.OrderTrackingHibernateDaoFactory.TrackerDAO.SaveOrUpdate(trac);
            }

            return;
        }

        public void RemoveTrackerFromDriver(ref Driver driver)
        {

            const string hql = "select trac from Tracker as trac where trac.Driver.Id = :DRIVERID ";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);
            query.SetInt64("DRIVERID", driver.Id.Value);
            query.SetCacheable(true);

            var trackers = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindByAdhocQuery(query);

            foreach (var trac in trackers)
            {
                trac.Driver = null;

                this.OrderTrackingHibernateDaoFactory.TrackerDAO.SaveOrUpdate(trac);
            }

            return;
        }

        public Tracker FindByPhoneNumber(string phoneNumber)
        {
            const string hql = "select t from Tracker as t where t.PhoneNumber= :PHONENUMBER";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);

            query.SetString("PHONENUMBER", phoneNumber);
            query.SetCacheable(true);

            var tracker = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindFirstElementByAdhocQuery(query);

            return tracker;
        }



        public void RemoveTrackerFromDriver(Driver driver)
        {
            const string hql = "select trac from Tracker as trac where trac.Driver.Id = :DRIVERID ";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);
            query.SetInt64("DRIVERID", driver.Id.Value);
            query.SetCacheable(true);

            var trackers = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindByAdhocQuery(query);

            foreach (var trac in trackers)
            {
                trac.Driver = null;

                this.OrderTrackingHibernateDaoFactory.TrackerDAO.SaveOrUpdate(trac);
            }

            return;
        }

        public IList<Tracker> FindAll(bool orderByStore)
        {
            if (orderByStore == false)
                return this.FindAll();

            const string hql = "select trac from Tracker as trac order by trac.Store.Id";

            var query = this.OrderTrackingHibernateDaoFactory.TrackerDAO.Session.CreateQuery(hql);
            query.SetCacheable(true);

            var trackers = this.OrderTrackingHibernateDaoFactory.TrackerDAO.FindByAdhocQuery(query);

            return trackers;
        }

        #endregion
    }
}
