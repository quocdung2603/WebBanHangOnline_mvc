namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCancelReason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_DetailOrderStatus", "CancelReason", c => c.String());
            AddColumn("dbo.tb_DetailOrderStatus", "ReturnReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_DetailOrderStatus", "ReturnReason");
            DropColumn("dbo.tb_DetailOrderStatus", "CancelReason");
        }
    }
}
