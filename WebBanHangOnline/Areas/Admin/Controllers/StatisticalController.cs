using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

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
        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
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
                DateTime start = DateTime.ParseExact(fromDate,"dd/MM/yyyy",null);
                query = query.Where(x => x.CreateDate >= start);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime end = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
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

    }
}