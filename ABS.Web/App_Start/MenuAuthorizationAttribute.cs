using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ABS.Models;

namespace ABS.Web
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MenuAuthorizationAttribute : ActionFilterAttribute
    {
        private int MenuID;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;


            var isOk = CheckAccessRight(actionName, controllerName);
            if (isOk)
            {
                filterContext.RouteData.Values.Add("menuId", MenuID);
                return;
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Accounting/Dashboard");
                //HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);
                //var path = UrlHelper.GenerateContentUrl("~/Inventory/InventoryHome/Index", httpContext);
                //filterContext.Result = new JsonResult
                //{

                //    Data = new
                //    {
                //        // put whatever data you want which will be sent
                //        // to the client
                //        url = path
                //    },
                //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                //};

            }

            base.OnActionExecuting(filterContext);

        }

        public int GetMenuId(string controllerName, string actionName)
        {
            //var isSysAdmin = Convert.ToBoolean(Session["IsSysAdmin"]);
            var menuId = 0;
            var builder = new StringBuilder();

            try
            {
                using (var db = new ERP_Entities())
                {


                    builder.Append("/Accounting").Append("/").Append(controllerName).Append("/").Append("Create");
                    var pathCreate = builder.ToString();
                    menuId = db.CmnMenus.First(r => r.MenuPath == pathCreate).MenuID;

                }
            }
            catch (Exception e)
            {
                using (var db = new ERP_Entities())
                {
                    builder.Clear();
                    builder.Append("/Accounting").Append("/").Append(controllerName);
                    var pathIndexAndEdit = builder.ToString();
                    menuId = db.CmnMenus.First(r => r.MenuPath == pathIndexAndEdit).MenuID;

                }


            }
            return menuId;

        }
        public CmnMenuPermission UserPrivilege(int menuId, int userId)
        {
            CmnMenuPermission userPrivilege = null;
            using (var db = new ERP_Entities())
            {
                userPrivilege = db.CmnMenuPermissions.FirstOrDefault(r => r.UserID == userId && r.MenuID == menuId);
            }
            return userPrivilege;

        }
        private bool CheckAccessRight(string action, string controller)
        {
            if (HttpContext.Current.Session["UserId"] != null)
            {
                var userId = Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                var isSysAdmin = Convert.ToBoolean(HttpContext.Current.Session["IsSysAdmin"]);
                if (isSysAdmin)
                {
                    return true;
                }
                var menuId = GetMenuId(controller, action);
                MenuID = menuId;
                var userPrevlg = UserPrivilege(menuId, userId);
                switch (action)
                {
                    case "Create":
                        return userPrevlg.EnableInsert;

                        break;
                    case "Edit":
                        return userPrevlg.EnableUpdate;
                        break;
                    case "Delete":
                        return userPrevlg.EnableDelete;
                        break;
                    case "Index":
                        return userPrevlg.EnableView;
                        break;
                    default:
                        return false;
                        break;
                }

            }
            else
            {
                return false;
            }
        }
    }
}