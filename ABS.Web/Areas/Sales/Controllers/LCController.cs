using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Sales.Controllers
{
    public class LCController : Controller
    {
       
        // GET: /Sales/LC/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}