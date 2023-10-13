namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ForeignKey_tb_Review : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_Review", "ProductId");
            AddForeignKey("dbo.tb_Review", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Review", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_Review", new[] { "ProductId" });
        }
    }
}
