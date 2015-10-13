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

    public partial class ExpenseCategorySvc : ServiceBase<ExpenseCategory, int>
    {
        protected IExpenseCategoryRepo repo;
        // Dependency Injection enabled constructors
        public ExpenseCategorySvc(IExpenseCategoryRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public ExpenseCategorySvc()
            : this(new acct.repository.ef6.ExpenseCategoryRepo())
        {
        }

        public ExpenseCategory GetbyCode(string Category){
            return repo.GetAll().Where(o => o.Category == Category).FirstOrDefault();
        }
    }
}