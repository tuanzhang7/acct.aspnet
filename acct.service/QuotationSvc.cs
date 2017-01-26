using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.service.Base;
using acct.common.Helper;

namespace acct.service
{
    
    public partial class QuotationSvc : ServiceBase<Quotation, int>
    {
        protected IQuotationRepo repo;
        // Dependency Injection enabled constructors
        public QuotationSvc(IQuotationRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public QuotationSvc()
            : this(new acct.repository.ef6.QuotationRepo())
        {
        }

        #region Get

        public Quotation GetByQuotationNumber(string QuotationNumber)
        {
            if (string.IsNullOrEmpty(QuotationNumber)) { throw new ArgumentException("Quotation Number could not be Null Or Empty"); }
            var value = QuotationNumber.Trim();
            return repo.GetAll().Where
                (o => o.OrderNumber.Equals(QuotationNumber, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
        public IList<Quotation> GetByCustomer(int customerId)
        {
            if (customerId < 0) { throw new ArgumentException("Invalid Customer"); }

            IList<Quotation> list = repo.GetAll().Where
                (o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderNumber)
                .ToList();

            return list;

        }

        public IQueryable<Quotation> GetByFilter( int? CustomerId,
            DateRange.DateRangeFilter DateFilter = DateRange.DateRangeFilter.ThisMonth)
        {

            return repo.GetByFilter(CustomerId, DateFilter);

        }

        public object GetMonthlyTotal(DateTime year)
        {
            var result = from s in repo.GetAll().Where(i => i.OrderDate > year)
                         group s by s.OrderDate.ToString("yyyy.MM") into g
                         select new
                         {
                             date = g.Key,
                             total = g.Sum(x => x.TotalAmount)
                         };

            return result.ToList();
        }



        #endregion

    }
}
