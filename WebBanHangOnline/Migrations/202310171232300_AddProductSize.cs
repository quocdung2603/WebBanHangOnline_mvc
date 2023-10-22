namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductSize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ProductSize",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        SizeName = c.String(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.tb_Product", "Size", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ProductSize", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_ProductSize", new[] { "ProductId" });
            DropColumn("dbo.tb_Product", "Size");
            DropTable("dbo.tb_ProductSize");
        }
    }
}
