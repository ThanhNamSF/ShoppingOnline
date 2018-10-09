namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteImageEnitity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Image", "NewId", "dbo.New");
            DropForeignKey("dbo.Image", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Image", "SlideId", "dbo.Slide");
            DropIndex("dbo.Image", new[] { "ProductId" });
            DropIndex("dbo.Image", new[] { "SlideId" });
            DropIndex("dbo.Image", new[] { "NewId" });
            AddColumn("dbo.Product", "ImagePath", c => c.String());
            DropTable("dbo.Image");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        ProductId = c.Int(),
                        SlideId = c.Int(),
                        NewId = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Product", "ImagePath");
            CreateIndex("dbo.Image", "NewId");
            CreateIndex("dbo.Image", "SlideId");
            CreateIndex("dbo.Image", "ProductId");
            AddForeignKey("dbo.Image", "SlideId", "dbo.Slide", "Id");
            AddForeignKey("dbo.Image", "ProductId", "dbo.Product", "Id");
            AddForeignKey("dbo.Image", "NewId", "dbo.New", "Id");
        }
    }
}
