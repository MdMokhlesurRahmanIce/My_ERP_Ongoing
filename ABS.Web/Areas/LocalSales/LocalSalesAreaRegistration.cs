using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.LocalSales
{
    public class LocalSalesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LocalSales";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                    name: "LocalSalesApiAction",
                    routeTemplate: "LocalSales/api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                   name: "LocalSalesApi",
                   routeTemplate: "LocalSales/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "LocalSales_default",
                "LocalSales/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}