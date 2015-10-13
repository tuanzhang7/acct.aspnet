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
    
    public partial class GSTSvc : ServiceBase<GST, int>
    {
        protected IGSTRepo repo;
        // Dependency Injection enabled constructors
        public GSTSvc(IGSTRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public GSTSvc()
            : this(new acct.repository.ef6.GSTRepo())
        {
        }

        public GST GetbyCode(string GSTCode){
            return repo.GetAll().Where(o => o.Code == GSTCode).FirstOrDefault();
        }
        public GST GetbyRate(decimal rate)
        {
            return repo.GetAll().Where(o => o.Rate == rate).FirstOrDefault();
        }
        
    }
}
