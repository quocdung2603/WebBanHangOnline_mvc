namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportProductDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_Product", "ImportProductId", "dbo.ImportProducts");
            DropIndex("dbo.tb_Product", new[] { "ImportProductId" });
            CreateTable(
                "dbo.tb_ImportProductDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportProductId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Color = c.String(),
                        Size = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ImportProducts", t => t.ImportProductId, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ImportProductId)
                .Index(t => t.ProductId);
            
            DropColumn("dbo.tb_Product", "ImportProductId");
            DropColumn("dbo.ImportProducts", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImportProducts", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Product", "ImportProductId", c => c.Int(nullable: false));
            DropForeignKey("dbo.tb_ImportProductDetail", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_ImportProductDetail", "ImportProductId", "dbo.ImportProducts");
            DropIndex("dbo.tb_ImportProductDetail", new[] { "ProductId" });
            DropIndex("dbo.tb_ImportProductDetail", new[] { "ImportProductId" });
            DropTable("dbo.tb_ImportProductDetail");
            CreateIndex("dbo.tb_Product", "ImportProductId");
            AddForeignKey("dbo.tb_Product", "ImportProductId", "dbo.ImportProducts", "Id", cascadeDelete: true);
        }
    }
}
