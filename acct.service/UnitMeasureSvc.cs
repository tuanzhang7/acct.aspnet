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
    
    public partial class UnitMeasureSvc : ServiceBase<UnitMeasure, int>
    {
        protected IUnitMeasureRepo repo;
        // Dependency Injection enabled constructors
        public UnitMeasureSvc(IUnitMeasureRepo repository)
            : base(repository)
        {
            repo = repository;
        }

        public UnitMeasureSvc()
            : this(new acct.repository.ef6.UnitMeasureRepo())
        {
        }
        
    }
}
