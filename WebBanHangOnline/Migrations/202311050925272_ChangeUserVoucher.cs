namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserVoucher : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_UserVoucher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VoucherId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Voucher", t => t.VoucherId, cascadeDelete: true)
                .Index(t => t.VoucherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_UserVoucher", "VoucherId", "dbo.tb_Voucher");
            DropIndex("dbo.tb_UserVoucher", new[] { "VoucherId" });
            DropTable("dbo.tb_UserVoucher");
        }
    }
}
