namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCombo_Add_IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Combo", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Combo", "IsActive");
        }
    }
}
