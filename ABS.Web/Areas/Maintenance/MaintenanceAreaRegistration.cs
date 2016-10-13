using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.Maintenance
{
    public class MaintenanceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Maintenance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                   name: "MaintenanceApiAction",
                   routeTemplate: "Maintenance/api/{controller}/{action}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.Routes.MapHttpRoute(
                   name: "MaintenanceApi",
                   routeTemplate: "Maintenance/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );
            context.MapRoute(
                "Maintenance_default",
                "Maintenance/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}