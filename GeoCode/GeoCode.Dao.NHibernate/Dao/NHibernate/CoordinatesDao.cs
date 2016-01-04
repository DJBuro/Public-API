using System;
using System.Collections.Generic;
using GeoCode.Dao.Domain;
using GeoCode.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace GeoCode.Dao.NHibernate
{
    public class CoordinatesDao : HibernateDaoSupport, ICoordinatesDao
    {
        public GeoCodeHibernateDAOFactory GeoCodeHibernateDaoFactory { get; set; }

        #region IGenericDao<Coordinates,int> Members

        public IList<Coordinates> FindAll()
        {
            return this.GeoCodeHibernateDaoFactory.CoordinateDAO.FindAll();
        }

        public Coordinates FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Coordinates Create(Coordinates coordinates)
        {
            return this.GeoCodeHibernateDaoFactory.CoordinateDAO.Create(coordinates);
        }

        public Coordinates Save(Coordinates coordinates)
        {
            return this.GeoCodeHibernateDaoFactory.CoordinateDAO.Save(coordinates);
        }

        public void Update(Coordinates coordinates)
        {
            this.GeoCodeHibernateDaoFactory.CoordinateDAO.Update(coordinates);
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
            this.GeoCodeHibernateDaoFactory.CoordinateDAO.DeleteAll(deleteSet);
        }

        public void Close()
        {
            this.GeoCodeHibernateDaoFactory.CoordinateDAO.Close();
        }

        #endregion
    }
}
