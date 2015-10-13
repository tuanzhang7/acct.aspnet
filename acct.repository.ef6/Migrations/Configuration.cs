namespace acct.repository.ef6.Migrations
{
    using acct.common.POCO;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<acct.repository.ef6.acctEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(acct.repository.ef6.acctEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //Option

//            context.Options.AddOrUpdate(
//                new Options { Id=1, Name = "company_name", Value = "Thick Potential Technology" });
//            context.Options.AddOrUpdate(
//                new Options
//                {
//                    Id = 2,
//                    Name = "company_address",
//                    Value = @"BLK 715 Bedok Reservoir Road
//#05-3002
//Singapore 470715"
//                });
//            context.Options.AddOrUpdate(
//                new Options { Id = 3, Name = "next_invoice_num", Value = "45" });

//            context.SaveChanges();

//            GST gst = new common.POCO.GST { Id = 1, Code = "00", Rate = 0 };
//            GST gst7 = new common.POCO.GST { Id = 2, Code = "07", Rate = 7};


//            context.GST.Add(gst);
//            context.GST.Add(gst7);

//            context.SaveChanges();
        }
    }
}
