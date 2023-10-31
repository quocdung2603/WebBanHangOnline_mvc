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
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index(string Searchtext, int ? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ProductCategory> items = db.ProductCategories.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProductCategory model)
        {
            if(ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Commons.Filter.FilterChar(model.Title);
                db.ProductCategories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }    
            return View();
        }
    }

}