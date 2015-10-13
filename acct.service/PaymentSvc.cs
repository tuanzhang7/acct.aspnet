using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.service.Base;

namespace acct.service
{
    
    public partial class PaymentSvc : ServiceBase<Payment, int>
    {
        protected IPaymentRepo repo;
        
        // Dependency Injection enabled constructors
        public PaymentSvc(IPaymentRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public PaymentSvc()
            : this(new acct.repository.ef6.PaymentRepo())
        {
        }
        //Move to trigger
        //public new object Save(Payment entity)
        //{
        //    InvoiceSvc invoiceSvc = new service.InvoiceSvc();

        //    object obj=base.Save(entity);
        //    foreach (var item in entity.PaymentDetail)
        //    {
        //        invoiceSvc.UpdateStatus(item.InvoiceId);
        //    }
            
        //    return obj;
        //}
        public void Save(List<Payment> Payments)
        {
            repo.Save(Payments, false);
        }

        public Payment GetByRemark(string Remark)
        {
            if (string.IsNullOrEmpty(Remark)) { throw new ArgumentException("Remark could not be Null Or Empty"); }
            var value = Remark.Trim();
            return repo.GetAll().Where
                (o => o.Remarks.Equals(value, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }
    }
}
