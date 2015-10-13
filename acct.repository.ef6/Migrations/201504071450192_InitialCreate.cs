namespace acct.repository.ef6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(),
                        Phone = c.String(maxLength: 255),
                        Fax = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        ContactName = c.String(maxLength: 255),
                        idmas_GST = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GST", t => t.idmas_GST, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.idmas_GST);
            
            CreateTable(
                "dbo.GST",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 45),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.String(nullable: false, maxLength: 12),
                        CustomerId = c.Int(nullable: false),
                        SalesmanId = c.Int(),
                        OrderDate = c.DateTime(nullable: false),
                        OrderType = c.Int(nullable: false),
                        Remark = c.String(),
                        GSTRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //TotalWithTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //AmountOutstanding = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Modified = c.DateTime(),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Salesman", t => t.SalesmanId)
                .Index(t => t.OrderNumber, unique: true)
                .Index(t => t.CustomerId)
                .Index(t => t.SalesmanId);

            AlterColumn("dbo.Order", "GSTRate", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            AlterColumn("dbo.Order", "TotalAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            AlterColumn("dbo.Order", "AmountPaid", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));

            Sql("ALTER TABLE [dbo].[Order] ADD TotalWithTax AS round((TotalAmount*(1+GSTRate/100)),2)");
            Sql("ALTER TABLE [dbo].[Order] ADD AmountOutstanding AS (round((TotalAmount*(1+GSTRate/100)),2)-AmountPaid)");

            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Description = c.String(),
                        Qty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        //LineTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);

            AlterColumn("dbo.OrderDetail", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            //AlterColumn("dbo.OrderDetail", "LineTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
            Sql("ALTER TABLE [dbo].[OrderDetail] ADD LineTotal AS (round((([UnitPrice]*[Qty])*((1)-[Discount]/(100))),(2)))");

            CreateTable(
                "dbo.Salesman",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.PaymentDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentId = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoice", t => t.InvoiceId)
                .ForeignKey("dbo.Payment", t => t.PaymentId, cascadeDelete: true)
                .Index(t => t.PaymentId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethod = c.String(),
                        ReferenceNumber = c.String(),
                        Remarks = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Expense",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpenseCategory", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ExpenseCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Category, unique: true);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Value = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UnitMeasure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Quotation",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotation", "Id", "dbo.Order");
            DropForeignKey("dbo.Invoice", "Id", "dbo.Order");
            DropForeignKey("dbo.Expense", "CategoryId", "dbo.ExpenseCategory");
            DropForeignKey("dbo.PaymentDetail", "PaymentId", "dbo.Payment");
            DropForeignKey("dbo.Payment", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.PaymentDetail", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.Order", "SalesmanId", "dbo.Salesman");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Customer", "idmas_GST", "dbo.GST");
            DropIndex("dbo.Quotation", new[] { "Id" });
            DropIndex("dbo.Invoice", new[] { "Id" });
            DropIndex("dbo.UnitMeasure", new[] { "Code" });
            DropIndex("dbo.Options", new[] { "Name" });
            DropIndex("dbo.ExpenseCategory", new[] { "Category" });
            DropIndex("dbo.Expense", new[] { "CategoryId" });
            DropIndex("dbo.Payment", new[] { "CustomerId" });
            DropIndex("dbo.PaymentDetail", new[] { "InvoiceId" });
            DropIndex("dbo.PaymentDetail", new[] { "PaymentId" });
            DropIndex("dbo.Salesman", new[] { "Name" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "SalesmanId" });
            DropIndex("dbo.Order", new[] { "CustomerId" });
            DropIndex("dbo.Order", new[] { "OrderNumber" });
            DropIndex("dbo.GST", new[] { "Code" });
            DropIndex("dbo.Customer", new[] { "idmas_GST" });
            DropIndex("dbo.Customer", new[] { "Name" });
            DropTable("dbo.Quotation");
            DropTable("dbo.Invoice");
            DropTable("dbo.UnitMeasure");
            DropTable("dbo.Options");
            DropTable("dbo.ExpenseCategory");
            DropTable("dbo.Expense");
            DropTable("dbo.Payment");
            DropTable("dbo.PaymentDetail");
            DropTable("dbo.Salesman");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Order");
            DropTable("dbo.GST");
            DropTable("dbo.Customer");
        }
    }
}
