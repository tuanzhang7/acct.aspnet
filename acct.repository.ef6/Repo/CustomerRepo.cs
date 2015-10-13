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
    public class CustomerRepo : RepositoryBase<Customer, int>, ICustomerRepo
    {
        public new Customer GetById(int Id)
        {
            using (var context = new acctEntities())
            {
                var item = context.Customer
                    .Include(m => m.GST)
                                .Where(m => m.Id == Id)
                                .FirstOrDefault();
                return item;
            }
        }

    }
}
