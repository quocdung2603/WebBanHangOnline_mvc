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
        public ActionResult Index(int?page, string SO)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ExportProduct> items = db.ExportProducts.ToList();
            ViewBag.TenPhieu = SO == "TenCombo" ? "TenCombod" : "TenCombo";
            ViewBag.NgayTao = SO == "NgayTao" ? "NgayTaod" : "NgayTao";
            ViewBag.NguoiTao = SO == "NguoiTao" ? "NguoiTaod" : "NguoiTao";
            switch (SO)
            {
                case "TenCombo": items = items.OrderBy(x => x.Title); break;
                case "TenCombod": items = items.OrderByDescending(x => x.Title); break;
                case "NgayTao": items = items.OrderBy(x => x.CreatedDate); break;
                case "NgayTaod": items = items.OrderByDescending(x => x.CreatedDate); break;
                case "NguoiTao": items = items.OrderBy(x => x.CreatedBy); break;
                case "NguoiTaod": items = items.OrderByDescending(x => x.CreatedBy); break;
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
            ViewBag.Titles = db.Products.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection f, List<ExportProductDetail> LProduct)
        {
            var title = f["TenDotNhapHang"];
            var note = f["GhiChu"];
            ExportProduct ip = new ExportProduct();
            ip.Title = title;
            ip.Note = note;
            ip.CreatedDate = DateTime.Now;
            ip.ModifierDate = DateTime.Now;
            if (Request.IsAuthenticated)
            {
                ip.CreatedBy = User.Identity.Name;
                ip.ModifierBy = User.Identity.Name;
            }
            db.ExportProducts.Add(ip);
            db.SaveChanges();

            foreach (var item in LProduct)
            {
                Product p = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (p != null)
                {
                    p.ModifierDate = DateTime.Now;
                    ProductSize tmp = db.ProductSizes.FirstOrDefault(x => x.ProductId == item.ProductId && x.SizeName == item.Size && x.ColorName == item.Color);
                    if (tmp != null && item.Quantity != null && item.Quantity > 0 && item.Quantity <= p.Quantity)
                    {
                        tmp.Quantity -= item.Quantity;
                        p.Quantity -= item.Quantity;
                    }
                }
                //them chi tiet phieu nhap
                ExportProductDetail ipd = new ExportProductDetail();
                ipd.ExportProductId = ip.Id;
                ipd.ProductId = item.ProductId;
                ipd.Title = item.Title;
                ipd.Color = item.Color;
                ipd.Size = item.Size;
                ipd.Quantity = item.Quantity;
                db.ExportProductDetails.Add(ipd);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var items = db.ExportProducts.FirstOrDefault(x => x.Id == id);
            return View(items);
        }

        public ActionResult Edit(int id)
        {
            var item = db.ExportProducts.FirstOrDefault(x => x.Id == id);
            List<Product> a = db.Products.ToList();
            ViewBag.p = a;
            ViewBag.Titles = db.Products.ToList();
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<ExportProductDetail> LProduct, List<ProductSize> ProductSizeNow, int ipId)
        {
            var title = f["TenDotNhapHang"];
            var note = f["GhiChu"];
            ExportProduct ip = db.ExportProducts.FirstOrDefault(x => x.Id == ipId);
            ip.Title = title;
            ip.Note = note;
            ip.ModifierDate = DateTime.Now;
            if (Request.IsAuthenticated)
            {
                ip.ModifierBy = User.Identity.Name;
            }
            db.SaveChanges();

            int i = 0;
            foreach (var item in LProduct)
            {
                ExportProductDetail ipd = db.ExportProductDetails.FirstOrDefault(x => x.Id == item.Id);
                int qt = item.Quantity;
                int ipr = item.ProductId;
                if (ipr == 0)
                {
                    var test = f["LProduct[" + i + "].ProductId"];
                    var test1 = test.Split(',');
                    ipr = int.Parse(test1[1]);
                }
                if (qt == 0)
                {
                    var test = f["LProduct[" + i + "].Quantity"];
                    var test1 = test.Split(',');
                    qt = int.Parse(test1[1]);
                }
                if (ipd!= null)
                {
                    ipd.ExportProductId = ip.Id;
                    ipd.ProductId = ipr;
                    ipd.Title = item.Title;
                    ipd.Color = item.Color;
                    ipd.Size = item.Size;
                    ipd.Quantity = qt;
                }
                else
                {
                    ipd = new ExportProductDetail();
                    ipd.ExportProductId = ip.Id;
                    ipd.ProductId = ipr;
                    ipd.Title = item.Title;
                    ipd.Color = item.Color;
                    ipd.Size = item.Size;
                    ipd.Quantity = qt;
                    db.ExportProductDetails.Add(ipd);
                    db.SaveChanges();
                } 
                    
                
                Product p = db.Products.FirstOrDefault(x => x.Id == ipr);
                if (p != null)
                {
                    p.ModifierDate = DateTime.Now;
                    if(i < ProductSizeNow.Count())
                    {
                        var Ids = ProductSizeNow[i].ProductId;
                        var Szn = ProductSizeNow[i].SizeName;
                        var Qtn = ProductSizeNow[i].Quantity;
                        var Cln = ProductSizeNow[i].ColorName;

                        ProductSize sz = db.ProductSizes.FirstOrDefault(x => x.ProductId == Ids && x.SizeName == Szn && x.ColorName == Cln);
                        ProductSize tmp = db.ProductSizes.FirstOrDefault(x => x.ProductId == ipr && x.SizeName == item.Size && x.ColorName == item.Color);
                        if (sz != tmp)
                        {
                            if (tmp != null)
                            {
                                sz.Quantity += Qtn;
                                tmp.Quantity -= qt;
                            }
                        }
                        else
                        {
                            tmp.Quantity += ProductSizeNow[i].Quantity;
                            tmp.Quantity -= qt;
                        }
                    }
                    else
                    {
                        ProductSize psz = db.ProductSizes.FirstOrDefault(x => x.ProductId == ipr && x.SizeName == item.Size && x.ColorName == item.Color);
                        if (psz != null)
                        {
                            psz.Quantity -= qt;
                            db.SaveChanges();
                        }
                    } 
                    i++;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}