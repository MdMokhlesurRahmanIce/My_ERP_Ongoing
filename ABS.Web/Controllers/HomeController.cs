using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ABS.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}