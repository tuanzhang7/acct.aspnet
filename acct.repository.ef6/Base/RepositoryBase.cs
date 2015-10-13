using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.Base;
using EntityFramework.BulkInsert.Extensions;
namespace acct.repository.ef6.Base
{
    
    public abstract class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T:class
    {
        #region Declarations

        protected acctEntities context;
        private bool _disposed = false;
        //private Dictionary<string, FetchMode> _fetchModeMap = new Dictionary<string, FetchMode>();
        #endregion

        #region Constructors

        public RepositoryBase()
        {
            if (context == null)
            {
                context = new acctEntities();
            }
        }
        
        
        #endregion

        #region Get Methods


        public T GetById(TKey id)
        {
            return context.Set<T>().Find(id);
        }
        public IQueryable<T> GetAll()
        {
            return context.Set<T>();
        }
        #region CRUD Methods

        public object Save(T entity)
        {
            object result = context.Set<T>().Add(entity);
            context.SaveChanges();
            return result;

        }
        public void Save(IEnumerable<T> list,bool BulkInsert)
        {
            if (BulkInsert)
            {
                context.BulkInsert(list);
            }
            else
            {
                context.Set<T>().AddRange(list);
            }
            context.SaveChanges();
        }
        
        //public void Delete(T entity)
        //{
        //    if (context.Entry(entity).State == EntityState.Detached)
        //    {
        //        context.Set<T>().Attach(entity);
        //    }
        //    context.Set<T>().Remove(entity);
        //    context.SaveChanges();
        //}
        public virtual void Delete(TKey id)
        {
            T entity = context.Set<T>().Find(id);
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            //context.Set<T>().Attach(entity);
            //context.Entry(entity).State = System.Data.EntityState.Modified;
            context.SaveChanges();
        }
        
        #endregion

        public System.Type Type
        {
            get { return typeof(T); }
        }
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }
        private void Dispose(bool finalizing)
        {
            if (!_disposed)
            {
                context.Dispose();

                if (!finalizing)
                    GC.SuppressFinalize(this);

                _disposed = true;
            }
        }
        /*
        public virtual T GetById(TKey id)
        {
            return (T)Session.GetISession().Get(typeof(T), id);
        }
        
        
        public IList<T> GetAll(int maxResults)
        {
            return GetByCriteria(maxResults);
        }
        public IList<T> GetAll(int start, int maxResults, out int count)
        { 

            ICriteria criteria = Session.GetISession()
                .CreateCriteria(typeof(T));

            ICriteria countCriteria = CriteriaTransformer.TransformToRowCount(criteria);

            criteria.SetFirstResult(start).SetMaxResults(maxResults);

            var results = countCriteria.List()[0];
            count=(int)results;
            return criteria.List<T>();
            
        }
        public PaginatedList<T> GetPaginatedList(int pageIndex, int pageSize)
        {
            int count;
            int _start = (pageIndex-1) * pageSize;
            IList<T> _List = GetAll(_start, pageSize, out count);
            var paginatedContact = new PaginatedList<T>(_List,
                                    pageIndex,
                                    pageSize, count,false);
            return paginatedContact;

        }
        
        */
        #endregion
        /*
        #region Misc Methods

        public void SetFetchMode(string associationPath, FetchMode mode)
        {
            if (!_fetchModeMap.ContainsKey(associationPath))
                _fetchModeMap.Add(associationPath, mode);
        }

        public ICriteria CreateCriteria()
        {
            ICriteria criteria = Session.GetISession().CreateCriteria(typeof(T));

            foreach (var pair in _fetchModeMap)
                criteria = criteria.SetFetchMode(pair.Key, pair.Value);

            return criteria;
        }

        #endregion

        

        #region Properties

        /// <summary>
        /// The NHibernate Session object is exposed only to the Repository class.
        /// It is recommended that you...
        /// ...use the the NHibernateSession methods to control Transactions (unless you specifically want nested transactions).
        /// ...do not directly expose the Flush method (to prevent open transactions from locking your DB).
        /// </summary>
        
        public INHibernateSession Session
        {
            get { return session; }
        }

        #endregion

        

        #region Trasaction
        public void BeginTransaction()
        {
            this.Session.BeginTransaction();
        }
        public void RollbackTransaction()
        {
            this.Session.RollbackTransaction();
        }
        public void CommitTransaction()
        {
            this.Session.CommitTransaction();
        }
        #endregion
        */
        
        

        #endregion
    }
}
