using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Commercial.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Commercial/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}