using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class PreCostingController : Controller
    {
        //
        // GET: /Accounting/PreCosting/
         [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}