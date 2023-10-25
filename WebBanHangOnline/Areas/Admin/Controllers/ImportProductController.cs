using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ImportProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ImportProduct
        public ActionResult Index()
        {
            var items = db.ImportProducts;
            return View(items);
        }

        public ActionResult Add()
        {
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
            db.ImportProducts.Add(ip);
            db.SaveChanges();

            foreach(var item in LProduct)
            {
                Product p = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (p != null)
                {
                    p.OriginalPrice = item.OriginalPrice;
                    p.Price = item.Price;
                    p.Quantity += item.Quantity;
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
                    ProductSize tmp = new ProductSize();
                    tmp.ProductId = item.ProductId;
                    tmp.ColorName = item.Color;
                    tmp.SizeName = item.Size;
                    tmp.Quantity = item.Quantity;
                    db.ProductSizes.Add(tmp);
                }
             
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
            return View();
        }
    }
}