using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee,StoreKeeper,Shipper")]
    //[Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Home
        public ActionResult Index(int? id)
        {
            if(id==null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var user = userManager.FindByName(User.Identity.Name);
                    ViewBag.Name = user.FullName;
                }
            }    
           else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var user = userManager.FindByName(User.Identity.Name);
                    ViewBag.Name = user.FullName;
                    ViewBag.UserId = user.Id;
                    var tmp = db.Messages.Where(item => item.RoomId == id && item.Content!=null).ToList();

                    ViewBag.IdRoom = id;
                    return View(tmp);
                }
            }
            ViewBag.A = db.Orders.ToList().Count();
            ViewBag.B = db.Orders.Where(x => x.OrderStatus == 3).Count();
            double tmp1 = Convert.ToInt32(ViewBag.B)/ Convert.ToInt32(ViewBag.A) *100;
            ViewBag.B = tmp1;
            var userStore1 = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager1 = new UserManager<ApplicationUser>(userStore1);
            var u = db.Users.ToList();
            List<ApplicationUser> lstUser = new List<ApplicationUser>();
            foreach(var item in u)
            {
                var ru = userManager1.GetRoles(item.Id).ElementAt(0);
                if(ru == "Customer")
                {
                    lstUser.Add(item);
                }
            }
            ViewBag.C = lstUser.Count();
            return View();
        }
       
    }
}