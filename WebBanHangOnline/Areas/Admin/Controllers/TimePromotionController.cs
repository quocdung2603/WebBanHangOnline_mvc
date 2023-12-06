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
        public ActionResult Index(int? page, string SO)
        {
            IEnumerable<TimePromotion> items = db.TimePromotions.ToList();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            ViewBag.TenTieuDe = SO == "TenCombo" ? "TenCombod" : "TenCombo";
            ViewBag.NgayTao = SO == "NgayTao" ? "NgayTaod" : "NgayTao";
            ViewBag.TinhTrang = SO == "NguoiTao" ? "NguoiTaod" : "NguoiTao";
            ViewBag.Khoa = SO == "Khoa" ? "Khoad" : "Khoa";
            switch (SO)
            {
                case "TenCombo": items = items.OrderBy(x => x.Title); break;
                case "TenCombod": items = items.OrderByDescending(x => x.Title); break;
                case "NgayTao": items = items.OrderBy(x => x.CreatedDate); break;
                case "NgayTaod": items = items.OrderByDescending(x => x.CreatedDate); break;
                case "NguoiTao": items = items.OrderBy(x => x.IsActive); break;
                case "NguoiTaod": items = items.OrderByDescending(x => x.IsActive); break;
                case "Khoa": items = items.OrderBy(x => x.IsBan);break;
                case "Khoad": items = items.OrderByDescending(x => x.IsBan); break;
                default: break;
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

        public ActionResult Detail(int id)
        {
            var item = db.TimePromotions.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost] 
        public ActionResult Delete(int id)
        {
            var item = db.TimePromotions.FirstOrDefault(x => x.Id == id);
            if(item!= null)
            {
                db.TimePromotions.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if(!string.IsNullOrEmpty(ids))
            {
                var data = ids.Split(',');
                foreach(var item in data)
                {
                    var _id = Convert.ToInt32(item);
                    var tp = db.TimePromotions.FirstOrDefault(x => x.Id == _id);
                    if (tp != null)
                    {
                        db.TimePromotions.Remove(tp);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
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


        [HttpGet]
        public ActionResult ApplyPromotion()
        {
            var tp = db.TimePromotions.ToList();
            var dtn = DateTime.Now;
            foreach (var item in tp)
            {
                if (dtn >= item.StartDate && dtn <= item.EndDate)
                {
                    item.IsActive = true;
                    db.SaveChanges();
                }
                else
                {
                    item.IsActive = false;
                    db.SaveChanges();
                    var tpd = db.TimePromotionDetails.Where(x => x.TimePromotionId == item.Id).ToList();
                    if(tpd!=null)
                    {
                        foreach(var t in tpd)
                        {
                            var p = db.Products.FirstOrDefault(x => x.Id == t.ProductId);
                            if(p!=null)
                            {
                                p.PriceSale = decimal.Zero;
                                p.IsSale = false;
                                db.SaveChanges();
                            }
                        }
                    }

                }
            }

            tp = db.TimePromotions.Where(x => x.IsActive == true).ToList();
            if (tp != null)
            {
                foreach (var item in tp)
                {
                    var tpd = db.TimePromotionDetails.Where(x => x.TimePromotionId == item.Id).ToList();
                    if (tpd != null)
                    {
                        foreach (var t in tpd)
                        {
                            var p = db.Products.FirstOrDefault(x => x.Id == t.ProductId);
                            if (p != null)
                            {
                                p.PriceSale = p.Price * (1 - item.PercentValue / 100);
                                p.IsSale = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            return Json(new { success = true });
        }
    }
}