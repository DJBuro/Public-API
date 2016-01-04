using System.Collections.Generic;
using NHibernate;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class DeviceDao : HibernateDaoSupport, IDeviceDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Device,int> Members

        public IList<Device> FindAll()
        {
            return OrderTrackingHibernateDaoFactory.DeviceDAO.FindAll();
        }

        public Device FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Device Create(Device device)
        {
            return this.OrderTrackingHibernateDaoFactory.DeviceDAO.Create(device);
        }

        public Device Save(Device instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Device device)
        {
            this.OrderTrackingHibernateDaoFactory.DeviceDAO.Update(device);
        }

        public void Delete(Device instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(Device type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Device> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IDeviceDao Members

        public Device FindByName(string name)
        {
            const string hql = "select d from Device as d where d.IMEI = :IMEI";

            var query = this.OrderTrackingHibernateDaoFactory.DeviceDAO.Session.CreateQuery(hql);

            query.SetString("IMEI", name);

            return this.OrderTrackingHibernateDaoFactory.DeviceDAO.FindFirstElementByAdhocQuery(query);
        }

        #endregion
    }
}
