namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.About",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WelcomeImagePath = c.String(maxLength: 255),
                        Description = c.String(),
                        WhoWeAreImagePath = c.String(maxLength: 255),
                        Information = c.String(),
                        Quality = c.String(),
                        Service = c.String(),
                        Support = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CodeGenerating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Prefix = c.String(nullable: false, maxLength: 6),
                        LastGeneratedDateTime = c.DateTime(nullable: false),
                        GeneratingNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 16),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Address = c.String(),
                        Email = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 12),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Title = c.String(maxLength: 255),
                        Content = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.FeedbackDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FeedbackId = c.Int(nullable: false),
                        ReplyContent = c.String(),
                        RepliedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feedback", t => t.FeedbackId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FeedbackId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 16),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Address = c.String(),
                        Email = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 12),
                        GroupUserId = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.GroupUser", t => t.GroupUserId, cascadeDelete: true)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.GroupUserId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.GroupUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        Amount = c.Double(nullable: false),
                        ReceiverName = c.String(nullable: false, maxLength: 50),
                        ReceiverAddress = c.String(nullable: false),
                        ReceiverPhone = c.String(nullable: false, maxLength: 12),
                        ApprovedDateTime = c.DateTime(),
                        ReceivedDateTime = c.DateTime(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        PaymentId = c.String(nullable: false),
                        CanceledBy = c.Int(),
                        UpdatedBy = c.Int(),
                        CustomerId = c.Int(nullable: false),
                        ApproverId = c.Int(),
                        DeliverId = c.Int(),
                        ClosedBy = c.Int(),
                        IsHasInvoice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApproverId)
                .ForeignKey("dbo.User", t => t.CanceledBy)
                .ForeignKey("dbo.User", t => t.ClosedBy)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.DeliverId)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CanceledBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.CustomerId)
                .Index(t => t.ApproverId)
                .Index(t => t.DeliverId)
                .Index(t => t.ClosedBy);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 12),
                        Name = c.String(maxLength: 250),
                        Title = c.String(maxLength: 250),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        Promotion = c.Single(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Detail = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ImagePath = c.String(),
                        FrontImagePath = c.String(),
                        BackImagePath = c.String(),
                        ProductCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                        SeoTitle = c.String(maxLength: 250),
                        Description = c.String(maxLength: 300),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Footer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoice", t => t.InvoiceId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        CustomerName = c.String(maxLength: 30),
                        CustomerAddress = c.String(maxLength: 255),
                        CustomerPhone = c.String(maxLength: 12),
                        Description = c.String(maxLength: 250),
                        CreatedBy = c.Int(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        OrderId = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.ApprovedBy)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.ReceiveDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DiscountRate = c.Single(nullable: false),
                        VatRate = c.Single(nullable: false),
                        ReceiveId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Receive", t => t.ReceiveId, cascadeDelete: true)
                .Index(t => t.ReceiveId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Receive",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 30),
                        ReceiveFrom = c.String(nullable: false, maxLength: 250),
                        Deliver = c.String(maxLength: 50),
                        Driver = c.String(maxLength: 50),
                        CarNumber = c.String(maxLength: 32),
                        Description = c.String(maxLength: 250),
                        DocumentDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.ApprovedBy);
            
            CreateTable(
                "dbo.Slide",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Name = c.String(maxLength: 32),
                        Title = c.String(maxLength: 255),
                        ImagePath = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.CreatedBy)
                .ForeignKey("dbo.User", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Slide", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.Slide", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.Receive", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.ReceiveDetail", "ReceiveId", "dbo.Receive");
            DropForeignKey("dbo.Receive", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.Receive", "ApprovedBy", "dbo.User");
            DropForeignKey("dbo.ReceiveDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.InvoiceDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Invoice", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.Invoice", "OrderId", "dbo.Order");
            DropForeignKey("dbo.InvoiceDetail", "InvoiceId", "dbo.Invoice");
            DropForeignKey("dbo.Invoice", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.Invoice", "ApprovedBy", "dbo.User");
            DropForeignKey("dbo.Order", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.ProductCategory", "UpdatedBy", "dbo.User");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.ProductCategory", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.Product", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "DeliverId", "dbo.User");
            DropForeignKey("dbo.Order", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Order", "ClosedBy", "dbo.User");
            DropForeignKey("dbo.Order", "CanceledBy", "dbo.User");
            DropForeignKey("dbo.Order", "ApproverId", "dbo.User");
            DropForeignKey("dbo.User", "GroupUserId", "dbo.GroupUser");
            DropForeignKey("dbo.FeedbackDetail", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "CreatedBy", "dbo.User");
            DropForeignKey("dbo.FeedbackDetail", "FeedbackId", "dbo.Feedback");
            DropForeignKey("dbo.Feedback", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Slide", new[] { "UpdatedBy" });
            DropIndex("dbo.Slide", new[] { "CreatedBy" });
            DropIndex("dbo.Receive", new[] { "ApprovedBy" });
            DropIndex("dbo.Receive", new[] { "UpdatedBy" });
            DropIndex("dbo.Receive", new[] { "CreatedBy" });
            DropIndex("dbo.Receive", new[] { "Code" });
            DropIndex("dbo.ReceiveDetail", new[] { "ProductId" });
            DropIndex("dbo.ReceiveDetail", new[] { "ReceiveId" });
            DropIndex("dbo.Invoice", new[] { "OrderId" });
            DropIndex("dbo.Invoice", new[] { "ApprovedBy" });
            DropIndex("dbo.Invoice", new[] { "UpdatedBy" });
            DropIndex("dbo.Invoice", new[] { "CreatedBy" });
            DropIndex("dbo.Invoice", new[] { "Code" });
            DropIndex("dbo.InvoiceDetail", new[] { "ProductId" });
            DropIndex("dbo.InvoiceDetail", new[] { "InvoiceId" });
            DropIndex("dbo.ProductCategory", new[] { "UpdatedBy" });
            DropIndex("dbo.ProductCategory", new[] { "CreatedBy" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.Product", new[] { "UpdatedBy" });
            DropIndex("dbo.Product", new[] { "CreatedBy" });
            DropIndex("dbo.Product", new[] { "Code" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "ClosedBy" });
            DropIndex("dbo.Order", new[] { "DeliverId" });
            DropIndex("dbo.Order", new[] { "ApproverId" });
            DropIndex("dbo.Order", new[] { "CustomerId" });
            DropIndex("dbo.Order", new[] { "UpdatedBy" });
            DropIndex("dbo.Order", new[] { "CanceledBy" });
            DropIndex("dbo.Order", new[] { "Code" });
            DropIndex("dbo.User", new[] { "CreatedBy" });
            DropIndex("dbo.User", new[] { "GroupUserId" });
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.FeedbackDetail", new[] { "FeedbackId" });
            DropIndex("dbo.FeedbackDetail", new[] { "UserId" });
            DropIndex("dbo.Feedback", new[] { "CustomerId" });
            DropIndex("dbo.Customer", new[] { "UserName" });
            DropTable("dbo.Slide");
            DropTable("dbo.Receive");
            DropTable("dbo.ReceiveDetail");
            DropTable("dbo.Invoice");
            DropTable("dbo.InvoiceDetail");
            DropTable("dbo.Footer");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Order");
            DropTable("dbo.GroupUser");
            DropTable("dbo.User");
            DropTable("dbo.FeedbackDetail");
            DropTable("dbo.Feedback");
            DropTable("dbo.Customer");
            DropTable("dbo.CodeGenerating");
            DropTable("dbo.About");
        }
    }
}
