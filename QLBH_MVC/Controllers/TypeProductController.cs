using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_MVC.Models;

namespace QLBH_MVC.Controllers
{
    public class TypeProductController : Controller
    {
        //
        // GET: /TypeProduct/getAll
        public ActionResult getAll()
        {
            using(QLBHEntities ctx = new QLBHEntities())
            {
                List<TypeProduct> list = ctx.TypeProducts.ToList();

                return PartialView(list);
            }
            
        }
	}
}