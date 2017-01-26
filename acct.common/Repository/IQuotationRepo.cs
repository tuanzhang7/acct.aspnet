using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.Base;
using acct.common.POCO;
using acct.common.Helper;
namespace acct.common.Repository
{
    public interface IQuotationRepo : IRepositoryBase<Quotation, int>
    {
        IQueryable<Quotation> GetByFilter(int? customerId, DateRange.DateRangeFilter dateFilter);
    }
}
