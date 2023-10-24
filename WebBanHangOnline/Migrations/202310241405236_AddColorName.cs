namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ProductSize", "ColorName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_ProductSize", "ColorName");
        }
    }
}
