using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
namespace WebBanHangOnline.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Hello()
        {
            Clients.All.hello();
        }
        public async Task JoinRoom(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);  
        }
        public void Connect(string name)
        {
            Clients.Caller.name(name);
        }
        public void Message(string name, string message,string id,string UserId)
        {
            Message m1 = new Message();
            m1.UserId = UserId;
            m1.TimesChat = DateTime.Now;
            m1.Content = message;
            m1.RoomId = int.Parse(id);
            db.Messages.Add(m1);

            db.SaveChanges();
            Clients.Group(id).message(name, message);

        }
    }
}