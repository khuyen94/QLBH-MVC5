using QLBH_MVC.Filters;
using QLBH_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Controllers
{

    [LoginRequired(RequiredPermission = 1)]
    public class CategoryAdminController : Controller
    {
        //
        // GET: /CategoryAdmin/getAll
        public ActionResult getAll()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                List<Category> list = ctx.Categories.ToList();

                //ViewBag.CategoryList = list;
                return View(list);
            }
        }

        //
        //POSt: /CategoryAdmin/addCat
        [HttpPost]
        public ActionResult addCat(string catName)
        {
            Category cat = new Category { CatName = catName };
            using (QLBHEntities ctx = new QLBHEntities())
            {
                ctx.Categories.Add(cat);
                ctx.SaveChanges();

                return RedirectToAction("getAll", "CategoryAdmin");
            }
        }

        //
        //POSt: /CategoryAdmin/delCat
        [HttpPost]
        public ActionResult delCat(int id)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.Where(p => p.CatID == id).ToList();
                foreach (Product p in list)
                {
                    ctx.Products.Remove(p);
                }
                Category cat = ctx.Categories.Where(c => c.CatID == id).First();
                if (cat != null)
                {
                    ctx.Categories.Remove(cat);
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }

        //
        //POSt: /CategoryAdmin/updateCat
        [HttpPost]
        public ActionResult updateCat(int id, string name)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                Category cat = ctx.Categories.Where(c => c.CatID == id).First();
                if (cat != null)
                {
                    cat.CatName = name;
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }
    }
}