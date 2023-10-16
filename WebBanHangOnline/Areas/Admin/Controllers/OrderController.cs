using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using PagedList;
using PagedList.Mvc;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int ?page, string Searchtext, string Cod, string Banking, string Paid, string UnPaid)
        {
            var pageSize = 10;
            if(page == null)
            {
                page = 1;
            }    
            IEnumerable<Order> items= db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            //search
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext) || x.Phone.Contains(Searchtext));
            }
            //lọc
            if(!string.IsNullOrEmpty(Cod) && Cod=="true")
            {
                items = items.Where(x => x.TypePayment == 1);
            }    
            else if(!string.IsNullOrEmpty(Banking) && Banking == "true")
            {
                items = items.Where(x => x.TypePayment == 2);
            }
            else if(!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
            {
                items = items.Where(x => x.Status == "1");
            }    
            else if(!string.IsNullOrEmpty(Paid) && Paid == "true")
            {
                items = items.Where(x => x.Status == "2");
            }    
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_SanPham(int id)
        {
            var items = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult UpdateTT(int id)
        {
            var item = db.Orders.Find(id);
            if(item!=null)
            {
                db.Orders.Attach(item);
                item.Status = "2";
                db.Entry(item).Property(x => x.Status).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Thành Công", success = true});
            }
            return Json(new { message = "Thất Bại", success = false});
        }
    }
}