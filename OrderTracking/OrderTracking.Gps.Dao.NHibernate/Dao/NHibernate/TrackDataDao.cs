using System.Collections.Generic;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class TrackDataDao : HibernateDaoSupport, ITrackDataDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Trackdata,int> Members

        public IList<Trackdata> FindAll()
        {
            return OrderTrackingHibernateDaoFactory.TrackdataDAO.FindAll();
        }

        public Trackdata FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Trackdata Create(Trackdata instance)
        {
            throw new System.NotImplementedException();
        }

        public Trackdata Save(Trackdata instance)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Trackdata instance)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Trackdata instance)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(Trackdata type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<Trackdata> deleteSet)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ITrackDataDao Members

        public string ReturnTest()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
