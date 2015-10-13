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
    
    public partial class OrderSvc : ServiceBase<Order, int>
    {
        protected IOrderRepo repo;
        // Dependency Injection enabled constructors
        public OrderSvc(IOrderRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public OrderSvc()
            : this(new acct.repository.ef6.OrderRepo())
        {
        }

        public Order Load(int Id)
        {
            return repo.Load(Id);
        }
        public bool IsExist(string orderNumber)
        {
            return repo.GetAll().Where(o => o.OrderNumber == orderNumber).Count() > 0;
        }
        
    }
}
