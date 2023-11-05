using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class ArticleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Article
        public ActionResult Index(string alias)
        {
            var item = db.Posts.FirstOrDefault(x=>x.Alias == alias);
            return View(item);
        }

        public ActionResult Partial_Voucher(int type)
        {
            var tn = DateTime.Now;
            var item = db.Vouchers.Where(x => x.Type == type && x.StartDate <= tn && tn <= x.EndDate && x.Quantity > 0);
            return PartialView(item);
        }
    }
}