using acct.common.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace acct.repository.ef6
{
    public partial class acctEntities
    {
        public override int SaveChanges()
        {
            // fix trackable entities
            var trackables = ChangeTracker.Entries<ITrackableEntity>();

            if (trackables != null)
            {
                // added
                foreach (var item in trackables.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.Modified = System.DateTime.Now;
                    item.Entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
                }
                // modified
                foreach (var item in trackables.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.Modified = System.DateTime.Now;
                    item.Entity.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;
                }
            }
            return base.SaveChanges();
        }
    }
}
