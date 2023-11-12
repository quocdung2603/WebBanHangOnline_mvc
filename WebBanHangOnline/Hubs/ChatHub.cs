using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
namespace WebBanHangOnline.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Connect(string name)
        {
            Clients.Caller.name(name);
        }
        public void Message(string name, string message)
        {
            Clients.All.message(name, message);

        }
    }
}