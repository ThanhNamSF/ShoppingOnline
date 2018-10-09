namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeDatabase : DbMigration
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
                        CreatedDateTime = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        UpdatedBy = c.Int(),
                        Status = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Status = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Footer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        ProductId = c.Int(),
                        SlideId = c.Int(),
                        NewId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.New", t => t.NewId)
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Slide", t => t.SlideId)
                .Index(t => t.ProductId)
                .Index(t => t.SlideId)
                .Index(t => t.NewId);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewCategory", t => t.NewCategoryId, cascadeDelete: true)
                .Index(t => t.NewCategoryId);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NewCategory", t => t.ParentId)
                .Index(t => t.ParentId);
            
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
                        UpdatedBy = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ProductCategoryId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategory", t => t.ParentId)
                .Index(t => t.ParentId);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 50),
                        CustomerAddress = c.String(nullable: false),
                        CustomerPhone = c.String(nullable: false, maxLength: 12),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Image", "SlideId", "dbo.Slide");
            DropForeignKey("dbo.Product", "ProductCategoryId", "dbo.ProductCategory");
            DropForeignKey("dbo.ProductCategory", "ParentId", "dbo.ProductCategory");
            DropForeignKey("dbo.Image", "ProductId", "dbo.Product");
            DropForeignKey("dbo.NewCategory", "ParentId", "dbo.NewCategory");
            DropForeignKey("dbo.New", "NewCategoryId", "dbo.NewCategory");
            DropForeignKey("dbo.Image", "NewId", "dbo.New");
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            DropIndex("dbo.Product", new[] { "ProductCategoryId" });
            DropIndex("dbo.Product", new[] { "Code" });
            DropIndex("dbo.NewCategory", new[] { "ParentId" });
            DropIndex("dbo.New", new[] { "NewCategoryId" });
            DropIndex("dbo.Image", new[] { "NewId" });
            DropIndex("dbo.Image", new[] { "SlideId" });
            DropIndex("dbo.Image", new[] { "ProductId" });
            DropTable("dbo.User");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Slide");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Product");
            DropTable("dbo.NewCategory");
            DropTable("dbo.New");
            DropTable("dbo.Image");
            DropTable("dbo.Footer");
            DropTable("dbo.FeedBack");
            DropTable("dbo.Contact");
            DropTable("dbo.About");
        }
    }
}
