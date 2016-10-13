using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Production.Controllers
{
    public class DailyWastageController : Controller
    {
        // GET: Production/DailyWastage
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}