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
    
    public partial class SalesmanSvc : ServiceBase<Salesman, int>
    {
        protected ISalesmanRepo repo;
        // Dependency Injection enabled constructors
        public SalesmanSvc(ISalesmanRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public SalesmanSvc()
            : this(new acct.repository.ef6.SalesmanRepo())
        {
        }

        public Salesman GetByName(string SalesmanName)
        {
            return repo.GetAll().Where(o => o.Name == SalesmanName).FirstOrDefault();
        }
    }
}
