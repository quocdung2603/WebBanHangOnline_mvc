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
    [Authorize]
    public class ReviewController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if(user !=null)
                {
                    item.Email = user.Email;
                    item.FullName = user.FullName;
                    item.UserName = user.UserName;
                    
                }
                return PartialView(item);
            }    
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult _Load_Review(int productId)
        {
            var item = db.ReviewProducts.Where(x => x.ProductId == productId).OrderByDescending(x=>x.Id).ToList();
            ViewBag.ViewCount = item.Count;
            return PartialView(item);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if(ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                db.ReviewProducts.Add(req);
                db.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(new { success = false });
        }
        public ActionResult LichSuDonHang()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var items = db.Orders.Where(x => x.CustomerId == user.Id).ToList();
                var it = db.Orders.Where(x => x.CustomerId == user.Id);
                decimal s = 0;
                int cnt = 0;
                if(it.Count() > 0)
                {
                    s += it.Sum(x => x.TotalAmount);
                    cnt += it.Count();
                }    
                
                TempData["totalAmount"] = s;
                TempData["TotalCount"] = cnt;
                return PartialView(items);
            }
            return PartialView();
        }

        public ActionResult TinhTrangDonHang()
        {
            return PartialView();
        }
        
        public ActionResult TinhTrang(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var item = db.Orders.Where(x => x.OrderStatus == id && x.CustomerId==user.Id);
                return PartialView(item);
            }
            return PartialView();
        }
    }
}