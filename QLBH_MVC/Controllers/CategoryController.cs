using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_MVC.Models;
using QLBH_MVC.Filters;

namespace QLBH_MVC.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Category/getAll
        public ActionResult getAll()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                List<Category> list = ctx.Categories.ToList();

                //ViewBag.CategoryList = list;
                return PartialView(list);
            }
        }
    }
}