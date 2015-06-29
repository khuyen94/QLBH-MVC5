using System.Web;
using System;
using System.Linq;
using QLBH_MVC.Models;

namespace QLBH.Helpers
{
    public static class CurrentContext
    {
        public static bool IsLogged()
        {
            if ((int)HttpContext.Current.Session["IsLogin"] == 1)
                return true;

            if (HttpContext.Current.Request.Cookies["Username"] != null)
            {
                HttpContext.Current.Session["IsLogin"] = 1;

                string username = HttpContext.Current.Request.Cookies["Username"].Value;
                using (QLBHEntities ctx = new QLBHEntities())
                {
                    User us = ctx.Users
                         .Where(u => u.f_Username == username).FirstOrDefault();

                    HttpContext.Current.Session["CurUser"] = us;
                }
                
                return true;
            }

            return false; 
        }

        public static User GetCurUser() 
        {
            return (User)HttpContext.Current.Session["CurUser"];
        }

        public static Cart GetCart()
        {
            return (Cart)HttpContext.Current.Session["Cart"];
        }

        public static void   Destroy() {
            HttpContext.Current.Session["IsLogin"] = 0;
            HttpContext.Current.Session["CurUser"] = null;
            GetCart().Items.Clear();
                
            HttpContext.Current.Response.Cookies["Username"].Expires =
                DateTime.Now.AddDays(-1);
        }

     
    }

}