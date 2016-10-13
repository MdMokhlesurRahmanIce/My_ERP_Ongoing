using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.Production
{
    public class ProductionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Production";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                    name: "ProductionApiAction",
                    routeTemplate: "Production/api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                   name: "ProductionApi",
                   routeTemplate: "Production/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Production_default",
                "Production/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}