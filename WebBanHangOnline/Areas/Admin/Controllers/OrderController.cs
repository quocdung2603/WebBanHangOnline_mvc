using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using PagedList;
using PagedList.Mvc;
using WebBanHangOnline.Models.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee,StoreKeeper,Shipper")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        /*index cho admin , chi dc thêm sửa xóa, k dc thao tác tình trạng đơn hàng*/
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

        /*index cho nhân viên */
        public ActionResult IndexForEmployee(int? page, string Searchtext, string Cod, string Banking, string Paid, string UnPaid)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 0).OrderByDescending(x => x.CreatedDate).ToList();
            //search
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext) || x.Phone.Contains(Searchtext));
            }
            //lọc
            if (!string.IsNullOrEmpty(Cod) && Cod == "true")
            {
                items = items.Where(x => x.TypePayment == 1);
            }
            else if (!string.IsNullOrEmpty(Banking) && Banking == "true")
            {
                items = items.Where(x => x.TypePayment == 2);
            }
            else if (!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
            {
                items = items.Where(x => x.Status == "1");
            }
            else if (!string.IsNullOrEmpty(Paid) && Paid == "true")
            {
                items = items.Where(x => x.Status == "2");
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        /*index cho nhan viên kho*/
        public ActionResult IndexForStoreKeeper(int? page, string Searchtext, string Cod, string Banking, string Paid, string UnPaid)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 1).OrderByDescending(x => x.CreatedDate).ToList();
            //search
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext) || x.Phone.Contains(Searchtext));
            }
            //lọc
            if (!string.IsNullOrEmpty(Cod) && Cod == "true")
            {
                items = items.Where(x => x.TypePayment == 1);
            }
            else if (!string.IsNullOrEmpty(Banking) && Banking == "true")
            {
                items = items.Where(x => x.TypePayment == 2);
            }
            else if (!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
            {
                items = items.Where(x => x.Status == "1");
            }
            else if (!string.IsNullOrEmpty(Paid) && Paid == "true")
            {
                items = items.Where(x => x.Status == "2");
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        /*index cho nhan viên kho*/
        public ActionResult IndexForShipper(int? page, string Searchtext, string Cod, string Banking, string Paid, string UnPaid)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 2).OrderByDescending(x => x.CreatedDate).ToList();
            //search
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Code.Contains(Searchtext) || x.CustomerName.Contains(Searchtext) || x.Phone.Contains(Searchtext));
            }
            //lọc
            if (!string.IsNullOrEmpty(Cod) && Cod == "true")
            {
                items = items.Where(x => x.TypePayment == 1);
            }
            else if (!string.IsNullOrEmpty(Banking) && Banking == "true")
            {
                items = items.Where(x => x.TypePayment == 2);
            }
            else if (!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
            {
                items = items.Where(x => x.Status == "1");
            }
            else if (!string.IsNullOrEmpty(Paid) && Paid == "true")
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

        public ActionResult Partial_TinhTrangDonHang(int id)
        {
            var items = db.Orders.Find(id);
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(items.IdEConform);
            if (user != null)
            {
                ViewBag.IdeConform = user.FullName;
            }
            else
            {
                ViewBag.IdeConform = "";
            }    
            user = userManager.FindById(items.IdEExport);
            if (user != null)
            {
                ViewBag.IdeEExport = user.FullName;
            }
            else
            {
                ViewBag.IdeEExport = "";
            }
            return PartialView();
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

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var item = db.Orders.Find(id);
                if (item != null)
                {
                    db.Orders.Attach(item);
                    if (item.OrderStatus == 0)
                    {
                        item.IdEConform = user.Id;
                        item.OrderStatus += 1;
                    }
                    else if (item.OrderStatus == 1)
                    {
                        item.IdEExport = user.Id;
                        item.OrderStatus += 1;
                    }
                    else if(item.OrderStatus == 2)
                    {
                        item.IdEDelivery = user.Id;
                        item.OrderStatus += 1;
                    }
                    db.Entry(item).Property(x => x.OrderStatus).IsModified = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
    }
}