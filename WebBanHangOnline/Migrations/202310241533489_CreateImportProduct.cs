namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImportProduct : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifierDate = c.DateTime(nullable: false),
                        ModifierBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tb_Product", "ImportProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Product", "ImportProductId");
            AddForeignKey("dbo.tb_Product", "ImportProductId", "dbo.ImportProducts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Product", "ImportProductId", "dbo.ImportProducts");
            DropIndex("dbo.tb_Product", new[] { "ImportProductId" });
            DropColumn("dbo.tb_Product", "ImportProductId");
            DropTable("dbo.ImportProducts");
        }
    }
}
