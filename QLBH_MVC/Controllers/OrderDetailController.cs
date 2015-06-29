using QLBH_MVC.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_MVC.Models;

namespace QLBH_MVC.Controllers
{
    [LoginRequired(RequiredPermission = 1)]
    public class OrderDetailController : Controller
    {
        //
        // GET: /OrderDetail/
        public ActionResult Index(int id)
        {
            decimal total = 0;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.OrderDetails.Include("Product").Where(o => o.OrderID == id).ToList();
                foreach (OrderDetail o in list)
                {
                    total += o.Price * o.Quantity;
                }

                ViewBag.Total = total;
                return View(list);
            }
        }
	}
}