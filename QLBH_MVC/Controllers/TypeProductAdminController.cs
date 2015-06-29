using QLBH_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Controllers
{
    public class TypeProductAdminController : Controller
    {
        //
        // GET: /TypeProductAdmin/
        public ActionResult Index()
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.TypeProducts.ToList();
                                
                return View(list);
            }
        }

        //
        //POSt: /TypeProductAdmin/addType
        [HttpPost]
        public ActionResult addType(string typeName)
        {
            TypeProduct type = new TypeProduct { TypeName = typeName };
            using (QLBHEntities ctx = new QLBHEntities())
            {
                ctx.TypeProducts.Add(type);
                ctx.SaveChanges();

                return RedirectToAction("Index", "TypeProductAdmin");
            }
        }

        //
        //POSt: /TypeProductAdmin/delType
        [HttpPost]
        public ActionResult delType(int id)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Products.Where(p => p.TypeID == id).ToList();
                foreach (Product p in list)
                {
                    ctx.Products.Remove(p);
                }
                var type = ctx.TypeProducts.Where(c => c.TypeID == id).First();
                if (type != null)
                {
                    ctx.TypeProducts.Remove(type);
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }

        //
        //POSt: /TypeProductAdmin/updateType
        [HttpPost]
        public ActionResult updateType(int id, string name)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var type = ctx.TypeProducts.Where(c => c.TypeID == id).First();
                if (type != null)
                {
                    type.TypeName = name;
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }
	}
}