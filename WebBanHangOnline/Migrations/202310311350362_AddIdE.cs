namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "IdEConform", c => c.String());
            AddColumn("dbo.tb_Order", "IdEExport", c => c.String());
            AddColumn("dbo.tb_Order", "IdEDelivery", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "IdEDelivery");
            DropColumn("dbo.tb_Order", "IdEExport");
            DropColumn("dbo.tb_Order", "IdEConform");
        }
    }
}
