namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditProductUpdateBy : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Product", "UpdatedBy", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "UpdatedBy", c => c.Int(nullable: false));
        }
    }
}
