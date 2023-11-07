namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExportProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ExportProductDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExportProductId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Color = c.String(),
                        Size = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_ExportProduct", t => t.ExportProductId, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ExportProductId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.tb_ExportProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Note = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifierDate = c.DateTime(nullable: false),
                        ModifierBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ExportProductDetail", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_ExportProductDetail", "ExportProductId", "dbo.tb_ExportProduct");
            DropIndex("dbo.tb_ExportProductDetail", new[] { "ProductId" });
            DropIndex("dbo.tb_ExportProductDetail", new[] { "ExportProductId" });
            DropTable("dbo.tb_ExportProduct");
            DropTable("dbo.tb_ExportProductDetail");
        }
    }
}
