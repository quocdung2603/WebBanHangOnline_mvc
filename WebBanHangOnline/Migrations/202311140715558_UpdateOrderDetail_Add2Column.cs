namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderDetail_Add2Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_OrderDetail", "ProductSize", c => c.String());
            AddColumn("dbo.tb_OrderDetail", "ProductColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_OrderDetail", "ProductColor");
            DropColumn("dbo.tb_OrderDetail", "ProductSize");
        }
    }
}
