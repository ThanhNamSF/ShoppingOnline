namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLength : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Delivery", new[] { "Code" });
            DropIndex("dbo.Order", new[] { "Code" });
            DropIndex("dbo.Product", new[] { "Code" });
            DropIndex("dbo.Receive", new[] { "Code" });
            AlterColumn("dbo.Delivery", "Code", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Order", "Code", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Product", "Code", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Receive", "Code", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.Delivery", "Code", unique: true);
            CreateIndex("dbo.Order", "Code", unique: true);
            CreateIndex("dbo.Product", "Code", unique: true);
            CreateIndex("dbo.Receive", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Receive", new[] { "Code" });
            DropIndex("dbo.Product", new[] { "Code" });
            DropIndex("dbo.Order", new[] { "Code" });
            DropIndex("dbo.Delivery", new[] { "Code" });
            AlterColumn("dbo.Receive", "Code", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Product", "Code", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Order", "Code", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Delivery", "Code", c => c.String(nullable: false, maxLength: 6));
            CreateIndex("dbo.Receive", "Code", unique: true);
            CreateIndex("dbo.Product", "Code", unique: true);
            CreateIndex("dbo.Order", "Code", unique: true);
            CreateIndex("dbo.Delivery", "Code", unique: true);
        }
    }
}
