using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.common.Base
{
    public interface ITrackableEntity
    {
        DateTime? Modified { get; set; }
        string ModifiedBy { get; set; }
    }
}
