using BotDetect.Web.UI.Mvc;
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
    public class AccountController : Controller
    {
        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "SampleCaptcha")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Error = "Captcha không đúng.";
                return View(model);
            }
            using (QLBHEntities ctx = new QLBHEntities())
            {
                int n = ctx.Users.Where(u => u.f_Username == model.UID).Count();

                if (n > 0)
                {
                    ViewBag.Error = model.UID + " đã tồn tại.";
                    return View(model);
                }

                User us = new User
                {
                    f_Username = model.UID,
                    f_Password = StringUtils.MD5(model.PWD),
                    f_Name = model.FullName,
                    f_Email = model.Email,

                    f_DOB = DateTime.ParseExact(model.DOB, "d/M/yyyy", null),
                    f_Permission = 0
                };

                ctx.Users.Add(us);
                ctx.SaveChanges();
            }
            Url.Action("login", "account");
            ViewBag.Status = true;

            return View();
        }
        //
        // GET: /Account/Login
        public ActionResult Login(string retUrl)
        {
            if (CurrentContext.IsLogged())
            {
                if (string.IsNullOrEmpty(retUrl) == false)
                {
                    ViewBag.RetUrl = retUrl;
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string username = model.UID;
            string enc_pwd = StringUtils.MD5(model.PWD);

            using (QLBHEntities ctx = new QLBHEntities())
            {
                User us = ctx.Users
                     .Where(u => u.f_Username == username && u.f_Password == enc_pwd).FirstOrDefault();
                if (us != null)
                {
                    Session["IsLogin"] = 1;
                    Session["CurUser"] = us;

                    if (model.RememberMe)
                    {
                        Response.Cookies["Username"].Value = username;
                        Response.Cookies["Username"].Expires = DateTime.Now.AddDays(7);
                    }


                    if (us.f_Permission > 0 && string.IsNullOrEmpty(model.RetUrl))
                    {
                            return RedirectToAction("admin_Dashboard", "Home");
                    }
                    else if (string.IsNullOrEmpty(model.RetUrl))
                    {
                            return RedirectToAction("Index", "Home");
                    }
                    return Redirect(model.RetUrl);
                }
                else
                {
                    ViewBag.Error = "Thông tin đăng nhập không đúng.";
                    ViewBag.RetUrl = model.RetUrl;
                    return View(model);
                }
            }
        }

        //
        // POST: /Account/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            CurrentContext.Destroy();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Profile
        [LoginRequired]
        public ActionResult UsProfile()
        {
            if (notify != null)
            {
                ViewBag.Notify = notify;
                notify = string.Empty;
            }
            int id = CurrentContext.GetCurUser().f_ID;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                User us = ctx.Users.Where(u => u.f_ID == id).FirstOrDefault();

                return View(us);
            }
        }

        static string notify = string.Empty;
        //
        // POST: /Account/UpdatePWD_UsProfile
        [LoginRequired]
        [HttpPost]
        public ActionResult changePWD_UsProfile(string PWD, string NewPWD)
        {
            int id = CurrentContext.GetCurUser().f_ID;
            string enc_pwd = StringUtils.MD5(PWD);
            string enc_newpwd = StringUtils.MD5(NewPWD);

            using (QLBHEntities ctx = new QLBHEntities())
            {
                int n = ctx.Users.Where(u => u.f_ID == id && u.f_Password == enc_pwd).Count();

                if (n == 0)
                {
                    notify = "fail";
                    RedirectToAction("UsProfile", "Account");
                }
                else
                {
                    User us = ctx.Users.Where(u => u.f_ID == id).First();

                    us.f_Password = enc_newpwd;
                    ctx.SaveChanges();

                    notify = "success";
                }
                return RedirectToAction("UsProfile", "Account");
            }
        }

        //
        // POST: /Account/UpdateProfile_UsProfile
        [LoginRequired]
        [HttpPost]
        public ActionResult changeProfile_UsProfile(string FullName, string Email, string DOB)
        {
            int id = CurrentContext.GetCurUser().f_ID;
            using (QLBHEntities ctx = new QLBHEntities())
            {
                User us = ctx.Users.Where(u => u.f_ID == id).First();
                us.f_DOB = DateTime.ParseExact(DOB, "d/M/yyyy", null);
                us.f_Name = FullName;
                us.f_Email = Email;
                ctx.SaveChanges();
                notify = "success";
            }
            return RedirectToAction("UsProfile", "Account");
        }
    }
}