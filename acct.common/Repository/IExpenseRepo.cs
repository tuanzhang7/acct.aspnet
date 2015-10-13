using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;

namespace acct.common.Repository
{
    public interface IExpenseRepo : acct.common.Base.IRepositoryBase<Expense, int>
    {
    }
}
