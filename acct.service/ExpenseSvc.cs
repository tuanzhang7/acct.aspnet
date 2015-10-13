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

    public partial class ExpenseSvc : ServiceBase<Expense, int>
    {
        protected IExpenseRepo repo;
        // Dependency Injection enabled constructors
        public ExpenseSvc(IExpenseRepo repository)
            : base(repository)
        {
            repo = repository;
        }

    }
}
