using QLBH.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_MVC.Models
{
    public class CartItemModel
    {
        public CartItem Item { get; set; }
        public Product Product { get; set; }
    }
}