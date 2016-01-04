using System.Collections.Generic;
using Spring.Data.NHibernate.Support;
using WebDashboard.Dao.Domain;
using WebDashboard.DAO.Factory;

namespace WebDashboard.Dao.NHibernate
{
    public class HeadOfficeDao: HibernateDaoSupport, IHeadOfficeDao
    {
        public WebDashboardHibernateDAOFactory WebDashboardHibernateDaoFactory { get; set; }

        #region IGenericDao<HeadOffice,int> Members

        public IList<HeadOffice> FindAll()
        {
            return this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.FindAll();
        }

        public HeadOffice FindById(int id)
        {
            return this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.FindById(id);
        }

        public HeadOffice Create(HeadOffice headOffice)
        {
            return this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.Create(headOffice);
        }

        public HeadOffice Save(HeadOffice headOffice)
        {
            return this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.Save(headOffice);
        }

        public void Update(HeadOffice headOffice)
        {
            this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.Update(headOffice);
        }

        public void Delete(HeadOffice headOffice)
        {
            this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.Delete(headOffice);
        }

        public void DeleteAll(HeadOffice headOffice)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<HeadOffice> headOffices)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            this.WebDashboardHibernateDaoFactory.HeadOfficeDAO.Close();
        }

        #endregion
    }
}
