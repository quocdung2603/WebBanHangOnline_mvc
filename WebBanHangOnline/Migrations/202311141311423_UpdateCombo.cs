namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCombo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Combo", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Combo", "Title", c => c.Int(nullable: false));
        }
    }
}
