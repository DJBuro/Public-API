using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class DriverOrderDao : HibernateDaoSupport, IDriverOrderDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<DriverOrder,int> Members

        public IList<DriverOrder> FindAll()
        {
            throw new NotImplementedException();
        }

        public DriverOrder FindById(int id)
        {
            throw new NotImplementedException();
        }

        public DriverOrder Create(DriverOrder driverOrder)
        {
            throw new NotImplementedException();
        }

        public DriverOrder Save(DriverOrder driverOrder)
        {
            return this.OrderTrackingHibernateDaoFactory.DriverOrderDAO.Save(driverOrder);
        }

        public void Update(DriverOrder driverOrder)
        {
            throw new NotImplementedException();
        }

        public void Delete(DriverOrder driverOrder)
        {
            this.OrderTrackingHibernateDaoFactory.DriverOrderDAO.Delete(driverOrder);
        }

        public void DeleteAll(DriverOrder driverOrderType)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<DriverOrder> deleteDriverOrderSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.DriverOrderDAO.Close();
        }

        #endregion
    }
}
