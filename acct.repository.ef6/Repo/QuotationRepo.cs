using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.common.Base;
using acct.repository.ef6.Base;
using System.Data.Entity;
using acct.common.Helper;
using LinqKit;

namespace acct.repository.ef6
{
    public class QuotationRepo : RepositoryBase<Quotation, int>, IQuotationRepo
    {
        public new Quotation GetById(int Id)
        {
            using (var context = new acctEntities())
            {
                var item = context.Quotation
                    .Include(m => m.Customer)
                    .Include(m => m.Salesman)
                                .Include(m => m.OrderDetail)
                                .Where(m => m.Id == Id)
                                .FirstOrDefault();
                return item;
            }
        }
        public new IQueryable<Quotation> GetAll()
        {
            var context = new acctEntities();

            var item = context.Quotation
                .Include(m => m.Customer)
                .Include(m => m.Salesman);
            return item;

        }

        public IQueryable<Quotation> GetByFilter( int? CustomerId,
            DateRange.DateRangeFilter DateFilter = DateRange.DateRangeFilter.ThisMonth)
        {
            var list = this.GetAll();
            if (DateFilter != DateRange.DateRangeFilter.AnyTime)
            {
                DateRange dRange = DateRange.GetDateRange(DateTime.Today, DateFilter);
                list = list.Where(o => o.OrderDate >= dRange.StartDate && o.OrderDate <= dRange.EndDate);
            }
            
            //if (CustomerId != null)
            //{
            //    if (CustomerId < 0) { throw new ArgumentException("Invalid Customer"); }
            //    list = list.Where(o => o.CustomerId == CustomerId);
            //}
            IQueryable<Quotation> result = list.OrderByDescending(o => o.OrderNumber);
            return result;

        }
    }
}
