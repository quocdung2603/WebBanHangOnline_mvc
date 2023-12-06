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
    [Authorize(Roles = "Admin")]
    public class ComboController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Combo
        public ActionResult Index(int? page, string SO)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Combo> items = db.Combos.ToList();
            ViewBag.TenCombo = SO == "TenCombo" ? "TenCombod" : "TenCombo";
            ViewBag.NgayTao = SO == "NgayTao" ? "NgayTaod" : "NgayTao";
            switch(SO)
            {
                case "TenCombo": items = items.OrderBy(x => x.Title);break;
                case "TenCombod": items = items.OrderByDescending(x => x.Title); break;
                case "NgayTao": items = items.OrderBy(x => x.CreatedDate); break;
                case "NgayTaod": items = items.OrderByDescending(x => x.CreatedDate); break;
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
            var title = f["TenCombo"];
            var percentdec = f["PhanTramGiam"];
            var quantity = f["SoLuongCombo"];
            decimal priceCombo = decimal.Zero;
            foreach(var p in LProduct)
            {
                priceCombo += p.Price;
            }    
            Combo c = new Combo();
            c.Title = title;
            c.PercentDec = Convert.ToDecimal(percentdec);
            c.Quantity = Convert.ToInt32(quantity);
            c.Price = priceCombo - (priceCombo * c.PercentDec / 100);
            c.IsActive = false;
            c.CreatedDate = DateTime.Now;
            c.ModifierDate = DateTime.Now;
            if (Request.IsAuthenticated)
            {
                c.CreatedBy = User.Identity.Name;
                c.ModifierBy = User.Identity.Name;
            }
            db.Combos.Add(c);
            db.SaveChanges();
            foreach(var item in LProduct)
            {
                ComboDetail cd = new ComboDetail();
                cd.ComboId = c.Id;
                cd.ProductId = item.Id;
                db.ComboDetails.Add(cd);
                db.SaveChanges();
            }    
            return RedirectToAction("Index", "Combo");
        }

        public ActionResult Edit(int id)
        {
            var item = db.Combos.FirstOrDefault(x => x.Id == id);
            List<Product> a = db.Products.ToList();
            ViewBag.p = a;
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<Product> LProduct)
        {
            int cid = Convert.ToInt32(f["Id"]);
            var title = f["TenCombo"];
            var percentdec = f["PhanTramGiam"];
            var quantity = f["SoLuongCombo"];
            DateTime createdDate = DateTime.Parse(f["CreatedDate"]);
            var createdBy = f["CreatedBy"];
            decimal pricecombo = Decimal.Zero;
            foreach(var z in LProduct)
            {
                pricecombo += z.Price;
            }
            var c = db.Combos.FirstOrDefault(x=>x.Id == cid);
            if(c!=null)
            {
                c.Title = title;
                c.Quantity = Convert.ToInt32(quantity);
                c.PercentDec = Convert.ToDecimal(percentdec);
                c.Price = pricecombo - (pricecombo * c.PercentDec / 100);
                c.CreatedBy = createdBy;
                c.CreatedDate = createdDate;
                if(Request.IsAuthenticated)
                {
                    c.ModifierBy = User.Identity.Name;
                }
                c.ModifierDate = DateTime.Now;
                db.SaveChanges();
            }
            for(var i =0; i< LProduct.Count;i++)
            {
                var productId = LProduct[i].Id;
                var cd = db.ComboDetails.FirstOrDefault(x=>x.ProductId == productId && x.ComboId == c.Id);
                if(cd == null)
                {
                    cd = new ComboDetail();
                    cd.ComboId = c.Id;
                    cd.ProductId = productId;
                    db.ComboDetails.Add(cd);
                    db.SaveChanges();
                }    
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Combos.FirstOrDefault(x => x.Id == id);
            if(item!=null)
            {
                db.Combos.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Combos.Find(Convert.ToInt32(item));
                        db.Combos.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public ActionResult Detail(int id)
        {
            var item = db.Combos.FirstOrDefault(x => x.Id == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Combos.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }
    }
}