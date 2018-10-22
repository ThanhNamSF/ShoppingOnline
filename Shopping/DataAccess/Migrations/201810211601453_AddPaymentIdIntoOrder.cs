namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentIdIntoOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "PaymentId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "PaymentId");
        }
    }
}
