namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitleExportProductDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ExportProductDetail", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_ExportProductDetail", "Title");
        }
    }
}
