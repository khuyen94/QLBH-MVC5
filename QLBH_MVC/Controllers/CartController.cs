using QLBH.Helpers;
using QLBH_MVC.Filters;
using QLBH_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Controllers
{
    [LoginRequired]
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            decimal total = 0;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                List<CartItemModel> list = new List<CartItemModel>();
                Cart cart = CurrentContext.GetCart();

                foreach (var item in cart.Items)
                {
                    Product pro = ctx.Products.Where(p => p.ProID == item.ProID).First();

                    CartItemModel cim = new CartItemModel
                    {
                        Item = item,
                        Product = pro
                    };

                    total += pro.Price * item.Quantity;
                    list.Add(cim);
                }
                ViewBag.Total = total;
                return View(list);
            }
        }
        //
        // POST: /Cart/AddToCart
        [HttpPost]
        public ActionResult AddToCart(CartItem item)
        {   
            int cartItemQuantity = CurrentContext.GetCart().getQuantityPro(item.ProID);
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == item.ProID).First();

                if (pro.Quantity >= (item.Quantity + cartItemQuantity))
                {
                    CurrentContext.GetCart().Add(item);
                    return RedirectToAction("Detail", "Product", new { id = item.ProID });
                }
                else
                {
                    Session["Error"] = "Loi";
                    return RedirectToAction("Detail", "Product", new { id = item.ProID });
                }
            }
        }

        //
        // POST: /Cart/Add1ToCart
        [HttpPost]
        public ActionResult Add1ToCart(int id)
        {
            CartItem item = new CartItem();
            item.ProID = id;
            item.Quantity = 1;
            int cartItemQuantity = CurrentContext.GetCart().getQuantityPro(item.ProID);
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == item.ProID).First();

                if (pro.Quantity >= (item.Quantity + cartItemQuantity))
                {
                    CurrentContext.GetCart().Add(item);
                }
                else
                {
                    Session["Error"] = "Loi";
                }
            }
            int numberItem = CurrentContext.GetCart().GetNumberOfItem();
            return Json(numberItem);
        }

        //
        // POST: /Cart/RemoveItem
        [HttpPost]
        public ActionResult RemoveItem(int proId)
        {
            CurrentContext.GetCart().RemoveItem(proId);
            return RedirectToAction("Index", "Cart");
        }

        //
        // POST: /Cart/UpdateItem
        [HttpPost]
        public ActionResult UpdateItem(int proId, int quantity)
        {
            using(QLBHEntities ctx = new QLBHEntities())
            {
                var pro = ctx.Products.Where(p => p.ProID == proId).First();

                if(pro.Quantity >= quantity)
                {                    
                    CurrentContext.GetCart().UpdateItem(proId, quantity);
                    return RedirectToAction("Index", "Cart");
                }
                else{
                    Session["Error"] = "Loi";
                    return RedirectToAction("Index", "Cart");
                }
            }
        }

        //
        // POST: /Cart/Checkout
        [HttpPost]
        public ActionResult CheckOut(decimal total)
        {
            Order ord = new Order
            {
                OrderDate = DateTime.Now,
                UserID = CurrentContext.GetCurUser().f_ID,
                Total = total,
                Status = 0
            };
            using (QLBHEntities ctx = new QLBHEntities())
            {
                foreach (CartItem item in CurrentContext.GetCart().Items)
                {
                    Product pro = ctx.Products.Where(p => p.ProID == item.ProID).FirstOrDefault();
                    if (pro != null)
                    {
                        OrderDetail d = new OrderDetail
                        {
                            ProID = item.ProID,
                            Quantity = item.Quantity,
                            Price = pro.Price,
                            Amount = item.Quantity * pro.Price
                        };
                        ord.OrderDetails.Add(d);

                        pro.Quantity = pro.Quantity - item.Quantity; //Giảm số lượng tồn

                        if (pro.SaleQuantity != null) // Thay đổi số lượng bán
                            pro.SaleQuantity += item.Quantity;
                        else pro.SaleQuantity = item.Quantity;
                    }
                }
                ctx.Orders.Add(ord);
                ctx.SaveChanges();
            }

            CurrentContext.GetCart().Items.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}