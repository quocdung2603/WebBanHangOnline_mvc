namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStart_End_Date_Voucher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Voucher", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Voucher", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.tb_Voucher", "HSD");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Voucher", "HSD", c => c.DateTime(nullable: false));
            DropColumn("dbo.tb_Voucher", "EndDate");
            DropColumn("dbo.tb_Voucher", "StartDate");
        }
    }
}
