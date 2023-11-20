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
                ViewBag.NowId = user.Id;
            }
            
            return PartialView(hs);
        }

        //chon user để nhắn tin => tạo roomchat mới

        public ActionResult ChoosePerson()
        {
            var item = db.Users.ToList();
            ViewBag.Users = new SelectList(item, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public ActionResult ChoosePerson(string idUser) // thang B
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name); // thang A

                List<Message> idnow = db.Messages.Where(item => item.UserId == user.Id).ToList();

                foreach (var item in idnow)
                {
                    Message check = db.Messages.FirstOrDefault(x => x.RoomId == item.RoomId && x.UserId == idUser);
                    if (check != null)
                    {
                        return RedirectToAction("Index", "Home", new { id = check.RoomId });
                    }
                }
                /*Tạo room mới và thêm 2 đoạn tin nhắn null vào*/
                RoomChat r = new RoomChat();
                r.Name = "Room" + db.RoomChats.Count()+1;
                r.Type = 1;
                db.RoomChats.Add(r);
                db.SaveChanges();

                Message m1 = new Message();
                m1.UserId = idUser;
                m1.TimesChat = DateTime.Now;
                m1.Content = null;
                m1.RoomId = r.Id;

                Message m2 = new Message();
                m2.UserId = user.Id;
                m2.TimesChat = DateTime.Now;
                m2.Content = null;
                m2.RoomId = r.Id;
                db.Messages.Add(m1);
                db.Messages.Add(m2);

                db.SaveChanges();

                return RedirectToAction("Index", "Home",new { id=r.Id});
            }    
            return View();
        }
    }
}