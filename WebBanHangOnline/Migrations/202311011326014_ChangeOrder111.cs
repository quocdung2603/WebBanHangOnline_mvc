namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrder111 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_Order", "IdEConform");
            DropColumn("dbo.tb_Order", "IdEExport");
            DropColumn("dbo.tb_Order", "IdEDelivery");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "IdEDelivery", c => c.String());
            AddColumn("dbo.tb_Order", "IdEExport", c => c.String());
            AddColumn("dbo.tb_Order", "IdEConform", c => c.String());
        }
    }
}
