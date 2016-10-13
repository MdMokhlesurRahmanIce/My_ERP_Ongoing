using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.Purchase
{
    public class PurchaseAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Purchase";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                   name: "PurchaseApiAction",
                   routeTemplate: "Purchase/api/{controller}/{action}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.Routes.MapHttpRoute(
                   name: "PurchaseApi",
                   routeTemplate: "Purchase/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Purchase_default",
                "Purchase/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}