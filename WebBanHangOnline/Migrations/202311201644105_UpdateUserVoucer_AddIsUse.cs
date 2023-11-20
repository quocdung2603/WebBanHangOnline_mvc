namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserVoucer_AddIsUse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_UserVoucher", "IsUse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_UserVoucher", "IsUse");
        }
    }
}
