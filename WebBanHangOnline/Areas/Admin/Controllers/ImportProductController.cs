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
    [Authorize(Roles = "Admin,StoreKeeper")]
    public class ImportProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ImportProduct
        public ActionResult Index(int? page, string Searchtext)
        {
            var pageSize = 10;
            if(page == null)
            {
                page = 1;
            }
            IEnumerable<ImportProduct> items = db.ImportProducts.OrderByDescending(x=> x.CreatedDate);
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

        [HttpPost]
        public ActionResult Add(FormCollection f, List<ImportProductDetail> LProduct)
        {
            var title = f["TenDotNhapHang"];
            var note = f["GhiChu"];
            ImportProduct ip = new ImportProduct();
            ip.Title = title;
            ip.Note = note;
            ip.CreatedDate = DateTime.Now;
            ip.ModifierDate = DateTime.Now;
            if(Request.IsAuthenticated)
            {
                ip.CreatedBy = User.Identity.Name;
                ip.ModifierBy = User.Identity.Name;
            }    
            db.ImportProducts.Add(ip);
            db.SaveChanges();

            foreach(var item in LProduct)
            {
                Product p = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (p != null)
                {
                    if(item.OriginalPrice!=null)
                    {
                        p.OriginalPrice = item.OriginalPrice;
                    }    
                    if(item.Price != null)
                    {
                        p.Price = item.Price;
                    }
                    if(item.Quantity != null)
                    {
                        p.Quantity += item.Quantity;
                    }
                    p.ModifierDate = DateTime.Now;
                    ProductSize tmp = db.ProductSizes.FirstOrDefault(x => x.ProductId == item.ProductId && x.SizeName == item.Size && x.ColorName == item.Color);
                    if (tmp == null)
                    {
                        tmp = new ProductSize();
                        tmp.ProductId = item.ProductId;
                        tmp.ColorName = item.Color;
                        tmp.SizeName = item.Size;
                        tmp.Quantity = item.Quantity;
                        db.ProductSizes.Add(tmp);
                    }
                    else
                    {
                        tmp.Quantity += item.Quantity;
                    }    
                }
                else
                {
                    p = new Product();
                    p.Title = item.Title;
                    p.OriginalPrice = item.OriginalPrice;
                    p.Price = item.Price;
                    p.Quantity = item.Quantity;
                    p.CreatedDate = DateTime.Now;
                    p.ModifierDate = DateTime.Now;
                    p.CategoryId = 3;
                    p.ProductCategoryId = 4;
                    Random rd = new Random();
                    p.ProductCode = "SP" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    p.Alias = WebBanHangOnline.Models.Commons.Filter.FilterChar(p.Title);
                    p.SeoTitle = p.Title;
                    db.Products.Add(p);

                    //them size + mau
                    ProductSize tmp = new ProductSize();
                    tmp.ProductId = item.ProductId;
                    tmp.ColorName = item.Color;
                    tmp.SizeName = item.Size;
                    tmp.Quantity = item.Quantity;
                    db.ProductSizes.Add(tmp);

                    //them hinh mac dinh cua san pham
                    ProductImage pi = new ProductImage();
                    pi.ProductId = p.Id;
                    pi.Image = "/Uploads/images/IMG/R.png";
                    pi.IsDefault = true;
                    db.ProductImages.Add(pi);
                }
                //them chi tiet phieu nhap
                ImportProductDetail ipd = new ImportProductDetail();
                ipd.ImportProductId = ip.Id;
                ipd.ProductId = item.ProductId;
                ipd.Title = item.Title;
                ipd.OriginalPrice = item.OriginalPrice;
                ipd.Price = item.Price;
                ipd.Color = item.Color;
                ipd.Size = item.Size;
                ipd.Quantity = item.Quantity;
                db.ImportProductDetails.Add(ipd);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        public ActionResult Detail(int id)
        {
            var items = db.ImportProducts.FirstOrDefault(x=>x.Id == id);
            return View(items);
        }
        
        public ActionResult Edit(int id)
        {
            var items = db.ImportProducts.FirstOrDefault(x => x.Id == id);
            return View(items);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<ImportProductDetail> LProduct, List<ProductSize> ProductSizeNow, int ipId)
        {
            // ip.Id = 0 la sai
            ImportProduct ip = db.ImportProducts.FirstOrDefault(x => x.Id == ipId);
            var title = f["TenDotNhapHang"];
            var note = f["GhiChu"];
          
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
                ImportProductDetail ipd = db.ImportProductDetails.FirstOrDefault(x => x.Id == item.Id);
                ipd.ImportProductId = ip.Id;
                ipd.ProductId = item.ProductId;
                ipd.Title = item.Title;
                ipd.OriginalPrice = item.OriginalPrice;
                ipd.Price = item.Price;
                ipd.Color = item.Color;
                ipd.Size = item.Size;
                ipd.Quantity = item.Quantity;

                Product p = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (p != null)
                {
                    p.OriginalPrice = item.OriginalPrice;
                    p.Price = item.Price;
                    if (ProductSizeNow[i].Quantity != item.Quantity)
                    {
                        p.Quantity -= ProductSizeNow[i].Quantity;
                        p.Quantity += item.Quantity;
                    }
                    p.ModifierDate = DateTime.Now;
                    var Ids = ProductSizeNow[i].ProductId;
                    var Szn = ProductSizeNow[i].SizeName;
                    var Qtn = ProductSizeNow[i].Quantity;
                    var Cln = ProductSizeNow[i].ColorName;
                    ProductSize sz = db.ProductSizes.FirstOrDefault(x => x.ProductId == Ids && x.SizeName==Szn && x.ColorName==Cln );
                    ProductSize tmp = db.ProductSizes.FirstOrDefault(x => x.ProductId == item.ProductId && x.SizeName == item.Size && x.ColorName == item.Color);
                    if (sz != tmp)
                    {
                        if (tmp == null)
                        {
                            tmp = new ProductSize();
                            tmp.ProductId = item.ProductId;
                            tmp.ColorName = item.Color;
                            tmp.SizeName = item.Size;
                            tmp.Quantity = item.Quantity;
                            sz.Quantity -= Qtn;
                            db.ProductSizes.Add(tmp);
                        }
                        else
                        {
                            sz.Quantity -= Qtn;
                            tmp.Quantity += item.Quantity;
                        }
                    }
                    else
                    {
                        tmp.Quantity -= ProductSizeNow[i].Quantity;
                        tmp.Quantity += item.Quantity;
                    }
                    i++;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}