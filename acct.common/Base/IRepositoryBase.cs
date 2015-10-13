using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.Helper;

namespace acct.common.Base
{
    public interface IRepositoryBase<T, TKey> : IDisposable
    {
        // Get Methods
        T GetById(TKey Id);
        IQueryable <T> GetAll();
        /*
        IList<T> GetAll(int maxResults);
        IList<T> GetAll(int start, int maxResults, out int count);
        PaginatedList<T> GetPaginatedList(int pageIndex, int pageSize);
        */
        

        // CRUD Methods
        
        object Save(T entity);
        void Save(IEnumerable<T> list,bool BulkInsert);
        //void SaveOrUpdate(T entity);
        void Delete(TKey Id);
        void Update(T entity);
        /**/
        //void Refresh(T entity);
        //void Evict(T entity);
        //void BeginTransaction();
        //void RollbackTransaction();
        //void CommitTransaction();
        // Properties
        System.Type Type { get; }

    }
}
