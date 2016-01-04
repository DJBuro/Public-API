using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class ApnDao : HibernateDaoSupport, IApnDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Apn,int> Members

        public IList<Apn> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.ApnDAO.FindAll();
        }

        public Apn FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.ApnDAO.FindById(id);
        }

        public Apn Create(Apn apn)
        {
            return this.OrderTrackingHibernateDaoFactory.ApnDAO.Create(apn);
        }

        public Apn Save(Apn apn)
        {
            return this.OrderTrackingHibernateDaoFactory.ApnDAO.Save(apn);
        }

        public void Update(Apn apn)
        {
            this.OrderTrackingHibernateDaoFactory.ApnDAO.Update(apn);
        }

        public void Delete(Apn apn)
        {
            this.OrderTrackingHibernateDaoFactory.ApnDAO.Delete(apn);
        }

        public void DeleteAll(Apn type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Apn> deleteSet)
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
