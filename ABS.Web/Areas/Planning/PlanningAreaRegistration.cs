using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.Planning
{
    public class PlanningAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Planning";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                    name: "PlanningApiAction",
                    routeTemplate: "Planning/api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );

            context.Routes.MapHttpRoute(
                   name: "PlanningApi",
                   routeTemplate: "Planning/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "Planning_default",
                "Planning/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}