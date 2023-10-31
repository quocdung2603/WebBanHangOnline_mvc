namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrder_OrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "OrderStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "OrderStatus");
        }
    }
}
