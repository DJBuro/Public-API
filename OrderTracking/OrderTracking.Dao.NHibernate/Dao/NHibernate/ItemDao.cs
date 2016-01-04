using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class ItemDao : HibernateDaoSupport, IItemDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<Item,int> Members

        public IList<Item> FindAll()
        {
            throw new NotImplementedException();
        }

        public Item FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Item Create(Item instance)
        {
            throw new NotImplementedException();
        }

        public Item Save(Item item)
        {
            return this.OrderTrackingHibernateDaoFactory.ItemDAO.Save(item);
        }

        public void Update(Item item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Item item)
        {
            this.OrderTrackingHibernateDaoFactory.ItemDAO.Delete(item);
        }

        public void DeleteAll(Item type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<Item> deleteItemSet)
        {
            this.OrderTrackingHibernateDaoFactory.ItemDAO.DeleteAll(deleteItemSet);
        }

        public void Close()
        {
            this.OrderTrackingHibernateDaoFactory.ItemDAO.Close();
        }

        #endregion
    }
}
