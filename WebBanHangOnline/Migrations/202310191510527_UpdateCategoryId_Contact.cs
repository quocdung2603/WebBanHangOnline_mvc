namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCategoryId_Contact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Contact", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Contact", "CategoryId");
            AddForeignKey("dbo.tb_Contact", "CategoryId", "dbo.tb_Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Contact", "CategoryId", "dbo.tb_Category");
            DropIndex("dbo.tb_Contact", new[] { "CategoryId" });
            DropColumn("dbo.tb_Contact", "CategoryId");
        }
    }
}
