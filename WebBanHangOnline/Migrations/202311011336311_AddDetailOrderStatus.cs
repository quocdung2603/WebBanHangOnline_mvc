namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetailOrderStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_DetailOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        IdUConfirm = c.String(),
                        CofirmDate = c.DateTime(nullable: false),
                        IdUExport = c.String(),
                        ExportDate = c.DateTime(nullable: false),
                        IdUDelivery = c.String(),
                        DeliveryDate = c.DateTime(nullable: false),
                        IdUCancel = c.String(),
                        CancelDate = c.DateTime(nullable: false),
                        IdUReturn = c.String(),
                        ReturnDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_DetailOrderStatus", "OrderId", "dbo.tb_Order");
            DropIndex("dbo.tb_DetailOrderStatus", new[] { "OrderId" });
            DropTable("dbo.tb_DetailOrderStatus");
        }
    }
}
