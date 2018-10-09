namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTables : DbMigration
    {
        public override void Up()
        {
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
                        CreatedBy = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ApprovedBy);
            
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
                        Deleted = c.Boolean(nullable: false),
                        Delivery_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Receive", t => t.ReceiveId, cascadeDelete: true)
                .ForeignKey("dbo.Delivery", t => t.Delivery_Id)
                .Index(t => t.ReceiveId)
                .Index(t => t.ProductId)
                .Index(t => t.Delivery_Id);
            
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
                        CreatedBy = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedDateTime = c.DateTime(nullable: false),
                        ApprovedBy = c.Int(),
                        ApprovedDateTime = c.DateTime(),
                        Status = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApprovedBy)
                .Index(t => t.Code, unique: true)
                .Index(t => t.ApprovedBy);
            
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
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Delivery", t => t.DeliveryId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.DeliveryId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.DeliveryDetail", "DeliveryId", "dbo.Delivery");
            DropForeignKey("dbo.ReceiveDetail", "Delivery_Id", "dbo.Delivery");
            DropForeignKey("dbo.ReceiveDetail", "ReceiveId", "dbo.Receive");
            DropForeignKey("dbo.Receive", "ApprovedBy", "dbo.User");
            DropForeignKey("dbo.ReceiveDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Delivery", "ApprovedBy", "dbo.User");
            DropIndex("dbo.DeliveryDetail", new[] { "ProductId" });
            DropIndex("dbo.DeliveryDetail", new[] { "DeliveryId" });
            DropIndex("dbo.Receive", new[] { "ApprovedBy" });
            DropIndex("dbo.Receive", new[] { "Code" });
            DropIndex("dbo.ReceiveDetail", new[] { "Delivery_Id" });
            DropIndex("dbo.ReceiveDetail", new[] { "ProductId" });
            DropIndex("dbo.ReceiveDetail", new[] { "ReceiveId" });
            DropIndex("dbo.Delivery", new[] { "ApprovedBy" });
            DropIndex("dbo.Delivery", new[] { "Code" });
            DropTable("dbo.DeliveryDetail");
            DropTable("dbo.Receive");
            DropTable("dbo.ReceiveDetail");
            DropTable("dbo.Delivery");
        }
    }
}
