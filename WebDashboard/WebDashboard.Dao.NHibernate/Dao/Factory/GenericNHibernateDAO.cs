using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using Spring.Data.NHibernate;
using NHibISessionFactory = NHibernate.ISessionFactory;

namespace WebDashboard.Dao.NHibernate.Dao.Factory
{
    public interface IGenericNHibernateDAO<T, ID> : IGenericDao<T, ID>
    {
        NHibISessionFactory NHibernateSessionFactory { get; set; }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        ISession Session { get; }

        ICriteria GetCriteria();
        IList<T> FindByAdhocQuery(IQuery q);
        IList<string> FindByAdhocQueryDateString(IQuery q);
        T FindFirstElementByAdhocQuery(IQuery q);
        IList<T> FindByCriteria(ICriteria criteria);
        T FindFirstElementByCriteria(ICriteria criteria);
        IList<object> FindByGroupedQuery(IQuery query);
    }

    public abstract class GenericNHibernateDAO<T, ID> : IGenericNHibernateDAO<T, ID>
    {
        public NHibISessionFactory NHibernateSessionFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_springManager"></param>
        public GenericNHibernateDAO(NHibISessionFactory _springManager)
        {
            if (_springManager == null)
                throw new ArgumentNullException("NHibernate.ISessionFactory cannot be null");

            this.NHibernateSessionFactory = _springManager;
        }


        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        public virtual ISession Session
        {
            get
            {
                ISession mySession = SessionFactoryUtils.DoGetSession(NHibernateSessionFactory, true);
              //  ISession mySession = NHibernateSessionFactory.GetCurrentSession();
                mySession.FlushMode = FlushMode.Commit;
                return mySession;
            }
        }

        public ICriteria GetCriteria()
        {
            return Session.CreateCriteria(typeof(T));
        }

        private IList<T> ConvertIListToGenerics(IList list)
        {
            IList<T> result = new List<T>();
            foreach (T t in list)
            {
                result.Add(t);
            }
            return result;
        }

        public IList<T> FindAll()
        {
            ICriteria crit = Session.CreateCriteria(typeof(T));
            return ConvertIListToGenerics(crit.List());
        }

        public IList<T> FindByAdhocQuery(IQuery query)
        {
            return ConvertIListToGenerics(query.List());
        }

        public IList<string> FindByAdhocQueryDateString(IQuery query)
        {
            IList<string> result = new List<string>();
            foreach (DateTime t in query.List())
            {
                if (t != null)
                    result.Add(t.ToString("yyyy-MM-dd"));
            }
            return result;
        }

        public IList<object> FindByGroupedQuery(IQuery query)
        {
            var blah = new List<object>();

            IList<string> result = new List<string>();
            foreach (Object[] t in query.List())
            {
                blah.Add(t);
            }

            return blah;
        }

        public T FindFirstElementByAdhocQuery(IQuery query)
        {
            IList<T> result = FindByAdhocQuery(query);
            return GetFirstElement(result);
        }


        public IList<T> FindByCriteria(ICriteria criteria)
        {
            return ConvertIListToGenerics(criteria.List());
        }

        public T FindFirstElementByCriteria(ICriteria criteria)
        {
            IList<T> result = FindByCriteria(criteria);
            return GetFirstElement(result);
        }

        private static T GetFirstElement(IList<T> param)
        {
            if (param == null || param.Count == 0) return default(T);
            return param[0];
        }

        public T FindById(ID id)
        {
            try
            {
                if (Session != null)
                {
                    return (T)Session.Load(typeof(T), id);
                }
                else
                {
                    return default(T);
                }
            }
            catch (ObjectNotFoundException)
            {
                return default(T);
            }
        }

        public T Create(T instance)
        {
            Session.Clear();
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            Session.Save((T)instance);
            tr.Commit();
            return instance;
        }

        public T Save(T instance)
        {
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            Session.Save(instance);
            tr.Commit();
            return instance;
        }

        private T SaveWithClear(T instance)
        {
            return SaveOrUpdate(instance, true);
        }


        private T SaveOrUpdate(T instance)
        {
            return SaveOrUpdate(instance, false);
        }

        public void Update(T instance)
        {
            Session.Clear();
            ITransaction tr = Session.BeginTransaction();
            
            tr.Begin();
            Session.Update(instance);
            tr.Commit();
        }

        private T SaveOrUpdate(T instance, bool clearSession)
        {
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            if (clearSession) Session.Clear();
            Session.SaveOrUpdate(instance);
            tr.Commit();
            return instance;
        }


        public void DeleteAll(T type)
        {
            Session.Clear();
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            Session.Delete(criteria);
            tr.Commit();
        }

        public void DeleteAll(IList<T> deleteSet)
        {
            Session.Clear();
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            foreach (T t in deleteSet)
            {
                Session.Delete(t);
            }
            tr.Commit();
        }

        public void Delete(T instance)
        {
            Session.Clear();
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            Session.Delete(instance);
            tr.Commit();
        }

        public void Delete(ICriteria criteria)
        {
            Session.Clear();
            ITransaction tr = Session.BeginTransaction();
            tr.Begin();
            IList<T> deletes = this.FindByCriteria(criteria);
            DeleteAll(deletes);
        }

        public void Close()
        { 
            //todo: test this...
            NHibernateSessionFactory.Close();
            
        }
    }
}
