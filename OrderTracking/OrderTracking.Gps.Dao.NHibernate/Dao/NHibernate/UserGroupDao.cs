using System.Collections.Generic;
using NHibernate;
using OrderTracking.Gps.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Gps.Dao.Domain;

namespace OrderTracking.Gps.Dao.NHibernate
{
    public class UserGroupDao : HibernateDaoSupport, IUserGroupDao
    {
        public OrderTrackingGpsHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<UserGroup,int> Members

        public IList<UserGroup> FindAll()
        {
            return this.OrderTrackingHibernateDaoFactory.UserGroupDAO.FindAll();
        }

        public UserGroup FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public UserGroup Create(UserGroup userGroup)
        {
            return this.OrderTrackingHibernateDaoFactory.UserGroupDAO.Create(userGroup);
        }

        public UserGroup Save(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(UserGroup type)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAll(IList<UserGroup> deleteSet)
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
