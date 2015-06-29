using QLBH_MVC.Filters;
using QLBH_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Home/admin_Dashboard
        [LoginRequired(RequiredPermission = 1)]
        public ActionResult admin_Dashboard()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                ViewBag.CountCat = ctx.Categories.Count().ToString();
                ViewBag.CountPro = ctx.Products.Count().ToString();
                ViewBag.CountOrder = ctx.Orders.Count().ToString();
                ViewBag.CountType = ctx.TypeProducts.Count().ToString();
            }
            return View();
        }

        //
        // GET: /Home/findPro/{keyword, findby, page}
        public ActionResult findPro(string keyword, string findby, int page = 1)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Product> list = null;
            int pageSz = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            using (QLBHEntities ctx = new QLBHEntities())
            {
                List<Product> temp = null;
                var query = ctx.Products.Include("Category").Include("TypeProduct");
               
                switch (findby)
                    {
                        case "0":
                            temp = query.Where(p => p.Category.CatName.Contains(keyword) || p.TypeProduct.TypeName.Contains(keyword) || p.Origin.Contains(keyword) || p.ProName.Contains(keyword)).ToList();
                            break;
                        case "1":
                            temp = query.Where(p => p.Category.CatName.Contains(keyword)).ToList();
                            break;
                        case "2":
                            temp = query.Where(p => p.TypeProduct.TypeName.Contains(keyword)).ToList();
                            break;
                        case "3":
                            temp = query.Where(p => p.Origin.Contains(keyword)).ToList();
                            break;
                        case "4":
                            temp = query.Where(p => p.ProName.Contains(keyword) && p.Price < 10000000).ToList();
                            break;
                        case "5":
                            temp = query.Where(p => p.ProName.Contains(keyword) && p.Price > 10000000 && p.Price < 20000000).ToList();
                            break;
                        case "6":
                            temp = query.Where(p => p.ProName.Contains(keyword) && p.Price > 20000000 && p.Price < 30000000).ToList();
                            break;
                        case "7":
                            temp = query.Where(p => p.ProName.Contains(keyword) && p.Price > 30000000).ToList();
                            break;
                        default:
                            goto case "0";
                    }
                int count = temp.Count();
                int nPages = count / pageSz + (count % pageSz > 0 ? 1 : 0);
                list = temp.OrderBy(p => p.ProID).Skip((page - 1) * pageSz).Take(pageSz).ToList();
                    ViewBag.CurPage = page;
                    ViewBag.PageCount = nPages;
                    ViewBag.KeyWord = keyword;
                    ViewBag.FindBy = findby;
            }
            return View(list);
        }

        //
        //GET: Home/Notify
        public ActionResult Notify()
        {
            ViewBag.Notify = "Bạn không đủ quyền truy cập.";
            return View();
        }
	}
}