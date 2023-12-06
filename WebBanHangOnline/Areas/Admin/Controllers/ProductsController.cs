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
    [Authorize(Roles = "Admin,Employee,StoreKeeper")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Products
        public ActionResult Index(int? page, string SO)
        {
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id);
            ViewBag.TenSanPham = SO == "TenSanPham" ? "TenSanPhamd" : "TenSanPham";
            ViewBag.ProductCategoryId = SO == "DanhMuc" ? "DanhMucd" : "DanhMuc";
            ViewBag.Quantity = SO == "Quantity" ? "Quantityd" : "Quantity";
            ViewBag.Price = SO == "Price" ? "Priced" : "Price";
            ViewBag.CreatedDate = SO == "CreatedDate" ? "CreatedDated" : "CreatedDate";
            ViewBag.Home = SO == "Home" ? "Homed" : "Home";
            ViewBag.Hot = SO == "Hot" ? "Hotd" : "Hot";
            ViewBag.Feature = SO == "Feature" ? "Featured" : "Feature";
            ViewBag.Sale = SO == "Sale" ? "Saled" : "Sale";
            ViewBag.Active = SO == "Active" ? "Actived" : "Active";
            var pageSize = 10;
            if (page==null)
            {
                page = 1;
            }
            switch(SO)
            {
                case "TenSanPham": items = items.OrderBy(x => x.Title); break;
                case "TenSanPhamd": items = items.OrderByDescending(x => x.Title); break;
                case "DanhMuc": items = items.OrderBy(x => x.ProductCategoryId); break;
                case "DanhMucd": items = items.OrderByDescending(x => x.ProductCategoryId); break;
                case "Quantity": items = items.OrderBy(x => x.Quantity); break;
                case "Quantityd": items = items.OrderByDescending(x => x.Quantity);break;
                case "CreatedDate": items = items.OrderBy(x => x.CreatedDate);break;
                case "CreatedDated": items = items.OrderByDescending(x => x.CreatedDate); break;
                case "Home": items = items.OrderBy(x => x.IsHome);break;
                case "Homed": items = items.OrderByDescending(x => x.IsHome); break;
                case "Hot": items = items.OrderBy(x => x.IsHot); break;
                case "Hotd": items = items.OrderByDescending(x => x.IsHot); break;
                case "Feature": items = items.OrderBy(x => x.IsFeature); break;
                case "Featured": items = items.OrderByDescending(x => x.IsFeature); break;
                case "Sale": items = items.OrderBy(x => x.IsSale); break;
                case "Saled": items = items.OrderByDescending(x => x.IsSale); break;
                case "Active": items = items.OrderBy(x => x.IsActive); break;
                case "Actived": items = items.OrderByDescending(x => x.IsActive); break;
                case "Price": items = items.OrderBy(x => x.Price); break;
                case "Priced": items = items.OrderByDescending(x => x.Price); break;
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
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(),"Id","Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images , List<int> rDefault )
        {
            if(ModelState.IsValid)
            {
                if(Images!=null && Images.Count > 0)
                {
                    for(int i=0;i<Images.Count;i++)
                    {
                        if(i+1 == rDefault[0])
                        {
                            model.ProductImage.Add(new ProductImage { 
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = true,
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage {
                                ProductId = model.Id,
                                Image = Images[i],
                                IsDefault = false,
                            });
                        } 
                            
                    }    
                }
                Random rd = new Random();
                model.ProductCode = "SP" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                model.CreatedDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.CategoryId = 3;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }    
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = WebBanHangOnline.Models.Commons.Filter.FilterChar(model.Title);
                }    
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            var item = db.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = WebBanHangOnline.Models.Commons.Filter.FilterChar(model.Title);
                model.CategoryId = 3;
                db.Products.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                var checkImg = item.ProductImage.Where(x=>x.ProductId == item.Id).ToList();
                if(checkImg !=null)
                { 
                    for(int i=0;i<checkImg.Count;i++)
                    {
                        db.ProductImages.Remove(checkImg[i]);
                        db.SaveChanges();
                    }    
                    /*foreach(var img in checkImg)
                    {
                        db.ProductImages.Remove(img);
                        db.SaveChanges();
                    }   */ 
                }    
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsHot(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHot = !item.IsHot;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isHot = item.IsHot });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsFeature(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsFeature = !item.IsFeature;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isFeature = item.IsFeature });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });
        }
    }
}