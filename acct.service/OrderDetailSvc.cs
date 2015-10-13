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
    
    public partial class OrderDetailSvc : ServiceBase<OrderDetail, int>
    {
        protected IOrderDetailRepo repo;
        // Dependency Injection enabled constructors
        public OrderDetailSvc(IOrderDetailRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public OrderDetailSvc()
            : this(new acct.repository.ef6.OrderDetailRepo())
        {
        }
        public void Save(List<OrderDetail> OrderDetails)
        {
            //repo handle performance issue,ef6 using EntityFramework.BulkInsert.Extensions;
            repo.Save(OrderDetails,true);
        }
        
    }
}
