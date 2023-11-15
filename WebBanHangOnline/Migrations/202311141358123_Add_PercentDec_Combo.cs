namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PercentDec_Combo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Combo", "PercentDec", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Combo", "PercentDec");
        }
    }
}
