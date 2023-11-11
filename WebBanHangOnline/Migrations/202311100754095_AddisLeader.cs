namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddisLeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsLeader", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsLeader");
        }
    }
}
