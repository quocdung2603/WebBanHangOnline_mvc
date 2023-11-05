using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Voucher
        public ActionResult Index()
        {
            var items = db.Vouchers;
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