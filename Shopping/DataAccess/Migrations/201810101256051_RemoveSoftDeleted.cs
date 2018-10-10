namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSoftDeleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.About", "Deleted");
            DropColumn("dbo.Contact", "Deleted");
            DropColumn("dbo.Delivery", "Deleted");
            DropColumn("dbo.User", "Deleted");
            DropColumn("dbo.Order", "Deleted");
            DropColumn("dbo.OrderDetail", "Deleted");
            DropColumn("dbo.Product", "Deleted");
            DropColumn("dbo.ProductCategory", "Deleted");
            DropColumn("dbo.DeliveryDetail", "Deleted");
            DropColumn("dbo.FeedBack", "Deleted");
            DropColumn("dbo.Footer", "Deleted");
            DropColumn("dbo.NewCategory", "Deleted");
            DropColumn("dbo.New", "Deleted");
            DropColumn("dbo.ReceiveDetail", "Deleted");
            DropColumn("dbo.Receive", "Deleted");
            DropColumn("dbo.Slide", "Deleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slide", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Receive", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReceiveDetail", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.New", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.NewCategory", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Footer", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.FeedBack", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.DeliveryDetail", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductCategory", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Product", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderDetail", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Delivery", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Contact", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.About", "Deleted", c => c.Boolean(nullable: false));
        }
    }
}
