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
    public class OrderRepo : RepositoryBase<Order, int>, IOrderRepo
    {
        public Order Load(int id)
        {
            using (var context = new acctEntities())
            {
                var order = (from o in context.Order
                                .Include("Customer")
                                where o.Id == id
                                select o).FirstOrDefault();
                return order;
            }
        }
    }
}
