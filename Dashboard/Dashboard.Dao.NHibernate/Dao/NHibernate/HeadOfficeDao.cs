using System;
using System.Collections.Generic;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;

namespace Dashboard.Dao.NHibernate
{
    public class HeadOfficeDao : HibernateDaoSupport, IHeadOfficeDao
    {
        public DashboardHibernateDAOFactory DashboardHibernateDAOFactory { get; set; }

        #region IGenericDao<HeadOffice,int> Members

        public IList<HeadOffice> FindAll()
        {
            return this.DashboardHibernateDAOFactory.HeadOfficeDAO.FindAll();
        }

        public HeadOffice FindById(int id)
        {
            return this.DashboardHibernateDAOFactory.HeadOfficeDAO.FindById(id);
        }

        public HeadOffice Create(HeadOffice headOffice)
        {
            return this.DashboardHibernateDAOFactory.HeadOfficeDAO.Create(headOffice);
        }

        public HeadOffice Save(HeadOffice headOffice)
        {
            return this.DashboardHibernateDAOFactory.HeadOfficeDAO.Save(headOffice);
        }

        public void Update(HeadOffice headOffice)
        {
            this.DashboardHibernateDAOFactory.HeadOfficeDAO.Update(headOffice);
        }

        public void Delete(HeadOffice headOffice)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(HeadOffice type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<HeadOffice> deleteSet)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            this.DashboardHibernateDAOFactory.HeadOfficeDAO.Close();
        }

        #endregion

    }
}
