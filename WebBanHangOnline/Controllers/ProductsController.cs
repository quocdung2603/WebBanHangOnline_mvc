using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using PagedList;
using PagedList.Mvc;

namespace WebBanHangOnline.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index(string Searchtext, int? page, string IsHot, string IsSale, string IsFeature)
        {
            var pageSize = 8;
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.CreatedDate);
            if (page == null)
            {
                page = 1;
            }
            //khung tìm kiếm
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            } 
            //Lọc theo tiêu chí
            if(!string.IsNullOrEmpty(IsHot) && IsHot =="true")
            {
                items = items.Where(x => x.IsHot == true);
            }
            if (!string.IsNullOrEmpty(IsSale) && IsSale == "true")
            {
                items = items.Where(x => x.IsSale == true);
            }
            if (!string.IsNullOrEmpty(IsFeature) && IsFeature == "true")
            {
                items = items.Where(x => x.IsFeature == true);
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
       
        public ActionResult Detail(string alias, int id)
        {
            var item = db.Products.Find(id);
            if(item!=null)
            {
                db.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                db.Entry(item).Property(x => x.ViewCount).IsModified = true;
                db.SaveChanges();
            }
            var countReview = db.ReviewProducts.Where(x => x.ProductId == id).Count();
            ViewBag.CountReview = countReview;
            return View(item);
        }
        public ActionResult ProductCategory(string alias, int id, int? page, string Searchtext, string IsHot, string IsSale, string IsFeature)
        {
            var pageSize = 8;
            IEnumerable<Product> items = db.Products.ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            //khung tìm kiếm
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            //lọc theo tiêu chí
            if (!string.IsNullOrEmpty(IsHot) && IsHot == "true")
            {
                items = items.Where(x => x.IsHot == true && x.ProductCategoryId == id);
            }
            if (!string.IsNullOrEmpty(IsSale) && IsSale == "true")
            {
                items = items.Where(x => x.IsSale == true && x.ProductCategoryId == id);
            }
            if (!string.IsNullOrEmpty(IsFeature) && IsFeature == "true")
            {
                items = items.Where(x => x.IsFeature == true && x.ProductCategoryId == id);
            }
            var cate = db.ProductCategories.Find(id);
            if(cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            return View(items);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = db.Products.Where(x => x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSales()
        {
            var items = db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductHots()
        {
            var items = db.Products.Where(x => x.IsHot && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public ActionResult Partial_ProductFeatures()
        {
            var items = db.Products.Where(x => x.IsFeature && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult PartialView_Combo(int ProductId)
        {
            var items = db.ComboDetails.Where(x => x.ProductId == ProductId && x.Combo.IsActive == true && x.Combo.Quantity > 0);

            HashSet<int> cb = new HashSet<int>();
            foreach (var i in items)
            {
                cb.Add(i.ComboId);
            }
            return PartialView(cb);
        }
    }
}