using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class TimePromotionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/TimePromotion
        public ActionResult Index(int? page)
        {
            IEnumerable<TimePromotion> items = db.TimePromotions.OrderByDescending(x => x.CreatedDate);
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult Add()
        {
            List<Product> a = db.Products.ToList();
            ViewBag.p = a;
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection f, List<Product> LProduct)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                DateTime sd = Convert.ToDateTime(f["StartDate"]);
                sd = sd.AddHours(Convert.ToInt32(f["SHour"]));
                sd = sd.AddMinutes(Convert.ToInt32(f["SMinute"]));
                DateTime ed = Convert.ToDateTime(f["EndDate"]);
                ed = ed.AddHours(Convert.ToInt32(f["EHour"]));
                ed = ed.AddMinutes(Convert.ToInt32(f["EMinute"]));
                TimePromotion tp = new TimePromotion();
                tp.Title = f["TenKM"];
                tp.StartDate = sd;
                tp.EndDate = ed;
                tp.PercentValue = Convert.ToDecimal(f["PercentPrice"]);
                tp.CreatedDate = DateTime.Now;
                tp.CreatedBy = user.FullName;
                tp.ModifierDate = DateTime.Now;
                tp.ModifierBy = user.FullName;
                tp.IsActive = false;
                tp.IsBan = false;
                var tmp = DateTime.Now;
                if(tmp >= tp.StartDate && tmp <=tp.EndDate)
                {
                    tp.IsActive = true;
                }
                db.TimePromotions.Add(tp);
                db.SaveChanges();
                for(var i =0; i < LProduct.Count; i++)
                {
                    var productId = LProduct[i].Id;
                    TimePromotionDetail tpd = new TimePromotionDetail();
                    tpd.ProductId = productId;
                    tpd.TimePromotionId = tp.Id;
                    db.TimePromotionDetails.Add(tpd);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }    
            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.TimePromotions.FirstOrDefault(x=>x.Id == id);
            List<Product> a = db.Products.ToList();
            ViewBag.p = a;
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(FormCollection f, List<Product> LProduct)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                int tpId = Convert.ToInt32(f["Id"]);
                DateTime sd = Convert.ToDateTime(f["StartDate"]);
                sd = sd.AddHours(Convert.ToInt32(f["SHour"]));
                sd = sd.AddMinutes(Convert.ToInt32(f["SMinute"]));
                DateTime ed = Convert.ToDateTime(f["EndDate"]);
                ed = ed.AddHours(Convert.ToInt32(f["EHour"]));
                ed = ed.AddMinutes(Convert.ToInt32(f["EMinute"]));

                var tp = db.TimePromotions.FirstOrDefault(x => x.Id == tpId);
                if(tp!=null)
                {
                    tp.Title = f["TenKM"];
                    tp.StartDate = sd;
                    tp.EndDate = ed;
                    tp.PercentValue = Convert.ToDecimal(f["PercentPrice"]);
                    tp.ModifierDate = DateTime.Now;
                    tp.ModifierBy = user.FullName;
                    tp.IsActive = false;
                    tp.IsBan = false;
                    var tmp = DateTime.Now;
                    if (tmp >= tp.StartDate && tmp <= tp.EndDate)
                    {
                        tp.IsActive = true;
                    }
                    db.SaveChanges();
                    for (var i = 0; i < LProduct.Count; i++)
                    {
                        var productId = LProduct[i].Id;
                        var tpd = db.TimePromotionDetails.FirstOrDefault(x=>x.TimePromotionId == tp.Id && x.ProductId == productId);
                        if(tpd == null)
                        {
                            tpd = new TimePromotionDetail();
                            tpd.ProductId = productId;
                            tpd.TimePromotionId = tp.Id;
                            db.TimePromotionDetails.Add(tpd);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult IsBan(int id)
        {
            var item = db.TimePromotions.Find(id);
            if (item != null)
            {
                item.IsBan = !item.IsBan;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isBan = item.IsBan });
            }
            return Json(new { success = false });
        }
    }
}