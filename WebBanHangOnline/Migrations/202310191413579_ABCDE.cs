namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ABCDE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Product", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Product", "CategoryId");
            AddForeignKey("dbo.tb_Product", "CategoryId", "dbo.tb_Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Product", "CategoryId", "dbo.tb_Category");
            DropIndex("dbo.tb_Product", new[] { "CategoryId" });
            DropColumn("dbo.tb_Product", "CategoryId");
        }
    }
}
