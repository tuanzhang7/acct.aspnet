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
    
    public partial class PaymentDetailSvc : ServiceBase<PaymentDetail, int>
    {
        protected IPaymentDetailRepo repo;
        
        // Dependency Injection enabled constructors
        public PaymentDetailSvc(IPaymentDetailRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public PaymentDetailSvc()
            : this(new acct.repository.ef6.PaymentDetailRepo())
        {
        }
        public new object Save(PaymentDetail entity)
        {
            InvoiceSvc invoiceSvc = new service.InvoiceSvc();

            object obj=base.Save(entity);

            invoiceSvc.UpdateStatus(entity.InvoiceId);
            return obj;
        }
        public void Save(List<PaymentDetail> PaymentDetail)
        {
            //repo handle performance issue,ef6 using EntityFramework.BulkInsert.Extensions;
            repo.Save(PaymentDetail, true);
        }
    }
}
