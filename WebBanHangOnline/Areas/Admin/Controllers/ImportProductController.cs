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
        public ActionResult Index(int? page, string SO)
        {
            var pageSize = 10;
            if(page == null)
            {
                page = 1;
            }
            IEnumerable<ImportProduct> items = db.ImportProducts.OrderByDescending(x=> x.CreatedDate);
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
                        db.SaveChanges();
                    }
                    else
                    {
                        tmp.Quantity += item.Quantity;
                        db.SaveChanges();
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
            List<Product> a = db.Products.ToList();
            ViewBag.p = a;
            ViewBag.Titles = db.Products.ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection f, List<ImportProductDetail> LProduct, List<ProductSize> ProductSizeNow, int ipId)
        {
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
            //
            int i = 0;
            foreach (var item in LProduct) //6
            {
                ImportProductDetail ipd = db.ImportProductDetails.FirstOrDefault(x => x.Id == item.Id);
                int qt = item.Quantity;
                int ipr = item.ProductId;
                if(ipr==0)
                {
                    var test = f["LProduct[" + i + "].ProductId"];
                    var test1 = test.Split(',');
                    ipr = int.Parse(test1[1]);
                }    
                if(qt==0)
                {
                    var test = f["LProduct[" + i + "].Quantity"];
                    var test1 = test.Split(',');
                    qt = int.Parse(test1[1]);
                }    
                if(ipd != null)
                {
                    ipd.ImportProductId = ip.Id;
                    ipd.ProductId = ipr;
                    ipd.Title = item.Title;
                    ipd.OriginalPrice = item.OriginalPrice;
                    ipd.Price = item.Price;
                    ipd.Color = item.Color;
                    ipd.Size = item.Size;
                    ipd.Quantity = qt;
                }
                else
                {
                    ipd = new ImportProductDetail();
                    ipd.ImportProductId = ip.Id;
                    ipd.ProductId = ipr;
                    ipd.Title = item.Title;
                    ipd.OriginalPrice = item.OriginalPrice;
                    ipd.Price = item.Price;
                    ipd.Color = item.Color;
                    ipd.Size = item.Size;

                    //Chỉnh số lượng
                    ipd.Quantity = qt;
                    db.ImportProductDetails.Add(ipd);
                    db.SaveChanges();
                }
                //
                Product p = db.Products.FirstOrDefault(x => x.Id == ipr);
                if (p != null)
                {
                    p.OriginalPrice = item.OriginalPrice;
                    p.Price = item.Price;
                    if(i<ProductSizeNow.Count())
                    {
                        if (ProductSizeNow[i].Quantity != qt)
                        {
                            p.Quantity -= ProductSizeNow[i].Quantity;
                            p.Quantity += qt;
                        }
                        p.ModifierDate = DateTime.Now;
                        var Ids = ProductSizeNow[i].ProductId;
                        var Szn = ProductSizeNow[i].SizeName;
                        var Qtn = ProductSizeNow[i].Quantity;
                        var Cln = ProductSizeNow[i].ColorName;
                        ProductSize sz = db.ProductSizes.FirstOrDefault(x => x.ProductId == Ids && x.SizeName == Szn && x.ColorName == Cln);
                        ProductSize tmp = db.ProductSizes.FirstOrDefault(x => x.ProductId == ipr && x.SizeName == item.Size && x.ColorName == item.Color);
                        if (sz != tmp)
                        {
                            if (tmp == null)
                            {
                                tmp = new ProductSize();
                                tmp.ProductId = ipr;
                                tmp.ColorName = item.Color;
                                tmp.SizeName = item.Size;
                                tmp.Quantity = qt;
                                sz.Quantity -= Qtn;
                                db.ProductSizes.Add(tmp);
                            }
                            else
                            {
                                sz.Quantity -= Qtn;
                                tmp.Quantity += qt;
                            }
                        }
                        else
                        {
                            tmp.Quantity -= ProductSizeNow[i].Quantity;
                            tmp.Quantity += qt;
                        }
                    }  
                    else
                    {
                        p.Quantity += qt;
                        ProductSize psz = db.ProductSizes.FirstOrDefault(x=> x.ProductId==ipr && x.SizeName==item.Size && x.ColorName==item.Color);
                        if(psz==null)
                        {
                            psz = new ProductSize();
                            psz.ProductId = ipr;
                            psz.SizeName = item.Size;
                            psz.ColorName = item.Color;
                            psz.Quantity = qt;
                            db.ProductSizes.Add(psz);
                            db.SaveChanges();
                        }    
                        else
                        {
                            psz.Quantity += qt;
                            db.SaveChanges();
                        }    
                    }    
                }
                else
                {
                    p = new Product();
                    p.Title = item.Title;
                    p.OriginalPrice = item.OriginalPrice;
                    p.Price = item.Price;
                    p.Quantity = qt;
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
                    tmp.ProductId = ipr;
                    tmp.ColorName = item.Color;
                    tmp.SizeName = item.Size;
                    tmp.Quantity = qt;
                    db.ProductSizes.Add(tmp);

                    //them hinh mac dinh cua san pham
                    ProductImage pi = new ProductImage();
                    pi.ProductId = p.Id;
                    pi.Image = "/Uploads/images/IMG/R.png";
                    pi.IsDefault = true;
                    db.ProductImages.Add(pi);
                }
                i++;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}