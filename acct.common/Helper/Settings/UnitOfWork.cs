using acct.common.POCO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.common.Helper.Settings
{
    public interface IUnitOfWork
    {
        DbSet<Options> Settings { get; set; }
        int SaveChanges();
    }

    public class UnitOfWork : DbContext, IUnitOfWork//, IPerWebRequest
    {
        public UnitOfWork() : base("acctEntities") { }

        public DbSet<Options> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Options>()
                        .HasKey(x => new { x.Name});

            modelBuilder.Entity<Options>()
                        .Property(x => x.Value)
                        .IsOptional();

            base.OnModelCreating(modelBuilder);
        }
    }
}
