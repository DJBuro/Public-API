using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using OrderTracking.Gps.Dao;
using NHibISessionFactory = NHibernate.ISessionFactory;

namespace OrderTracking.Gps.Dao.NHibernate.Dao.Factory
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
        T FindFirstElementByAdhocQuery(IQuery q);
        IList<T> FindByCriteria(ICriteria criteria);
        T FindFirstElementByCriteria(ICriteria criteria);

        //PagedList<T> FindAllWithPaging(PagingData pagingData);
        //PagedList<T> FindByCriteriaWithPaging(ICriteria criteria, PagingData pagingData);
        //PagedQuery<T> CreatePagedQuery(String hql, PagingData pagingData);
        //PagedList<T> FindByAdhocQueryWithPaging(PagedQuery<T> q);


        T SaveOrUpdate(T instance);
        //void Delete(ICriteria criteria);
        //void DeleteAll(IList<T> deleteSet);
        //T SaveWithClear(T instance);
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
                ISession mySession = NHibernateSessionFactory.GetCurrentSession();
                mySession.FlushMode = FlushMode.Commit;
                return mySession;
            }
        }

        public ICriteria GetCriteria()
        {
            return Session.CreateCriteria(typeof(T));
        }

        private static IList<T> ConvertIListToGenerics(IList list)
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
            //todo:test, currently works
            //ICriteria crit = Session.CreateCriteria(typeof(T)).SetCacheable(true);
            ICriteria crit = Session.CreateCriteria(typeof(T));

            return ConvertIListToGenerics(crit.List());
        }

        public IList<T> FindByAdhocQuery(IQuery query)
        {
            return ConvertIListToGenerics(query.List());
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
                return (T)Session.Load(typeof(T), id);
            }
            catch (ObjectNotFoundException)
            {
                return default(T);
            }
        }

        #region Paging
        /*
        public PagedList<T> FindAllWithPaging(PagingData pagingData)
        {
            return FindByCriteriaWithPaging(GetCriteria(), pagingData);
        }

        public PagedList<T> FindByCriteriaWithPaging(ICriteria criteria, PagingData pagingData)
        {
            Asserts.ArgumentExists(criteria, "criteria");

            //todo: paging fix implement? needed?
            //examine the sort property and add order, if necessary
            //if (pagingData.SortProperty.Length > 0)
            //{
            //    switch (pagingData.SortPropertyDirection)
            //    {
            //        case PagingSortDirection.Asc:
            //           criteria.AddOrder(Order.Asc(pagingData.SortProperty));
            //            break;

            //        case PagingSortDirection.Desc:
            //            criteria.AddOrder(Order.Desc(pagingData.SortProperty));
            //            break;
            //    }
            //}

            // then add the paging criteria and execute the list
            criteria.SetMaxResults(pagingData.PageSize);
            criteria.SetFirstResult(pagingData.PageOffset);

            IList<T> list = ConvertIListToGenerics(criteria.List());

            //now change to the rowCount and execute second time 
            criteria.SetProjection(Projections.RowCount());
            criteria.SetFirstResult(0);
            criteria.SetMaxResults(1);
            System.Collections.IList results = criteria.List();

            Asserts.ArgumentHasLength(results, "CountQuery Results");

            int count = Convert.ToInt32(results[0]);

            return new PagedList<T>(list, count, pagingData);
        }

        public PagedQuery<T> CreatePagedQuery(String hql, PagingData pagingData)
        {
            Asserts.ArgumentExists(hql, "QueryText");

            Asserts.ArgumentExists(pagingData, "pagingData");

            return new PagedQuery<T>(hql, pagingData, Session);
        }


        public PagedList<T> FindByAdhocQueryWithPaging(PagedQuery<T> q)
        {

            IList<T> list = FindByAdhocQuery(q.DataQuery);

            System.Collections.IList countResults = q.CountQuery.List();

            Asserts.ArgumentHasLength(countResults, "CountQueryResults");

            int count = Convert.ToInt32(countResults[0]);

            return new PagedList<T>(list, count, q.PagingData);
        }
        */
        #endregion

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


        public T SaveOrUpdate(T instance)
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


        //public void DeleteAll(Type type)
        //{
        //    Session.Clear();
        //    ICriteria criteria = Session.CreateCriteria(type);
        //    ITransaction tr = Session.BeginTransaction();
        //    tr.Begin();
        //    Session.Delete(criteria);
        //    tr.Commit();
        //}


        public void Close()
        {
           //this.Close();
        }
    }
}
