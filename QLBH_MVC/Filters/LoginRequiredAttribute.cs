using QLBH.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_MVC.Filters
{
    public class LoginRequiredAttribute : ActionFilterAttribute
    {
        public int RequiredPermission { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentContext.IsLogged() == false)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                filterContext.Result = new RedirectResult(string.Format(
                    "~/Account/Login?retUrl=/{0}/{1}",
                    controller,
                    action
                    )
                 );
            }
            else
            {
                if (CurrentContext.GetCurUser().f_Permission < RequiredPermission)
                {
                    var Url = new UrlHelper(filterContext.RequestContext);
                    var url = Url.Action("Notify", "Home");
                    filterContext.Result = new RedirectResult(url);
                    return;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}