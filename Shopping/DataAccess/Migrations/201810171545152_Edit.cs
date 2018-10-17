namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Order", name: "DiliveredId", newName: "DeliveredId");
            RenameIndex(table: "dbo.Order", name: "IX_DiliveredId", newName: "IX_DeliveredId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Order", name: "IX_DeliveredId", newName: "IX_DiliveredId");
            RenameColumn(table: "dbo.Order", name: "DeliveredId", newName: "DiliveredId");
        }
    }
}
