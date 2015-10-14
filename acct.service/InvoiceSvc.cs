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
    
    public partial class InvoiceSvc : ServiceBase<Invoice, int>
    {
        protected IInvoiceRepo repo;
        // Dependency Injection enabled constructors
        public InvoiceSvc(IInvoiceRepo repository)
            : base(repository)
        {
            repo = repository;
        }
        
        public InvoiceSvc()
            : this(new acct.repository.ef6.InvoiceRepo())
        {
        }

        #region Get

        public Invoice GetByInvoiceNumber(string InvoiceNumber)
        {
            if (string.IsNullOrEmpty(InvoiceNumber)) { throw new ArgumentException("Invoice Number could not be Null Or Empty"); }
            var value = InvoiceNumber.Trim();
            return repo.GetAll().Where
                (o => o.OrderNumber.Equals(InvoiceNumber, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
        public IList<Invoice> GetByCustomer(int customerId)
        {
            if (customerId < 0) { throw new ArgumentException("Invalid Customer"); }

            IList<Invoice> list = repo.GetAll().Where
                (o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderNumber)
                .ToList();

            return list;

        }

        public IQueryable<Invoice> GetByFilter(List<Order.StatusOptions> statusList, int? CustomerId,
            DateRange.DateRangeFilter DateFilter = DateRange.DateRangeFilter.ThisMonth)
        {

            return repo.GetByFilter(statusList, CustomerId, DateFilter) ;

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

        #region update
        public void UpdateStatus(int InvoiceId)
        {
            Invoice entity =repo.GetById(InvoiceId);
            UpdateStatus(entity);
            Update(entity);
        }
        
        private void UpdateStatus(Invoice entity)
        {
            if (entity != null)
            {
                decimal totalPayment = entity.PaymentDetails.Sum(p => p.Amount);
                decimal totalAmount = entity.TotalAmount;

                //entity.Paid = totalPayment >= totalAmount;
            }
        }
        
        public new object Save(Invoice entity)
        {
            UpdateStatus(entity);
            return base.Save(entity);
        }
        
        public new void Update(Invoice entity)
        {
            UpdateStatus(entity);
            base.Update(entity);
        }
        
        public void Save(List<Invoice> Invoices)
        {
            repo.Save(Invoices,false);
        }
        #endregion

    }
}
