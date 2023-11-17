using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebBanHangOnline.Models;

namespace WebBanHangOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            /*ApplyPromotion();*/
        }

       /* private void ApplyPromotion()
        {
            using (ApplicationDbContext db = new ApplicationDbContext()) // Thay th? "YourDbContext" b?ng tên th?c t? c?a DbContext c?a b?n
            {
                var tp = db.TimePromotions.ToList();
                var dtn = DateTime.Now;

                foreach (var item in tp)
                {
                    if (dtn >= item.StartDate && dtn <= item.EndDate)
                    {
                        item.IsActive = true;
                        db.SaveChanges();
                    }
                }

                tp = db.TimePromotions.Where(x => x.IsActive == true).ToList();

                if (tp != null)
                {
                    foreach (var item in tp)
                    {
                        var tpd = db.TimePromotionDetails.Where(x => x.TimePromotionId == item.Id).ToList();

                        if (tpd != null)
                        {
                            foreach (var t in tpd)
                            {
                                var p = db.Products.FirstOrDefault(x => x.Id == t.ProductId);

                                if (p != null)
                                {
                                    p.PriceSale = p.Price * (1 - item.PercentValue / 100);
                                    p.IsSale = true;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }*/
    }
}
