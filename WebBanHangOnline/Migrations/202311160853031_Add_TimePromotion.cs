namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TimePromotion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_TimePromotionDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimePromotionId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.tb_TimePromotion", t => t.TimePromotionId, cascadeDelete: true)
                .Index(t => t.TimePromotionId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.tb_TimePromotion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsBan = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifierDate = c.DateTime(nullable: false),
                        ModifierBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_TimePromotionDetail", "TimePromotionId", "dbo.tb_TimePromotion");
            DropForeignKey("dbo.tb_TimePromotionDetail", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_TimePromotionDetail", new[] { "ProductId" });
            DropIndex("dbo.tb_TimePromotionDetail", new[] { "TimePromotionId" });
            DropTable("dbo.tb_TimePromotion");
            DropTable("dbo.tb_TimePromotionDetail");
        }
    }
}
