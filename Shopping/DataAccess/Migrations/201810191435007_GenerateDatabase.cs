namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenerateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.About",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 250),
                        Description = c.String(),
                        Detail = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 6),
                        DeliveryTo = c.String(nullable: false, maxLength: 250),
                        Deliver = c.String(maxLength: 50),
                        Driver = c.String(maxLength: 50),
                        CarNumber = c.String(maxLength: 32),
                        Description = c.String(maxLength: 250),
                        InvoiveNo = c.String(maxLength: 30),
                        DocumentDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ApprovedBy);
            
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
                        Role = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 6),
                        Amount = c.Double(nullable: false),
                        ReceiverName = c.String(nullable: false, maxLength: 50),
                        ReceiverAddress = c.String(nullable: false),
                        ReceiverPhone = c.String(nullable: false, maxLength: 12),
                        Status = c.Boolean(nullable: false),
                        ApprovedDateTime = c.DateTime(),
                        ReceivedDateTime = c.DateTime(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        Canceled = c.Boolean(nullable: false),
                        UpdatedBy = c.Int(),
                        UserId = c.Int(nullable: false),
                        ApprovedId = c.Int(),
                        DeliveredId = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedId)
                .ForeignKey("dbo.User", t => t.DeliveredId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.UserId)
                .Index(t => t.ApprovedId)
                .Index(t => t.DeliveredId)
                .Index(t => t.User_Id);
            
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
                        Code = c.String(nullable: false, maxLength: 6),
                        Name = c.String(maxLength: 250),
                        Title = c.String(maxLength: 250),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        Promotion = c.Single(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Detail = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ImagePath = c.String(),
                        ProductCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ProductCategoryId, cascadeDelete: true)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                        SeoTitle = c.String(maxLength: 250),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.DeliveryDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DiscountRate = c.Single(nullable: false),
                        VatRate = c.Single(nullable: false),
                        ExpiryDateTime = c.DateTime(),
                        DeliveryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Delivery", t => t.DeliveryId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.DeliveryId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.FeedBack",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 12),
                        Email = c.String(maxLength: 50),
                        Address = c.String(),
                        Content = c.String(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.NewCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                        SeoTitle = c.String(maxLength: 250),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewCategory", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.New",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Description = c.String(nullable: false),
                        Detail = c.String(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        NewCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewCategory", t => t.NewCategoryId, cascadeDelete: true)
                .Index(t => t.NewCategoryId);
            
            CreateTable(
                "dbo.ReceiveDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DiscountRate = c.Single(nullable: false),
                        VatRate = c.Single(nullable: false),
                        ExpiryDateTime = c.DateTime(),
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
                        Code = c.String(nullable: false, maxLength: 6),
                        ReceiveFrom = c.String(nullable: false, maxLength: 250),
                        Deliver = c.String(maxLength: 50),
                        Driver = c.String(maxLength: 50),
                        CarNumber = c.String(maxLength: 32),
                        Description = c.String(maxLength: 250),
                        DocumentDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ApprovedBy);
            
            CreateTable(
                "dbo.Slide",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayOrder = c.Int(nullable: false),
                        Link = c.String(maxLength: 250),
                        Description = c.String(),
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiveDetail", "ReceiveId", "dbo.Receive");
            DropForeignKey("dbo.Receive", "ApprovedBy", "dbo.User");
            DropForeignKey("dbo.ReceiveDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.NewCategory", "ParentId", "dbo.NewCategory");
            DropForeignKey("dbo.New", "NewCategoryId", "dbo.NewCategory");
            DropForeignKey("dbo.DeliveryDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.DeliveryDetail", "DeliveryId", "dbo.Delivery");
            DropForeignKey("dbo.Delivery", "ApprovedBy", "dbo.User");
            DropForeignKey("dbo.Order", "User_Id", "dbo.User");
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.ProductCategory", "ParentId", "dbo.ProductCategory");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Order", "DeliveredId", "dbo.User");
            DropForeignKey("dbo.Order", "ApprovedId", "dbo.User");
            DropIndex("dbo.Receive", new[] { "ApprovedBy" });
            DropIndex("dbo.Receive", new[] { "Code" });
            DropIndex("dbo.ReceiveDetail", new[] { "ProductId" });
            DropIndex("dbo.ReceiveDetail", new[] { "ReceiveId" });
            DropIndex("dbo.New", new[] { "NewCategoryId" });
            DropIndex("dbo.NewCategory", new[] { "ParentId" });
            DropIndex("dbo.DeliveryDetail", new[] { "ProductId" });
            DropIndex("dbo.DeliveryDetail", new[] { "DeliveryId" });
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.Product", new[] { "Code" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "User_Id" });
            DropIndex("dbo.Order", new[] { "DeliveredId" });
            DropIndex("dbo.Order", new[] { "ApprovedId" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.Order", new[] { "Code" });
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.Delivery", new[] { "ApprovedBy" });
            DropIndex("dbo.Delivery", new[] { "Code" });
            DropTable("dbo.Slide");
            DropTable("dbo.Receive");
            DropTable("dbo.ReceiveDetail");
            DropTable("dbo.New");
            DropTable("dbo.NewCategory");
            DropTable("dbo.Footer");
            DropTable("dbo.FeedBack");
            DropTable("dbo.DeliveryDetail");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Order");
            DropTable("dbo.User");
            DropTable("dbo.Delivery");
            DropTable("dbo.Contact");
            DropTable("dbo.About");
        }
    }
}
