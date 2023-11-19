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
                var items = db.Orders.Where(x => x.CustomerId == user.Id && x.OrderStatus == 3).ToList();
                var it = db.Orders.Where(x => x.CustomerId == user.Id);
                decimal s = 0;
                int cnt = 0;
                if(it.Count() > 0)
                {
                    s += it.Sum(x => x.TotalAmount);
                    cnt += it.Count();
                }    
                
                Session["totalAmount"] = s;
                Session["TotalCount"] = cnt;
                return PartialView(items);
            }
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

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var item = db.Orders.Find(id);
                if (item != null)
                {
                    if (item.OrderStatus == 0)
                    {
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                        dos.IdUCancel = user.Id;
                        dos.CancelDate = DateTime.Now;
                        db.SaveChanges();
                        item.OrderStatus = -1;
                    }
                    var od = db.OrderDetails.Where(x => x.OrderId == item.Id).ToList();
                    if(od != null)
                    {
                        foreach(var z in od)
                        {
                            var ps = db.ProductSizes.FirstOrDefault(x => x.ProductId == z.ProductId && x.SizeName == z.ProductSize && x.ColorName == z.ProductColor);
                            if(string.IsNullOrEmpty(z.ProductSize))
                            {
                                ps = db.ProductSizes.FirstOrDefault(x => x.ProductId == z.ProductId && x.ColorName == z.ProductColor);
                            }    
                            if(string.IsNullOrEmpty(z.ProductColor))
                            {
                                ps = db.ProductSizes.FirstOrDefault(x => x.ProductId == z.ProductId && x.SizeName == z.ProductSize);
                            }    
                            if(ps!=null)
                            {
                                ps.Quantity += z.Quantity;
                                db.OrderDetails.Remove(z);
                                db.SaveChanges();
                            }    
                        }    
                    }    
                    db.Orders.Attach(item);
                    db.Entry(item).Property(x => x.OrderStatus).IsModified = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult ReturnOrder(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var item = db.Orders.Find(id);
                if (item != null)
                {
                    if (item.OrderStatus == 3)
                    {
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                        dos.IdUReturn = dos.IdUDelivery;
                        dos.ReturnDate = DateTime.Now;
                        List<OrderDetail> od = db.OrderDetails.Where(x => x.OrderId == item.Id).ToList();
                        foreach (var i in od)
                        {
                            Product p = db.Products.FirstOrDefault(x => x.Id == i.ProductId);
                            p.Quantity += i.Quantity;
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                        item.OrderStatus +=1;
                    }
                    db.Orders.Attach(item);
                    db.Entry(item).Property(x => x.OrderStatus).IsModified = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
    }
}