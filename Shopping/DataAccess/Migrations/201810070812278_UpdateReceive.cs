namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReceive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receive", "InvoiveNo", c => c.String(maxLength: 30));
            AddColumn("dbo.Receive", "DocumentDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receive", "DocumentDateTime");
            DropColumn("dbo.Receive", "InvoiveNo");
        }
    }
}
