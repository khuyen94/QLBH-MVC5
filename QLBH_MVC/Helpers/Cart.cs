using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Helpers
{
    public class Cart
    {

        public List<CartItem> Items { get; set; }

        public Cart()
        {
            Items = new List<CartItem>();
        }

        public void Add(CartItem item)
        {
            CartItem existedItem = Items.Where(i => i.ProID == item.ProID).FirstOrDefault();

            if (existedItem != null)
            {
                existedItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public int GetNumberOfItem()
        {
            return Items.Sum(i=>i.Quantity);
        }

        public int getQuantityPro(int proId)
        {
            int number = 0;
            CartItem item = Items.Where(i => i.ProID == proId).FirstOrDefault();
            if (item != null)
            {
                number = item.Quantity;
            }
            return number;
        }

        public void RemoveItem(int proId)
        {
            CartItem existedItem = Items.Where(i => i.ProID == proId).FirstOrDefault();

            if (existedItem != null)
            {
                Items.Remove(existedItem);
            }
        }

        public void UpdateItem(int proId, int quantity)
        {
            CartItem existedItem = Items.Where(i => i.ProID == proId).FirstOrDefault();

            if (existedItem != null)
            {
                existedItem.Quantity = quantity;
            }
        }
    }

    public class CartItem
    {

        public int ProID { get; set; }

        public int Quantity { get; set; }
    }
}