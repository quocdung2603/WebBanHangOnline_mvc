using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ChatController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Chat
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }
       
        public ActionResult ListRoom()
        {
            HashSet<int> hs = new HashSet<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var tmp = db.Messages.Where(item => item.UserId == user.Id).ToList();
                foreach (var item in tmp)
                {
                    hs.Add(item.RoomId);
                }    
            }
            
            return PartialView(hs);
        }
    }
}