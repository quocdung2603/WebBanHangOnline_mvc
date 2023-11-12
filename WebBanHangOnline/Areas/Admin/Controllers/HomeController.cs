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
                    var tmp = db.Messages.Where(item => item.UserId == user.Id && item.RoomId == id).ToList();
                    return View(tmp);
                }
            }    
            return View();
        }
       
    }
}