using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acct.repository.ef6
{
    using acct.common.POCO;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class acctEntities : DbContext
    {
        public acctEntities()
            : base("name=acctEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //dbModelBuilder.Entity<User>().Map(t => t.ToTable("Users"));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Quotation> Quotation { get; set; }
        public DbSet<GST> GST { get; set; }
        public DbSet<UnitMeasure> UnitMeasure { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentDetail> PaymentDetail { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategory { get; set; }
    }
}
