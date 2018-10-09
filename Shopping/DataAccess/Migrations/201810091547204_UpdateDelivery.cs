namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDelivery : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReceiveDetail", "Delivery_Id", "dbo.Delivery");
            DropIndex("dbo.ReceiveDetail", new[] { "Delivery_Id" });
            AddColumn("dbo.Delivery", "DocumentDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.ReceiveDetail", "Delivery_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiveDetail", "Delivery_Id", c => c.Int());
            DropColumn("dbo.Delivery", "DocumentDateTime");
            CreateIndex("dbo.ReceiveDetail", "Delivery_Id");
            AddForeignKey("dbo.ReceiveDetail", "Delivery_Id", "dbo.Delivery", "Id");
        }
    }
}
