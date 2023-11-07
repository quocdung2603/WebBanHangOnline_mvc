using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
using PagedList;
using PagedList.Mvc;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ExportProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ExportProduct
        public ActionResult Index(int?page, string Searchtext)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ExportProduct> items = db.ExportProducts.OrderByDescending(x => x.CreatedDate);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Title.Contains(Searchtext));
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
            ViewBag.Titles = db.Products.ToList();
            return View();
        }
    }
}