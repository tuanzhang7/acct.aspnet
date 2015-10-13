using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acct.common.POCO;
using acct.common.Repository;
using acct.common.Base;
using acct.repository.ef6.Base;
using System.Data.Entity;
namespace acct.repository.ef6
{
    public class InvoiceRepo : RepositoryBase<Invoice, int>, IInvoiceRepo
    {
        public new Invoice GetById(int Id)
        {
            using (var context = new acctEntities())
            {
                var item = context.Invoice
                    .Include(m => m.Customer)
                    .Include(m => m.Salesman)
                                .Include(m=>m.OrderDetail)
                                .Where(m=>m.Id== Id)
                                .FirstOrDefault();
                return item;
            }
        }
        public new IQueryable<Invoice> GetAll()
        {
            var context = new acctEntities();

            var item = context.Invoice
                .Include(m => m.Customer)
                .Include(m => m.Salesman);
            return item;
            
        }
    }
}
