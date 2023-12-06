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
    public class VoucherController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Voucher
        public ActionResult Index(string SO,int ? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Voucher> items = db.Vouchers.ToList();
            ViewBag.TenVoucher = SO == "TenCombo" ? "TenCombod" : "TenCombo";
            ViewBag.Loai = SO == "NgayTao" ? "NgayTaod" : "NgayTao";
            ViewBag.SoLuong = SO == "NguoiTao" ? "NguoiTaod" : "NguoiTao";
            switch (SO)
            {
                case "TenCombo": items = items.OrderBy(x => x.Title); break;
                case "TenCombod": items = items.OrderByDescending(x => x.Title); break;
                case "NgayTao": items = items.OrderBy(x => x.Type); break;
                case "NgayTaod": items = items.OrderByDescending(x => x.Type); break;
                case "NguoiTao": items = items.OrderBy(x => x.Quantity); break;
                case "NguoiTaod": items = items.OrderByDescending(x => x.Quantity); break;
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
            List<Voucher> listvoucher = new List<Voucher> {
                new Voucher { Title = "Giảm Giá Trên Tổng Bill", Value = 1 },
                new Voucher { Title = "Giảm phí vận chuyển", Value = 2 }
            };
            ViewBag.vch = new SelectList(listvoucher, "Value", "Title");
            
            return View();
        }

        [HttpPost]
        public ActionResult Add(Voucher model,FormCollection data)
        {
            if(ModelState.IsValid)
            {
                model.StartDate = model.StartDate.AddHours(Convert.ToInt32(data["SHour"]));
                model.StartDate = model.StartDate.AddMinutes(Convert.ToInt32(data["SMinute"]));

                model.EndDate = model.EndDate.AddHours(Convert.ToInt32(data["EHour"]));
                model.EndDate = model.EndDate.AddMinutes(Convert.ToInt32(data["EMinute"]));
                model.CreatedDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                if(User.Identity.IsAuthenticated)
                {
                    model.CreatedBy = User.Identity.Name;
                    model.ModifierBy = User.Identity.Name;
                }
                db.Vouchers.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
       
        public ActionResult Detail(int id)
        {
            var item = db.Vouchers.Find(id);
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            List<Voucher> listvoucher = new List<Voucher> {
                new Voucher { Title = "Giảm Giá Trên Tổng Bill", Value = 1 },
                new Voucher { Title = "Giảm phí vận chuyển", Value = 2 }
            };
            ViewBag.vch = new SelectList(listvoucher, "Value", "Title");
            var item = db.Vouchers.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voucher model, FormCollection data)
        {
            if (ModelState.IsValid)
            {
                //xem thêm lại cách gán giá trị từ select -> model
                model.StartDate = model.StartDate.AddHours(Convert.ToInt32(data["SHour"]));
                model.StartDate = model.StartDate.AddMinutes(Convert.ToInt32(data["SMinute"]));

                model.EndDate = model.EndDate.AddHours(Convert.ToInt32(data["EHour"]));
                model.EndDate = model.EndDate.AddMinutes(Convert.ToInt32(data["EMinute"]));

                model.ModifierDate = DateTime.Now;
                
                if (User.Identity.IsAuthenticated)
                {
                    model.ModifierBy = User.Identity.Name;
                }
                db.Vouchers.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Vouchers.Find(id);
            if(item!=null)
            {
                db.Vouchers.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }    
            return Json(new { success = false });
        }
    }
}