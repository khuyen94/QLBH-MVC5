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
    public class UserController : Controller
    {
        //
        // GET: /User/Info
        public ActionResult Info(int id)
        {
            using (QLBHEntities ctx = new QLBHEntities())
            {
                var us = ctx.Users.Where(u => u.f_ID == id).First();
                return View(us);
            }
        }
	}
}