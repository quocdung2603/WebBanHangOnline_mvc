using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Title = "ABC Store";
            ViewBag.SeoDescription = "ABC Store là shop bán hàng uy tín, chất lượng, giá cả hợp lý.";
            ViewBag.SeoKeyWord = "ABC Store";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    
        public ActionResult Partial_Subcribe()
        {
            return PartialView();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Subcribe(Subscribe req)
        {
            if(ModelState.IsValid)
            {
                db.Subscribes.Add(new Subscribe { 
                    Email = req.Email,
                    CreatedDate = DateTime.Now
                });
                db.SaveChanges();
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send_subcribe.html"));
                contentCustomer = contentCustomer.Replace("{{DiaChiMailKhach}}", req.Email);
                contentCustomer = contentCustomer.Replace("{{DiaChiMailAdmin}}", "nguyenquocdung26032003@gmail.com");
                contentCustomer = contentCustomer.Replace("{{SoDienThoai}}", "0901291640");
                WebBanHangOnline.Common.Common.SendMail("ABC Store", "CẢM ƠN QUÝ KHÁCH ĐÃ ĐĂNG KÝ NHẬN THÔNG BÁO QUA EMAIL", contentCustomer.ToString(), req.Email);
                return Json(new { success = true});
            }    
            return View("Partial_Subcribe",req);
        }

        public ActionResult ShowChat()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var emp = db.Users.FirstOrDefault(x => x.Id == "0e9524ad-a5e6-46e7-9adf-948bffce8f1c");
                List<Message> idnow = db.Messages.Where(item => item.UserId == user.Id).ToList();

                ViewBag.Name = user.FullName;
                ViewBag.UserId = user.Id;
                if (idnow.Count()==0)
                {
                    RoomChat r = new RoomChat();
                    r.Name = "Room" + db.RoomChats.Count() + 1;
                    r.Type = 1;
                    db.RoomChats.Add(r);
                    db.SaveChanges();

                    Message m1 = new Message();
                    m1.UserId = emp.Id;
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
                    List<Message> tmp = db.Messages.Where(item => item.RoomId == r.Id && item.Content != null).ToList();
                    ViewBag.IdRoom = r.Id;
                    return View(tmp);
                }   
                else
                {
                    List<Message> tmp = db.Messages.Where(item => item.UserId == user.Id).ToList();
                    ViewBag.IdRoom = tmp[0].RoomId;
                    int id = tmp[0].RoomId;
                    List<Message> tmp1 = db.Messages.Where(item => item.RoomId == id && item.Content!=null).ToList();
                    return View(tmp1);
                }    

            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}