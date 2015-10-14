using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.Base;
using acct.common.Helper;
using acct.common.POCO;

namespace acct.common.Repository
{
    public interface IInvoiceRepo : IRepositoryBase<Invoice, int>
    {
        IQueryable<Invoice> GetByFilter(List<Order.StatusOptions> statusList, int? customerId, DateRange.DateRangeFilter dateFilter);
    }
}
