using System.Web.Http;
using System.Web.Mvc;

namespace ABS.Web.Areas.HRPayroll
{
    public class HRPayrollAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HRPayroll";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                   name: "HRPayrollApiAction",
                   routeTemplate: "HR/api/{controller}/{action}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.Routes.MapHttpRoute(
                   name: "HRPayrollApi",
                   routeTemplate: "HR/api/{controller}",
                   defaults: new { id = RouteParameter.Optional }
               );

            context.MapRoute(
                "HRPayroll_default",
                "HRPayroll/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}