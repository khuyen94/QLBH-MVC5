using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_MVC.Models;
using System.Configuration;

namespace QLBH_MVC.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/byCat/{id, page}
        public ActionResult byCat(int? id, int page = 1)
        {
            if(id.HasValue == false)
            {
                return RedirectToAction("Index","Home");
            }

            int pageSz = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            using(QLBHEntities ctx = new QLBHEntities())
            {
                var query = ctx.Products.Where(p => p.CatID == id);
                int count = query.Count();
                int nPages = count / pageSz + (count % pageSz > 0 ? 1 : 0);

                List<Product> list = query.OrderBy(p => p.ProID).Skip((page-1)*pageSz).Take(pageSz).ToList();

                ViewBag.CatName = list.First().Category.CatName;
                ViewBag.CurPage = page;
                ViewBag.PageCount = nPages;
                ViewBag.CatId = id;

                return View(list);
            }
        }

        //
        // GET: /Product/byType/{id, page}
        public ActionResult byType(int? id, int page = 1)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            int pageSz = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            using (QLBHEntities ctx = new QLBHEntities())
            {
                var query = ctx.Products.Where(p => p.TypeID == id);
                int count = query.Count();
                int nPages = count / pageSz + (count % pageSz > 0 ? 1 : 0);

                List<Product> list = query.OrderBy(p => p.ProID).Skip((page - 1) * pageSz).Take(pageSz).ToList();

                ViewBag.TypeName = list.First().TypeProduct.TypeName;
                ViewBag.CurPage = page;
                ViewBag.PageCount = nPages;
                ViewBag.TypeId = id;

                return View(list);
            }
        }

        //
        // GET: /Product/Detail/id
        public ActionResult Detail(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }
            using (QLBHEntities ctx = new QLBHEntities())
            {
                Product pro = ctx.Products.Include("Category").Include("TypeProduct").Where(p => p.ProID == id).FirstOrDefault();

                ViewBag.CatId = pro.CatID;
                ViewBag.TypeId = pro.TypeID;
                return View(pro);
            }
        }

        //
        //GET: /Product/NewProduct
        public ActionResult NewProduct()
        {
            using(QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.OrderByDescending(p => p.ProID).Take(5).ToList();

                return PartialView(list);
            }
        }

        //
        //GET: /Product/BestSaleProduct
        public ActionResult BestSaleProduct()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.OrderByDescending(p => p.SaleQuantity).Take(5).ToList();

                return PartialView(list);
            }
        }

        //
        //GET: /Product/BestViewProduct
        public ActionResult BestViewProduct()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.OrderByDescending(p => p.PageView).Take(5).ToList();

                return PartialView(list);
            }
        }

        //
        //GET: /Product/LikeCatProduct
        public ActionResult LikeCatProduct(int id)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.Where(p => p.CatID == id).Take(5).ToList();

                return PartialView(list);
            }
        }

        //
        //GET: /Product/LikeTypeProduct
        public ActionResult LikeTypeProduct(int id)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.Where(p => p.TypeID == id).Take(5).ToList();

                return PartialView(list);
            }
        }
	}
}