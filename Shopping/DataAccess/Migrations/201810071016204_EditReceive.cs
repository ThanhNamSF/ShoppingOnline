namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditReceive : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Delivery", "UpdatedDateTime", c => c.DateTime());
            AlterColumn("dbo.Receive", "UpdatedDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Receive", "UpdatedDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Delivery", "UpdatedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
