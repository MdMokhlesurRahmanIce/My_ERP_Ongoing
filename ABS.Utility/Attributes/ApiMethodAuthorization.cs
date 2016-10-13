using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace ABS.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ApiMethodAuthorization : AuthorizationFilterAttribute
    {
        private const string apiToken = "methodtoken";

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            string tokenHeader = ctx.Request.Headers[apiToken];
            if (IsAuthorizeAccess(tokenHeader)) { return; }
            filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            base.OnAuthorization(filterContext);
        }

        private bool IsAuthorizeAccess(string tokenHeader)
        {
            bool result = false;
            try
            {
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
