namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCombo_ComboDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_ComboDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComboId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Combo", t => t.ComboId, cascadeDelete: true)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ComboId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.tb_Combo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifierDate = c.DateTime(nullable: false),
                        ModifierBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ComboDetail", "ProductId", "dbo.tb_Product");
            DropForeignKey("dbo.tb_ComboDetail", "ComboId", "dbo.tb_Combo");
            DropIndex("dbo.tb_ComboDetail", new[] { "ProductId" });
            DropIndex("dbo.tb_ComboDetail", new[] { "ComboId" });
            DropTable("dbo.tb_Combo");
            DropTable("dbo.tb_ComboDetail");
        }
    }
}
