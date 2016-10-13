using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ABS.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public SessionTimeoutAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            string path = filterContext.HttpContext.Request.FilePath.ToString();
            int length = path.ToString().Length;
            if (HttpContext.Current.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            if (HttpContext.Current.Session["UserID"] != null && HttpContext.Current.Session["CompanyID"] != null)
            {
                Int64 companyID =Convert.ToInt64( HttpContext.Current.Session["CompanyID"]);
                Int64 userID = Convert.ToInt64(HttpContext.Current.Session["UserID"]);
                ABS.Models.ERP_Entities contex = new Models.ERP_Entities();
                if(length>1)
                {
                    var menuPermission = (from menu in contex.CmnMenus
                                      join per in contex.CmnMenuPermissions on menu.MenuID equals per.MenuID
                                      where per.UserID == userID && per.IsDeleted == false && per.CompanyID==companyID
                                      && menu.MenuPath.Contains(path)
                                      select menu).ToList();
                    if(menuPermission.Count()==0 && path!="/Home" && !path.Contains("/SystemCommon"))
                    {
                        HttpContext.Current.Session["UserID"] = null;
                        HttpContext.Current.Session["CompanyID"] = null;
                        filterContext.Result = new RedirectResult("~/Account/Login");
                        return;
                    }
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}
