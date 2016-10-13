using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Sample.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Sample/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}