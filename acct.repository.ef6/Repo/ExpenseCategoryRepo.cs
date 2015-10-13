using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.common.Base;
using acct.repository.ef6.Base;
namespace acct.repository.ef6
{
    public class ExpenseCategoryRepo : RepositoryBase<ExpenseCategory, int>, IExpenseCategoryRepo
    {
    }
}
