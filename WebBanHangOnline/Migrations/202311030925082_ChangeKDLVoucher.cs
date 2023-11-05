namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeKDLVoucher : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Voucher", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.tb_Voucher", "PercentValue", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Voucher", "PercentValue", c => c.Double(nullable: false));
            AlterColumn("dbo.tb_Voucher", "Value", c => c.Double(nullable: false));
        }
    }
}
