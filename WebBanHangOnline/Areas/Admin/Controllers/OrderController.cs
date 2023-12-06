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
using System.ComponentModel;
using OfficeOpenXml;
using System.IO;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee,StoreKeeper,Shipper")]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        /*index cho admin , chi dc thêm sửa xóa, k dc thao tác tình trạng đơn hàng*/
        public ActionResult Index(int ?page, string Searchtext, string Cod, string Banking, string Paid, string UnPaid, string SO)
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
            // sort
            ViewBag.Code = SO == "Code" ? "Coded" : "Code";
            ViewBag.Total = SO == "Total" ? "Totald" : "Total";
            ViewBag.PT = SO == "PT" ? "PTd" : "PT";
            ViewBag.TT = SO == "TT" ? "TTd" : "TT";
            ViewBag.CreatedDate = SO == "CreatedDate" ? "CreatedDated" : "CreatedDate";
            ViewBag.TinhTrang = SO == "TinhTrang" ? "TinhTrangd" : "TinhTrang";
            switch(SO)
            {
                case "Code": items = items.OrderBy(x => x.Code);break;
                case "Coded": items = items.OrderByDescending(x => x.Code); break;
                case "Total": items = items.OrderBy(x => x.TotalAmount); break;
                case "Totald": items = items.OrderByDescending(x => x.TotalAmount); break;
                case "PT": items = items.OrderBy(x => x.TypePayment); break;
                case "PTd": items = items.OrderByDescending(x => x.TypePayment); break;
                case "TT": items = items.OrderBy(x => x.Status); break;
                case "TTd": items = items.OrderByDescending(x => x.Status); break;
                case "CreatedDate": items = items.OrderBy(x => x.CreatedDate); break;
                case "CreatedDated": items = items.OrderByDescending(x => x.CreatedDate); break;
                case "TinhTrang": items = items.OrderBy(x => x.OrderStatus); break;
                case "TinhTrangd": items = items.OrderByDescending(x => x.OrderStatus); break;
                default:break;
            }    
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        /*index cho nhân viên */
        public ActionResult IndexForEmployee(int? page, string Cod, string Banking, string Paid, string UnPaid, string ods0, string ods_1)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var pageSize = 10;
                if (page == null)
                {
                    page = 1;
                }
                IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 0 || x.OrderStatus == -1).OrderByDescending(x => x.CreatedDate).ToList();
                List<Order> o = new List<Order>();
                if (user.IsLeader == false)
                {
                    var tmp = db.DetailOrderStatuses.Where(x => x.IdUConfirm == user.Id).ToList();
                    foreach (var item in tmp)
                    {
                        var i = db.Orders.FirstOrDefault(x => x.Id == item.OrderId && (x.OrderStatus == 0 || x.OrderStatus == -1));
                        if (i != null)
                        {
                            o.Add(i);
                        }
                    }
                    items = o.OrderByDescending(x => x.CreatedDate).ToList();
                }
                //lọc
                if (!string.IsNullOrEmpty(Cod) && Cod == "true")
                {
                    items = o.Where(x => x.TypePayment == 1).OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(Banking) && Banking == "true")
                {
                    items = o.Where(x => x.TypePayment == 2).OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
                {
                    items = o.Where(x => x.Status == "1").OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(Paid) && Paid == "true")
                {
                    items = o.Where(x => x.Status == "2").OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if(!string.IsNullOrEmpty(ods0) && ods0 == "true")
                {
                    items = o.Where(x => x.OrderStatus == 0).OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(ods_1) && ods_1 == "true")
                {
                    items = o.Where(x => x.OrderStatus == -1).OrderByDescending(x => x.CreatedDate).ToList();
                }
                var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                items = items.ToPagedList(pageIndex, pageSize);
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.IsLeader = user.IsLeader;
                return View(items);
            }
            return View();
        }
        /*index cho nhan viên kho*/
        public ActionResult IndexForStoreKeeper(int? page, string ods1, string ods4)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var pageSize = 10;
                if (page == null)
                {
                    page = 1;
                }
                IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 1 || x.OrderStatus == 4).OrderByDescending(x => x.CreatedDate).ToList();
                List<Order> o = new List<Order>();
                if (user.IsLeader == false)
                {
                    var tmp = db.DetailOrderStatuses.Where(x => x.IdUExport == user.Id).ToList();
                    foreach(var item in tmp)
                    {
                        var i = db.Orders.FirstOrDefault(x => x.Id == item.OrderId && (x.OrderStatus == 1 || x.OrderStatus == 4));
                        if(i!=null)
                        {
                            o.Add(i);
                        }
                    }
                    items = o.OrderByDescending(x => x.CreatedDate).ToList();
                }
                //lọc
                if(!string.IsNullOrEmpty(ods1) && ods1 == "true")
                {
                    items = o.Where(x => x.OrderStatus == 1).OrderByDescending(x => x.CreatedDate).ToList();
                }
                if (!string.IsNullOrEmpty(ods4) && ods4 == "true")
                {
                    items = o.Where(x => x.OrderStatus == 4).OrderByDescending(x => x.CreatedDate).ToList();
                }
                var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                items = items.ToPagedList(pageIndex, pageSize);
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.IsLeader = user.IsLeader;
                return View(items);
            }
            return View();
        }

        /*index cho shipper*/
        public ActionResult IndexForShipper(int? page, string Paid, string UnPaid, string ods2, string ods3)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                var pageSize = 10;
                if (page == null)
                {
                    page = 1;
                }
                IEnumerable<Order> items = db.Orders.Where(x => x.OrderStatus == 2 || x.OrderStatus == 3).OrderByDescending(x => x.CreatedDate).ToList();
                List<Order> o = new List<Order>();
                if (user.IsLeader == false)
                {
                    var tmp = db.DetailOrderStatuses.Where(x => x.IdUDelivery == user.Id).ToList();
                    
                    foreach (var item in tmp)
                    {
                        var i = db.Orders.FirstOrDefault(x => x.Id == item.OrderId && x.OrderStatus == 2);
                        if (i != null)
                        {
                            o.Add(i);
                        }
                    }
                    items = o.OrderByDescending(x => x.CreatedDate).ToList();
                }
                //lọc
                if (!string.IsNullOrEmpty(UnPaid) && UnPaid == "true")
                {
                    items = o.Where(x => x.Status == "1").OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(Paid) && Paid == "true")
                {
                    items = o.Where(x => x.Status == "2").OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if(!string.IsNullOrEmpty(ods2) && ods2=="true")
                {
                    items = o.Where(x => x.OrderStatus == 2).OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if(!string.IsNullOrEmpty(ods3) && ods3=="true")
                {
                    items = o.Where(x => x.OrderStatus == 3).OrderByDescending(x => x.CreatedDate).ToList();
                }
                var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                items = items.ToPagedList(pageIndex, pageSize);
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.IsLeader = user.IsLeader;
                return View(items);
            }
            return View();
        }
        
        public ActionResult View(int id)
        {
            var item = db.Orders.Find(id);
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Orders.Find(id);
            ViewBag.DanhSachSanPham = db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(Order model, List<OrderDetail> LProduct)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var user = userManager.FindByName(User.Identity.Name);
                    var i = 0;
                    var tmpOrderId = 0;
                    foreach(var item in LProduct)
                    {
                        var lpOrderId = LProduct[i].OrderId;
                        tmpOrderId = LProduct[i].OrderId;
                        var lpProductId = LProduct[i].ProductId;
                        var lpProductSize = LProduct[i].ProductSize;
                        var lpProductColor = LProduct[i].ProductColor;
                        var orderdetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == lpOrderId && x.ProductId == lpProductId && x.ProductSize == lpProductSize && x.ProductColor == lpProductColor);
                        if(string.IsNullOrEmpty(lpProductSize))
                        {
                            orderdetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == lpOrderId && x.ProductId == lpProductId && x.ProductColor == lpProductColor);
                        }   
                        if(string.IsNullOrEmpty(lpProductColor))
                        {
                            orderdetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == lpOrderId && x.ProductId == lpProductId && x.ProductSize == lpProductSize);
                        }    
                        if(orderdetail != null)
                        {
                            orderdetail.Price = LProduct[i].Price;
                            orderdetail.Quantity = LProduct[i].Quantity;
                            orderdetail.ProductSize = LProduct[i].ProductSize;
                            orderdetail.ProductColor = LProduct[i].ProductColor;
                            db.SaveChanges();
                        }
                        else
                        {
                            orderdetail = db.OrderDetails.FirstOrDefault(x => x.OrderId == lpOrderId && x.ProductId == lpProductId && x.ProductColor == "" && x.ProductSize == "");
                            if(orderdetail != null)
                            {
                                orderdetail.Price = LProduct[i].Price;
                                orderdetail.Quantity = LProduct[i].Quantity;
                                orderdetail.ProductSize = LProduct[i].ProductSize;
                                orderdetail.ProductColor = LProduct[i].ProductColor;
                                db.SaveChanges();
                                var psz = db.ProductSizes.FirstOrDefault(x => x.ProductId == lpProductId && x.SizeName == lpProductSize && x.ColorName == lpProductColor);
                                if(psz!=null)
                                {
                                    psz.Quantity -= orderdetail.Quantity;
                                    db.SaveChanges();
                                }
                            }    
                        } 
                        i++;
                    }
                    var tmp = db.OrderDetails.Where(x => x.OrderId == tmpOrderId).ToList();
                    decimal totalAmount = Decimal.Zero;
                    foreach (var z in tmp)
                    {
                        totalAmount += (z.Price * z.Quantity);
                    }
                    model.TotalAmount = totalAmount;
                    model.ModifierBy = user.FullName;
                    model.ModifierDate = DateTime.Now;
                    db.Orders.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexForEmployee");
                }
            }
            return View(model); 
        }

        [HttpPost]
        public ActionResult DeleteInEditOrder(int id)
        {
            var item = db.OrderDetails.Find(id);
            if(item!=null)
            {
                var p = db.ProductSizes.FirstOrDefault(x=>x.ProductId == item.ProductId && x.SizeName == item.ProductSize && x.ColorName == item.ProductColor);
                if(string.IsNullOrEmpty(item.ProductSize))
                {
                    p = db.ProductSizes.FirstOrDefault(x => x.ProductId == item.ProductId && x.ColorName == item.ProductColor);
                }
                if(string.IsNullOrEmpty(item.ProductColor))
                {
                    p = db.ProductSizes.FirstOrDefault(x => x.ProductId == item.ProductId && x.SizeName == item.ProductSize);
                }
                if(p!=null)
                {
                    p.Quantity += item.Quantity;
                    db.SaveChanges();
                }
                db.OrderDetails.Remove(item);
                db.SaveChanges();
                var tmp = db.OrderDetails.Where(x => x.OrderId == item.OrderId).ToList();
                decimal totalAmount = Decimal.Zero;
                foreach(var z in tmp)
                {
                    totalAmount += (z.Price * z.Quantity);
                }
                var o = db.Orders.FirstOrDefault(x => x.Id == item.OrderId);
                o.TotalAmount = totalAmount;
                db.Orders.Attach(o);
                db.Entry(o).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
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
                if(!string.IsNullOrEmpty(items.IdUConfirm))
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

        //trưởng bộ phận có thể xuất file excel
        [HttpPost]
        public ActionResult ExportFileExcel_StoreKeeper(string data)
        {
            List<Product> p = new List<Product>();
            if (!string.IsNullOrEmpty(data))
            {
                var items = data.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var o in items)
                    {
                        var oid = Convert.ToInt32(o);
                        var od = db.OrderDetails.Where(x => x.OrderId == oid ).ToList();
                        if (od != null)
                        {
                            foreach (var item in od)
                            {
                                var p1 = db.Products.Find(item.ProductId);
                                if (p1 != null)
                                {
                                    var pcheck = p.FirstOrDefault(x => x.Id == p1.Id);
                                    if (pcheck != null)
                                    {
                                        pcheck.Quantity += item.Quantity;
                                    }
                                    else
                                    {
                                        p1.Quantity = item.Quantity;
                                        p.Add(p1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("SanPham");
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells[1, 1].Value = "PHIẾU TIẾP NHẬN ĐƠN HÀNG";
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[1, 1, 1, 3].Merge = true;

                worksheet.Cells[2, 1].Value = "Mã Sản Phẩm";
                worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[2, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[2, 1].AutoFitColumns();
                worksheet.Cells[2, 2].Value = "Tên Sản Phẩm";
                worksheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[2, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[2, 2].AutoFitColumns();
                worksheet.Cells[2, 3].Value = "Số Lượng Lấy";
                worksheet.Cells[2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[2, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[2, 3].AutoFitColumns();
                int row = 3;
                foreach (var item in p)
                {
                    worksheet.Cells[row,1].Value = item.Id;
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, 1].AutoFitColumns();
                    worksheet.Cells[row,2].Value = item.Title;
                    worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, 2].AutoFitColumns();
                    worksheet.Cells[row,3].Value = item.Quantity;
                    worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[row, 3].AutoFitColumns();
                    row++;
                }
                
                package.Save();
                stream.Position = 0;
                var fileName = $"SanPhamCanLay_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpPost]
        public ActionResult ExportFileExcel_Shipper_Employee(string data)
        {
            if(User.Identity.IsAuthenticated)
            {
                // lấy user đang login
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                //lấy dữ liệu vào list
                List<Product> p = new List<Product>();

                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("SanPham");
                    worksheet.Cells[1, 1].Value = "PHIẾU TIẾP NHẬN ĐƠN HÀNG";
                    if (User.IsInRole("Employee"))
                    {
                        worksheet.Cells[1, 1].Value = "PHIẾU XÁC NHẬN ĐƠN HÀNG";
                    }    
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 8].Merge = true;

                    worksheet.Cells[2, 1].Value = "Tên người nhận";
                    worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[2, 1].AutoFitColumns();

                    worksheet.Cells[2, 2].Value = user.FullName;
                    worksheet.Cells[2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[2, 2].AutoFitColumns();

                    worksheet.Cells[3, 1].Value = "Ngày Nhận";
                    worksheet.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[3, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[3, 1].AutoFitColumns();

                    worksheet.Cells[3, 2].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    worksheet.Cells[3, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[3, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[3, 2].AutoFitColumns();

                    int row = 5;

                    if (!string.IsNullOrEmpty(data))
                    {
                        var items = data.Split(',');
                        if (items != null && items.Any())
                        {
                            foreach (var o in items)
                            {
                                var oid = Convert.ToInt32(o);
                                var tmp = db.Orders.Find(oid);
                                if (tmp != null)
                                {
                                    worksheet.Cells[row, 1].Value = "Thông Tin Đơn Hàng";
                                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 1, row, 3].Merge = true;
                                    worksheet.Cells[row, 5].Value = "Thông Tin Khách Hàng";
                                    worksheet.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 5, row, 7].Merge = true;
                                    row++;
                                    worksheet.Cells[row, 1].Value = "Mã đơn hàng";
                                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 1].AutoFitColumns();

                                    worksheet.Cells[row, 2].Value = "Tổng Hóa Đơn";
                                    worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 2].AutoFitColumns();

                                    worksheet.Cells[row, 3].Value = "Ngày Đặt";
                                    worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 3].AutoFitColumns();

                                    worksheet.Cells[row, 5].Value = "Tên Khách Hàng";
                                    worksheet.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 5].AutoFitColumns();

                                    worksheet.Cells[row, 6].Value = "Số Điện Thoại";
                                    worksheet.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 6].AutoFitColumns();

                                    worksheet.Cells[row, 7].Value = "Địa Chỉ Giao Hàng";
                                    worksheet.Cells[row, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 7].AutoFitColumns();
                                    row++;

                                    worksheet.Cells[row, 1].Value = tmp.Id;
                                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 1].AutoFitColumns();

                                    worksheet.Cells[row, 2].Value = tmp.TotalAmount;
                                    worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 2].AutoFitColumns();

                                    worksheet.Cells[row, 3].Value = tmp.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss");
                                    worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 3].AutoFitColumns();

                                    worksheet.Cells[row, 5].Value = tmp.CustomerName;
                                    worksheet.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 5].AutoFitColumns();

                                    worksheet.Cells[row, 6].Value = tmp.Phone;
                                    worksheet.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 6].AutoFitColumns();

                                    worksheet.Cells[row, 7].Value = tmp.Address;
                                    worksheet.Cells[row, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 7].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 7].AutoFitColumns();

                                    row += 2;
                                    worksheet.Cells[row, 1].Value = "Thông Tin Sản Phẩm Trong Đơn Hàng";
                                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 1, row, 7].Merge = true;
                                    row++;
                                    worksheet.Cells[row, 1].Value = "Tên Sản Phẩm";
                                    worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 1].AutoFitColumns();

                                    worksheet.Cells[row, 2].Value = "Giá Sản Phẩm";
                                    worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 2].AutoFitColumns();

                                    worksheet.Cells[row, 3].Value = "Số Lượng Sản Phẩm";
                                    worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[row, 3].AutoFitColumns();
                                    row++;
                                    var od = db.OrderDetails.Where(x => x.OrderId == oid).ToList();
                                    if (od != null)
                                    {
                                        foreach (var item in od)
                                        {
                                            var p1 = db.Products.Find(item.ProductId);
                                            if (p1 != null)
                                            {
                                                worksheet.Cells[row, 1].Value = p1.Title;
                                                worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                worksheet.Cells[row, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                                worksheet.Cells[row, 1].AutoFitColumns();

                                                worksheet.Cells[row, 2].Value = item.Price;
                                                worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                worksheet.Cells[row, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                                worksheet.Cells[row, 2].AutoFitColumns();

                                                worksheet.Cells[row, 3].Value = item.Quantity;
                                                worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                worksheet.Cells[row, 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                                worksheet.Cells[row, 3].AutoFitColumns();
                                                row++;
                                            }
                                        }
                                        row++;
                                    }
                                }
                            }
                        }
                    }
                    package.Save();
                    stream.Position = 0;
                    var fileName = $"PhieuTiepNhanDonHang_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    if(User.IsInRole("Employee"))
                    {
                        fileName = $"PhieuXacNhanDonHang_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    }    
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            return View();
        }

        //trưởng bộ phân có thể phân chia việc cho nhân viên cùng bộ phận
        public ActionResult ShareWork(string data)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var item = userManager.GetRoles(user.Id);
                var item1 = item.ElementAt(0);
                var u = db.Users.ToList();
                List<ApplicationUser> listuser = new List<ApplicationUser>();
                foreach (var us in u)
                {
                    var check = userManager.GetRoles(us.Id);
                    var check1 = check.ElementAt(0);
                    Console.WriteLine(check);
                    Console.WriteLine(item1);
                    if (check1 == item1 && us.IsLeader == false)
                    {
                        listuser.Add(us);
                    }
                }
                ViewBag.Users = new SelectList(listuser, "Id", "FullName");
                ViewBag.Data = data;
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ShareWork(string data, string idUser)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userRole = userManager.GetRoles(idUser);
                var ur = userRole.ElementAt(0);

                if (!string.IsNullOrEmpty(data))
                {
                    var items = data.Split(',');
                    foreach (var item in items)
                    {
                        int orderid = Convert.ToInt32(item);
                        var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == orderid);
                        if (dos != null)
                        {
                            if(ur=="StoreKeeper")
                            {
                                dos.IdUExport = idUser;
                                dos.ExportDate = DateTime.Now;
                            }
                            else if(ur=="Shipper")
                            {
                                dos.IdUDelivery = idUser;
                                dos.DeliveryDate = DateTime.Now;
                            }
                            else if(ur=="Employee")
                            {
                                dos.IdUConfirm = idUser;
                                dos.CofirmDate = DateTime.Now;
                            }    
                        }
                        db.SaveChanges();
                    }
                    if(ur== "StoreKeeper")
                    {
                        return RedirectToAction("IndexForStoreKeeper");
                    }
                    else if(ur == "Shipper")
                    {
                        return RedirectToAction("IndexForShipper");
                    }
                    else if(ur == "Employee")
                    {
                        return RedirectToAction("IndexForEmployee");
                    }    
                }
            }    
            return View();
        }
        
        [HttpPost]
        public ActionResult Tmp(string data)
        {
            return RedirectToAction("ShareWork", new { @data = data});
        }

        [HttpPost]
        public ActionResult AcceptedAll(string data)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                var ur = userManager.GetRoles(user.Id).ElementAt(0);
                var url = 0;
                if (!string.IsNullOrEmpty(data))
                {
                    var items = data.Split(',');
                    if (items != null && items.Any())
                    {
                        foreach (var z in items)
                        {
                            var item = db.Orders.Find(Convert.ToInt32(z));
                            if (item != null)
                            {
                                if (item.OrderStatus == 0) //xác nhận đơn hàng
                                {
                                    url = 0;
                                    var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                                    dos.IdUConfirm = user.Id;
                                    dos.CofirmDate = DateTime.Now;
                                    db.SaveChanges();
                                    item.OrderStatus += 1;
                                }
                                else if (item.OrderStatus == 1) //lấy hàng ra từ kho, cập nhật lại số lượng sản phẩm trong đơn hàng
                                {
                                    url = 1;
                                    var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                                    dos.IdUExport = user.Id;
                                    dos.ExportDate = DateTime.Now;
                                    //tru lai so luong hang hoa
                                    List<OrderDetail> od = db.OrderDetails.Where(x => x.OrderId == item.Id).ToList();
                                    foreach (var i in od)
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
                                    url = 2;
                                    var dos = db.DetailOrderStatuses.FirstOrDefault(x => x.OrderId == item.Id);
                                    dos.IdUDelivery = user.Id;
                                    dos.DeliveryDate = DateTime.Now;
                                    db.SaveChanges();
                                    item.OrderStatus += 1;
                                }
                                else
                                {
                                    if (ur == "Admin") return RedirectToAction("Index");
                                    else if (ur == "Employee") return RedirectToAction("IndexForEmployee");
                                    else if (ur == "StoreKeeper") return RedirectToAction("IndexForStoreKeeper");
                                    else return RedirectToAction("IndexForShipper");
                                }
                                db.Orders.Attach(item);
                                db.Entry(item).Property(x => x.OrderStatus).IsModified = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                if (url == 0)
                {
                    return RedirectToAction("IndexForEmployee");
                }
                else if (url == 1)
                {
                    return RedirectToAction("IndexForStoreKeeper");
                }
                else if(url == 2)
                {
                    return RedirectToAction("IndexForShipper");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult DestroyedAll(string data)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);

                if (!string.IsNullOrEmpty(data))
                {
                    var lst = data.Split(',');
                    ExportProduct ep = new ExportProduct();
                    ep.Title = "Hủy Sản Phẩm_" + DateTime.Now.ToString("dd/MM/yyyy");
                    ep.Note = "Hủy sản phẩm lỗi " + data;
                    ep.CreatedDate = DateTime.Now;
                    ep.ModifierDate = DateTime.Now;
                    if (Request.IsAuthenticated)
                    {
                        ep.CreatedBy = user.FullName;
                        ep.ModifierBy = user.FullName;
                    }
                    db.ExportProducts.Add(ep);
                    db.SaveChanges();
                    foreach (var z in lst)
                    {
                        var _id = Convert.ToInt32(z);
                        var od = db.OrderDetails.Where(x => x.OrderId == _id).ToList();
                        foreach (var item in od)
                        {
                            ExportProductDetail epd = new ExportProductDetail();
                            epd.ExportProductId = ep.Id;
                            epd.ProductId = item.ProductId;
                            epd.Quantity = item.Quantity;
                            epd.Color = item.ProductColor;
                            epd.Size = item.ProductSize;
                            epd.Title = item.Product.Title;
                            db.ExportProductDetails.Add(epd);
                            db.SaveChanges();
                        }
                        var o = db.Orders.FirstOrDefault(x=>x.Id == _id);
                        db.Orders.Remove(o);
                        db.SaveChanges();
                    }
                    return RedirectToAction("IndexForStoreKeeper");
                }
            }
            return View();
        }
    }
}