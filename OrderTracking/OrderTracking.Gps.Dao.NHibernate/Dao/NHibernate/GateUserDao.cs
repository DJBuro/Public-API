using System.Collections.Generic;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class GateUserDao : HibernateDaoSupport, IGateUserDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }


        #region IGenericDao<GateUser,int> Members

        public IList<GateUser> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.GateuserDAO.FindAll();
        }

        public GateUser FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.GateuserDAO.FindById(id);
        }

        public GateUser Create(GateUser gateUser)
        {
            return this.OrderTrackingHibernateDaoFactory.GateuserDAO.Create(gateUser);
        }

        public GateUser Save(GateUser gateUser)
        {
            throw new System.NotImplementedException();
        }

        public void Update(GateUser gateUser)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(GateUser gateUser)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(GateUser type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<GateUser> deleteSet)
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
