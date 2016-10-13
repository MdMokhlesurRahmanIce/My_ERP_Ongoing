using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory
{
    public class InventoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Inventory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                    name: "InventoryApiAction",
                    routeTemplate: "Inventory/api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                   name: "InventoryApi",
                   routeTemplate: "Inventory/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Inventory_default",
                "Inventory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}