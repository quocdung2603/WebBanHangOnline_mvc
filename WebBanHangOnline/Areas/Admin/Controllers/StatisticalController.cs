using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;
namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index_Categories()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetStatistical_Date(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails 
                            on o.Id equals od.OrderId
                        join p in db.Products 
                            on od.ProductId equals p.Id
                        select new
                        {
                            CreateDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if(!string.IsNullOrEmpty(fromDate))
            {
                DateTime start = DateTime.ParseExact(fromDate,"yyyy-MM-dd",null);
                query = query.Where(x => x.CreateDate >= start);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime end = DateTime.ParseExact(fromDate, "yyyy-MM-dd", null);
                query = query.Where(x => x.CreateDate < end);
            }

            var result = query.GroupBy(x=>DbFunctions.TruncateTime(x.CreateDate)).Select(x=> new {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y=>y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x=> new {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy,
            });
            return Json(new { Data= result }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetStatistical_Month(string fromMonth, string toMonth)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.Id equals od.OrderId
                        join p in db.Products on od.ProductId equals p.Id
                        select new
                        {
                            CreateDate = o.CreatedDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            if (!string.IsNullOrEmpty(fromMonth))
            {
                DateTime tmp = DateTime.ParseExact(fromMonth, "MM/yyyy", null);
                int startMonth = tmp.Month;
                int startYear = tmp.Year;
                query = query.Where(x => x.CreateDate.Month >= startMonth && x.CreateDate.Year >= startYear);
            }

            if (!string.IsNullOrEmpty(toMonth))
            {
                DateTime tmp = DateTime.ParseExact(toMonth, "MM/yyyy", null);
                int endMonth = tmp.Month;
                int endYear = tmp.Year;
                query = query.Where(x => x.CreateDate.Month <= endMonth && x.CreateDate.Year <= endYear);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreateDate)).Select(x => new {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date, // Assuming you want the first day of the month
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy,
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetStatistical_Categories ()
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                            on o.Id equals od.OrderId
                        join p in db.Products
                            on od.ProductId equals p.Id
                        join pc in db.ProductCategories
                            on p.ProductCategoryId equals pc.Id
                        select new
                        {
                            Title = pc.Title,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            var result = query.GroupBy(x => x.Title).Select(x => new {
                Title = x.Key,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new { 
                Title = x.Title,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy,
            });
            return Json(new { Data = result}, JsonRequestBehavior.AllowGet);
        }
    }
}