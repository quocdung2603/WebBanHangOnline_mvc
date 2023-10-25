namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImportP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportProducts", "Title", c => c.String());
            AddColumn("dbo.ImportProducts", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportProducts", "Note");
            DropColumn("dbo.ImportProducts", "Title");
        }
    }
}
