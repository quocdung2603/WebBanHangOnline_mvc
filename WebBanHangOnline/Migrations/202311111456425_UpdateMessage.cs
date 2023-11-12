namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_Message", "RoomChats_Id", "dbo.tb_RoomChat");
            DropIndex("dbo.tb_Message", new[] { "RoomChats_Id" });
            DropColumn("dbo.tb_Message", "RoomId");
            RenameColumn(table: "dbo.tb_Message", name: "RoomChats_Id", newName: "RoomId");
            AlterColumn("dbo.tb_Message", "RoomId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Message", "RoomId");
            AddForeignKey("dbo.tb_Message", "RoomId", "dbo.tb_RoomChat", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Message", "RoomId", "dbo.tb_RoomChat");
            DropIndex("dbo.tb_Message", new[] { "RoomId" });
            AlterColumn("dbo.tb_Message", "RoomId", c => c.Int());
            RenameColumn(table: "dbo.tb_Message", name: "RoomId", newName: "RoomChats_Id");
            AddColumn("dbo.tb_Message", "RoomId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Message", "RoomChats_Id");
            AddForeignKey("dbo.tb_Message", "RoomChats_Id", "dbo.tb_RoomChat", "Id");
        }
    }
}
