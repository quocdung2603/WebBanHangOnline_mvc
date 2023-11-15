namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Quantity_Combo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Combo", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Combo", "Quantity");
        }
    }
}
