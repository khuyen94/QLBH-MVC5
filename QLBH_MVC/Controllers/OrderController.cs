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
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        public ActionResult Index()
        {
            int total = 0;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var list = ctx.Orders.Include("User").ToList();
                total = list.Count();

                ViewBag.Total = total;
                return View(list);
            }
        }

        //
        //POSt: /Order/delOrder
        [HttpPost]
        public ActionResult delOrder(int id)
        {
            bool status = false;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                Order order = ctx.Orders.Where(c => c.OrderID == id).First();
                if (order != null)
                {
                    ctx.Orders.Remove(order);
                    ctx.SaveChanges();
                    status = true;
                }

                return Json(status);
            }
        }
        //
        //POSt: /Order/updateOrder
        public ActionResult updateOrder(int id, int sttOrder)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {               
                var order = ctx.Orders.Where(c => c.OrderID == id).First();
                if (order != null)
                {
                    order.Status = sttOrder;
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index","Order");
        }
	}
}