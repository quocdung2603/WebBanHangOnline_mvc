namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoomChatMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        RoomId = c.Int(nullable: false),
                        TimesChat = c.DateTime(nullable: false),
                        Content = c.String(),
                        RoomChats_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tb_RoomChat", t => t.RoomChats_Id)
                .Index(t => t.RoomChats_Id);
            
            CreateTable(
                "dbo.tb_RoomChat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Message", "RoomChats_Id", "dbo.tb_RoomChat");
            DropIndex("dbo.tb_Message", new[] { "RoomChats_Id" });
            DropTable("dbo.tb_RoomChat");
            DropTable("dbo.tb_Message");
        }
    }
}
