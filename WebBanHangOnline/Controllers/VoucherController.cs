using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    public class VoucherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Voucher
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Voucher(int type)
        {
            var tn = DateTime.Now;
            var item = db.Vouchers.Where(x => x.Type == type && x.StartDate <= tn && tn <= x.EndDate && x.Quantity > 0);
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult UpdateQuantityVoucher(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name); //tim user 

                var item = db.Vouchers.Find(id); //tim thay voucher
                if (item != null)
                {
                    var check = db.UserVouchers.FirstOrDefault(x => x.UserId == user.Id && x.VoucherId == item.Id);
                    if (check == null)
                    {
                        item.Quantity -= 1;
                        UserVoucher uv = new UserVoucher();
                        uv.VoucherId = item.Id;
                        uv.UserId = user.Id;
                        uv.Type = item.Type;
                        db.UserVouchers.Add(uv);
                        db.Vouchers.Attach(item);
                        db.Entry(item).Property(x => x.Quantity).IsModified = true;
                        db.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            return Json(new { success = false });
        }

        public ActionResult Partial_User_Voucher(int type)
        {
            if(User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name); //tim user 

                var uv = db.UserVouchers.Where(x => x.UserId == user.Id && x.Type == type).ToList();
                if(uv.Count > 0)
                {
                    List<Voucher> lv = new List<Voucher>();
                    foreach (var item in uv)
                    {
                        var v = db.Vouchers.Find(item.VoucherId);
                        if (v != null)
                        {
                            lv.Add(v);
                        }
                    }
                    return PartialView(lv);
                }    
            }    
            return PartialView();
        }

        [HttpPost]
        public ActionResult UseVoucher(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var v = db.Vouchers.Find(id);
                if(v!=null)
                {
                    if(v.Type == 1)
                    {
                        decimal TongGioHang = (decimal)Session["TongHoaDon"];
                        decimal tmp = TongGioHang * v.PercentValue / 100;
                        if(tmp > v.Value)
                        {
                            tmp = v.Value;
                        }
                        TongGioHang -= tmp;
                        Session["TongGioHang"] = TongGioHang;
                        return Json(new { success = true });
                    }
                    else
                    {
                        decimal TienShip = 30000 - v.Value;
                        Session["TienShip"] = TienShip;
                        return Json(new { success = true });
                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            return Json(new { success = false });
        }
    }
}