using System;
using System.Collections.Generic;
using OrderTracking.Dao.NHibernate.Dao.Factory;
using Spring.Data.NHibernate.Support;
using OrderTracking.Dao.Domain;

namespace OrderTracking.Dao.NHibernate
{
    public class SmsCredentialDao : HibernateDaoSupport, ISmsCredentialsDao
    {
        public OrderTrackingHibernateDAOFactory OrderTrackingHibernateDaoFactory { get; set; }

        #region IGenericDao<SmsCredential,int> Members

        public IList<SmsCredential> FindAll()
        {
            throw new NotImplementedException();
        }

        public SmsCredential FindById(int id)
        {
            return this.OrderTrackingHibernateDaoFactory.SmsCredentialDAO.FindById(id);
        }

        public SmsCredential Create(SmsCredential smsCredential)
        {
            throw new NotImplementedException();
        }

        public SmsCredential Save(SmsCredential smsCredential)
        {
            return this.OrderTrackingHibernateDaoFactory.SmsCredentialDAO.Save(smsCredential);
        }

        public void Update(SmsCredential smsCredential)
        {
            this.OrderTrackingHibernateDaoFactory.SmsCredentialDAO.Update(smsCredential);
        }

        public void Delete(SmsCredential smsCredential)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(SmsCredential type)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(IList<SmsCredential> deleteSet)
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
