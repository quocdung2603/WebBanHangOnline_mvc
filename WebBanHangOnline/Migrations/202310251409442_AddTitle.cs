namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ImportProductDetail", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_ImportProductDetail", "Title");
        }
    }
}
