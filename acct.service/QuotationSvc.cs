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
        
    }
}
