using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.SystemCommon
{
    public class SystemCommonAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemCommon";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                       name: "SystemCommonApiAction",
                       routeTemplate: "SystemCommon/api/{controller}/{action}",
                       defaults: new { id = RouteParameter.Optional }
                   );

            context.Routes.MapHttpRoute(
                   name: "SystemCommonApi",
                   routeTemplate: "SystemCommon/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );


            context.MapRoute(
                "SystemCommon_default",
                "SystemCommon/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


           // context.MapRoute(
           //    "SystemCommon_Session",
           //    "SystemCommon/{controller}/{action}/{Cid}/{CName}",
           //    new { action = "Index", id = UrlParameter.Optional, Name = UrlParameter.Optional }
           //);

        }
    }
}