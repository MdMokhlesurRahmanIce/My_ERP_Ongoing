using System;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session.IsNewSession || HttpContext.Current.Session["UserId"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                    var path = UrlHelper.GenerateContentUrl("~/Account/Login", httpContext);
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            // put whatever data you want which will be sent
                            // to the client
                            url = path
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {

                    filterContext.Result = new RedirectResult("~/Account/Login");
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}