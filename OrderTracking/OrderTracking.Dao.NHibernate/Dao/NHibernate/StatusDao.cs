using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class StatusDao : HibernateDaoSupport, IStatusDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Status,int> Members

        public IList<Status> FindAll()
        {
            throw new NotImplementedException();
        }

        public Status FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.StatusDAO.FindById(id);
        }

        public Status Create(Status instance)
        {
            throw new NotImplementedException();
        }

        public Status Save(Status instance)
        {
            throw new NotImplementedException();
        }

        public void Update(Status instance)
        {
            throw new NotImplementedException();
        }

        public void Delete(Status instance)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Status type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Status> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.StatusDAO.Close();
        }

        #endregion
    }
}
