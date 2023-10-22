using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductSizeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductSize
        public ActionResult Index(int id)
        {
            TempData["productId"] = id;
            var items = db.ProductSizes.Where(x=> x.ProductId == id); 
            return View(items);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductSize model)
        {
            if (ModelState.IsValid)
            {
                model.ProductId = (int)TempData["productId"];
                db.ProductSizes.Add(model);
                db.SaveChanges();
                return Json(new { success = true});
            }
            return Json(new { success = false });
        }

        public ActionResult Edit(int id)
        {
            var item = db.ProductSizes.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductSize model)
        {
            if (ModelState.IsValid)
            {
                db.ProductSizes.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true});
            }
            return Json(new { success = false});
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductSizes.Find(id);
            if (item != null)
            {
                db.ProductSizes.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}