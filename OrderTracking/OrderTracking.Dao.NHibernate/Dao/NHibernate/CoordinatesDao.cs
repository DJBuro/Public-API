using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class CoordinatesDao : HibernateDaoSupport, ICoordinatesDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }


        #region IGenericDao<Coordinates,int> Members

        public IList<Coordinates> FindAll()
        {
            throw new NotImplementedException();
        }

        public Coordinates FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Coordinates Create(Coordinates coordinates)
        {
            return this.OrderTrackingHibernateDaoFactory.CoordinateDAO.Create(coordinates);
        }

        public Coordinates Save(Coordinates coordinates)
        {
            return this.OrderTrackingHibernateDaoFactory.CoordinateDAO.Save(coordinates);
        }

        public void Update(Coordinates coordinates)
        {
            this.OrderTrackingHibernateDaoFactory.CoordinateDAO.Update(coordinates);
        }

        public void Delete(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(Coordinates type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Coordinates> deleteSet)
        {
            this.OrderTrackingHibernateDaoFactory.CoordinateDAO.DeleteAll(deleteSet);
        }

        public void Close()
        {
           this.OrderTrackingHibernateDaoFactory.CoordinateDAO.Close();
        }

        #endregion
    }
}
