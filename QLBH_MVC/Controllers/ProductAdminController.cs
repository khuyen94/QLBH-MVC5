using QLBH_MVC.Filters;
using QLBH_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Controllers
{
    [LoginRequired(RequiredPermission = 1)]
    public class ProductAdminController : Controller
    {
        //
        // GET: /ProductAdmin/getAll/{page}
        public ActionResult getAll(int page = 1)
        {

            int pageSz = Convert.ToInt32(ConfigurationManager.AppSettings["admin_PageSize"]);

            using (QLBHEntities ctx = new QLBHEntities())
            {
                var query = ctx.Products.Include("Category").Include("TypeProduct");
                int count = query.Count();
                int nPages = count / pageSz + (count % pageSz > 0 ? 1 : 0);

                List<Product> list = query.OrderBy(p => p.ProID).Skip((page - 1) * pageSz).Take(pageSz).ToList();

                ViewBag.CurPage = page;
                ViewBag.PageCount = nPages;

                return View(list);
            }
        }

        //
        //POSt: /ProductAdmin/delPro
        [HttpPost]
        public ActionResult delPro(int id)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                Product pro = ctx.Products.Where(p => p.ProID == id).First();
                if (pro != null)
                {
                    ctx.Products.Remove(pro);
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }
                
        //
        //GET: /ProductAdmin/addProduct
        public ActionResult addProduct()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Categories.ToList();
                var list2 = ctx.TypeProducts.ToList();

                ViewBag.TypeProduct = list2;
                ViewBag.Categories = list;
                return View();
            }
            
        }


        //
        //POST: /ProductAdmin/addProduct
        [HttpPost]
        public ActionResult addProduct(Product model, HttpPostedFileBase fuMain, HttpPostedFileBase fuThumbs)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                if (string.IsNullOrEmpty(model.TinyDes)) model.TinyDes = string.Empty;
                if (string.IsNullOrEmpty(model.FullDes)) model.FullDes = string.Empty;

                ctx.Products.Add(model);
                ctx.SaveChanges();

                var list = ctx.Categories.ToList();
                var list2 = ctx.TypeProducts.ToList();

                ViewBag.TypeProduct = list2;
                ViewBag.Categories = list;
            }
            if (fuMain != null && fuMain.ContentLength > 0 && fuThumbs != null && fuThumbs.ContentLength > 0)
            {
                //tao folder chua hinh [~/Imgs/sp/{ID}]
                string spDirPath = Server.MapPath("~/Imgs/sp");
                string targetDirPath = Path.Combine(spDirPath, model.ProID.ToString());
                Directory.CreateDirectory(targetDirPath);

                //copy hinh len
                string mainFileName = Path.Combine(targetDirPath, "main.jpg");
                fuMain.SaveAs(mainFileName);

                string thumbsFileName = Path.Combine(targetDirPath, "main_thumbs.jpg");
                fuThumbs.SaveAs(thumbsFileName);

            }
            ViewBag.Added = true;
            return View(); 
        }

        //
        //GET: /ProductAdmin/editProduct
        public ActionResult editProduct(int id)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == id).FirstOrDefault();
                var list = ctx.Categories.ToList();
                var list2 = ctx.TypeProducts.ToList();

                ViewBag.TypeProduct = list2;
                ViewBag.Categories = list;
                return View(pro);
            }

        }

        //
        //POST: /ProductAdmin/editProduct
        [HttpPost]
        public ActionResult editProduct(Product model, HttpPostedFileBase fuMain, HttpPostedFileBase fuThumbs)
        {
            Product pro = new Product();
            using (QLBHEntities ctx = new QLBHEntities())
            {
                pro = ctx.Products.Where(p => p.ProID == model.ProID).First();
                if (string.IsNullOrEmpty(model.TinyDes)) model.TinyDes = string.Empty;
                if (string.IsNullOrEmpty(model.FullDes)) model.FullDes = string.Empty;

                pro.CatID = model.CatID;
                pro.TypeID = model.TypeID;
                pro.ProName = model.ProName;
                pro.Quantity = model.Quantity;
                pro.Price = model.Price;
                pro.Origin = model.Origin;
                pro.TinyDes = model.TinyDes;
                pro.FullDes = model.FullDes;
                
                ctx.SaveChanges();

                var list = ctx.Categories.ToList();
                var list2 = ctx.TypeProducts.ToList();

                ViewBag.TypeProduct = list2;
                ViewBag.Categories = list;
            }
            if (fuMain != null && fuMain.ContentLength > 0 && fuThumbs != null && fuThumbs.ContentLength > 0)
            {
                //tao folder chua hinh [~/Imgs/sp/{ID}]
                string spDirPath = Server.MapPath("~/Imgs/sp");
                string targetDirPath = Path.Combine(spDirPath, model.ProID.ToString());
                Directory.CreateDirectory(targetDirPath);

                //copy hinh len
                string mainFileName = Path.Combine(targetDirPath, "main.jpg");
                fuMain.SaveAs(mainFileName);

                string thumbsFileName = Path.Combine(targetDirPath, "main_thumbs.jpg");
                fuThumbs.SaveAs(thumbsFileName);

            }
            ViewBag.Added = true;
            return View(pro);
        }
	}
}