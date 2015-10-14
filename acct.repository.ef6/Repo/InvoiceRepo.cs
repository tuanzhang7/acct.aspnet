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
    public class InvoiceRepo : RepositoryBase<Invoice, int>, IInvoiceRepo
    {
        public new Invoice GetById(int Id)
        {
            using (var context = new acctEntities())
            {
                var item = context.Invoice
                    .Include(m => m.Customer)
                    .Include(m => m.Salesman)
                                .Include(m=>m.OrderDetail)
                                .Where(m=>m.Id== Id)
                                .FirstOrDefault();
                return item;
            }
        }
        public new IQueryable<Invoice> GetAll()
        {
            var context = new acctEntities();

            var item = context.Invoice
                .Include(m => m.Customer)
                .Include(m => m.Salesman);
            return item;
            
        }

        public IQueryable<Invoice> GetByFilter(List<Order.StatusOptions> statusList, int? CustomerId,
            DateRange.DateRangeFilter DateFilter = DateRange.DateRangeFilter.ThisMonth)
        {
            var list = this.GetAll();
            if (DateFilter != DateRange.DateRangeFilter.AnyTime)
            {
                DateRange dRange = DateRange.GetDateRange(DateTime.Today, DateFilter);
                list = list.Where(o => o.OrderDate >= dRange.StartDate && o.OrderDate <= dRange.EndDate);
            }
            if (statusList.Count > 0)
            {
                //Order.StatusOptions _status =
                //(Order.StatusOptions)Enum.Parse(typeof(Order.StatusOptions), status);
                //list = list.Where(o => o.Status == (int)_status);

                var searchPredicate = PredicateBuilder.False<Invoice>();

                foreach (Order.StatusOptions status in statusList)
                {
                    searchPredicate =
                      searchPredicate.Or(SongsVar => SongsVar.Status == (int)status);
                }
                //searchPredicate = searchPredicate.And(ovar=>var.);
                list = list.AsExpandable().Where(searchPredicate);
            }
            else
            {

            }
            //if (CustomerId != null)
            //{
            //    if (CustomerId < 0) { throw new ArgumentException("Invalid Customer"); }
            //    list = list.Where(o => o.CustomerId == CustomerId);
            //}
            IQueryable<Invoice> result = list.OrderByDescending(o => o.OrderNumber);
            return result;

        }
    }
}
