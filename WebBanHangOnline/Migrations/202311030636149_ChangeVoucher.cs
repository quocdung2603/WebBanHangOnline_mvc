namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeVoucher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Voucher", "Title", c => c.String());
            AddColumn("dbo.tb_Voucher", "HSD", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Voucher", "HSD");
            DropColumn("dbo.tb_Voucher", "Title");
        }
    }
}
