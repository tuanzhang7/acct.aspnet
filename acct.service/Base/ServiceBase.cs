using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using acct.common.Base;
using acct.common.Helper;

namespace acct.service.Base
{
    public abstract partial class ServiceBase<T, TKey>
    {

        public IRepositoryBase<T, TKey> repositoryBase;
        public ServiceBase(IRepositoryBase<T, TKey> repositoryBase)
        {
            this.repositoryBase = repositoryBase;
        }
        #region Get Methods

        public virtual T GetById(TKey id)
        {
            return repositoryBase.GetById(id);
        }

        public IQueryable<T> GetAll()
        {
            return repositoryBase.GetAll();
        }/*
        public IList<T> GetAll(int maxResults)
        {
            return repositoryBase.GetAll(maxResults);
        }
        public IList<T> GetAll(int start, int maxResults, out int count)
        {
            return repositoryBase.GetAll(start, maxResults, out count);
        }
        public PaginatedList<T> GetPaginatedList(int pageIndex, int pageSize)
        {
            return repositoryBase.GetPaginatedList(pageIndex, pageSize);
        }
        public IList<T> GetByQuery(string query)
        {
            return repositoryBase.GetByQuery(query);
        }*/
        //public IList<T> GetByQuery(int maxResults, string query)
        //{
        //    return repositoryBase.GetByQuery( maxResults, query);
        //}
        //public T GetUniqueByQuery(string query)
        //{
        //    return repositoryBase.GetUniqueByQuery(query);
        //}

        #endregion

        #region CRUD Methods

        public object Save(T entity)
        {
            return repositoryBase.Save(entity);
        }
        //public void SaveOrUpdate(T entity)
        //{
        //    repositoryBase.SaveOrUpdate(entity);
        //}
        public void Delete(TKey id)
        {
            repositoryBase.Delete(id);
        }
        public void Update(T entity)
        {
            repositoryBase.Update(entity);
        }
        //public void Refresh(T entity)
        //{
        //    repositoryBase.Refresh(entity);
        //}
        //public void Evict(T entity)
        //{
        //    repositoryBase.Evict(entity);
        //}

        #endregion


        /// <summary>
        /// The NHibernate Session object is exposed only to the Service class.
        /// It is recommended that you...
        /// ...use the the NHibernateSession methods to control Transactions (unless you specifically want nested transactions).
        /// ...do not directly expose the Flush method (to prevent open transactions from locking your DB).
        /// </summary>
        public System.Type Type
        {
            get { return typeof(T); }
        }

    }
}
