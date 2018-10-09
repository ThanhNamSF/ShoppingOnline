namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            AddColumn("dbo.Order", "ReceiverName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Order", "ReceiverAddress", c => c.String(nullable: false));
            AddColumn("dbo.Order", "ReceiverPhone", c => c.String(nullable: false, maxLength: 12));
            AddColumn("dbo.Order", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.Order", "ApprovedDateTime", c => c.DateTime());
            AddColumn("dbo.Order", "ReceivedDateTime", c => c.DateTime());
            AddColumn("dbo.Order", "ApprovedId", c => c.Int());
            AddColumn("dbo.Order", "DiliveredId", c => c.Int());
            AddColumn("dbo.Order", "User_Id", c => c.Int());
            CreateIndex("dbo.Order", "ApprovedId");
            CreateIndex("dbo.Order", "DiliveredId");
            CreateIndex("dbo.Order", "User_Id");
            AddForeignKey("dbo.Order", "ApprovedId", "dbo.User", "Id");
            AddForeignKey("dbo.Order", "DiliveredId", "dbo.User", "Id");
            AddForeignKey("dbo.Order", "User_Id", "dbo.User", "Id");
            DropColumn("dbo.About", "CreatedDateTime");
            DropColumn("dbo.About", "CreatedBy");
            DropColumn("dbo.About", "UpdatedDateTime");
            DropColumn("dbo.About", "UpdatedBy");
            DropColumn("dbo.OrderDetail", "Amount");
            DropColumn("dbo.Order", "CustomerName");
            DropColumn("dbo.Order", "CustomerAddress");
            DropColumn("dbo.Order", "CustomerPhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "CustomerPhone", c => c.String(nullable: false, maxLength: 12));
            AddColumn("dbo.Order", "CustomerAddress", c => c.String(nullable: false));
            AddColumn("dbo.Order", "CustomerName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.OrderDetail", "Amount", c => c.Double(nullable: false));
            AddColumn("dbo.About", "UpdatedBy", c => c.Int());
            AddColumn("dbo.About", "UpdatedDateTime", c => c.DateTime());
            AddColumn("dbo.About", "CreatedBy", c => c.Int(nullable: false));
            AddColumn("dbo.About", "CreatedDateTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Order", "User_Id", "dbo.User");
            DropForeignKey("dbo.Order", "DiliveredId", "dbo.User");
            DropForeignKey("dbo.Order", "ApprovedId", "dbo.User");
            DropIndex("dbo.Order", new[] { "User_Id" });
            DropIndex("dbo.Order", new[] { "DiliveredId" });
            DropIndex("dbo.Order", new[] { "ApprovedId" });
            DropColumn("dbo.Order", "User_Id");
            DropColumn("dbo.Order", "DiliveredId");
            DropColumn("dbo.Order", "ApprovedId");
            DropColumn("dbo.Order", "ReceivedDateTime");
            DropColumn("dbo.Order", "ApprovedDateTime");
            DropColumn("dbo.Order", "Status");
            DropColumn("dbo.Order", "ReceiverPhone");
            DropColumn("dbo.Order", "ReceiverAddress");
            DropColumn("dbo.Order", "ReceiverName");
            AddForeignKey("dbo.Order", "UserId", "dbo.User", "Id", cascadeDelete: true);
        }
    }
}
