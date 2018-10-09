namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRêciveDelivery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Delivery", "InvoiveNo", c => c.String(maxLength: 30));
            DropColumn("dbo.Receive", "InvoiveNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Receive", "InvoiveNo", c => c.String(maxLength: 30));
            DropColumn("dbo.Delivery", "InvoiveNo");
        }
    }
}
