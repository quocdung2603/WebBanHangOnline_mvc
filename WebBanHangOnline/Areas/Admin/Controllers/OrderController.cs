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
            if (User.Identity.IsAuthenticated)
            {
                var o = db.Orders.Find(id);
                var items = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == o.Id);
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var tmp = "";
                //confirm
                if(items.IdUConfirm != null)
                {
                    tmp = items.IdUConfirm;
                }
                var user = userManager.FindById(tmp);
                if(user!=null)
                {
                    ViewBag.IdUConfirm = user.FullName; ViewBag.ConfirmDate = items.CofirmDate.ToString("dd/MM/yyyy hh:mm:ss");
                }
                else
                {
                    ViewBag.IdUConfirm = ""; ViewBag.ConfirmDate = "";
                }
                //export
                tmp = "";
                if(items.IdUExport!=null)
                {
                    tmp = items.IdUExport;
                }
                user = userManager.FindById(tmp);
                if (user != null)
                {
                    ViewBag.IdUExport = user.FullName; ViewBag.ExportDate = items.ExportDate.ToString("dd/MM/yyyy hh:mm:ss");
                }
                else
                {
                    ViewBag.IdUExport = ""; ViewBag.ExportDate = "";
                }
                //delivery
                tmp = "";
                if(items.IdUDelivery!=null)
                {
                    tmp = items.IdUDelivery;
                }
                user = userManager.FindById(tmp);
                if (user != null)
                {
                    ViewBag.IdUDelivery = user.FullName; ViewBag.DeliveryDate = items.DeliveryDate.ToString("dd/MM/yyyy hh:mm:ss");
                }
                else
                {
                    ViewBag.IdUDelivery = ""; ViewBag.DeliveryDate = "";
                }
                //cancel
                tmp = "";
                if (items.IdUCancel != null)
                {
                    tmp = items.IdUCancel;
                }
                user = userManager.FindById(tmp);
                if (user != null)
                {
                    ViewBag.IdUCancel = user.FullName; ViewBag.CancelDate = items.CancelDate.ToString("dd/MM/yyyy hh:mm:ss");
                }
                else
                {
                    ViewBag.IdUCancel = ""; ViewBag.CancelDate = "";
                }
                //return
                tmp = "";
                if (items.IdUReturn != null)
                {
                    tmp = items.IdUReturn;
                }
                user = userManager.FindById(tmp);
                if (user != null)
                {
                    ViewBag.IdUReturn = user.FullName; ViewBag.ReturnDate = items.ReturnDate.ToString("dd/MM/yyyy hh:mm:ss");
                }
                else
                {
                    ViewBag.IdUReturn = ""; ViewBag.ReturnDate = "";
                }
                return PartialView();
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
                    if (item.OrderStatus == 0) //xác nhận đơn hàng
                    {
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                        dos.IdUConfirm = user.Id;
                        dos.CofirmDate = DateTime.Now;
                        db.SaveChanges();
                        item.OrderStatus += 1;
                    }
                    else if (item.OrderStatus == 1) //lấy hàng ra từ kho, cập nhật lại số lượng sản phẩm trong đơn hàng
                    {
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                        dos.IdUExport = user.Id;
                        dos.ExportDate = DateTime.Now;
                        //tru lai so luong hang hoa
                        List<OrderDetail> od = db.OrderDetails.Where(x => x.OrderId == item.Id).ToList();
                        foreach(var i in od)
                        {
                            Product p = db.Products.FirstOrDefault(x => x.Id == i.ProductId);
                            p.Quantity -= i.Quantity;
                            db.SaveChanges();
                        }    
                        db.SaveChanges();
                        item.OrderStatus += 1;
                    }
                    else if (item.OrderStatus == 2)
                    {
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                        dos.IdUDelivery = user.Id;
                        dos.DeliveryDate = DateTime.Now;
                        db.SaveChanges();
                        item.OrderStatus += 1;
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