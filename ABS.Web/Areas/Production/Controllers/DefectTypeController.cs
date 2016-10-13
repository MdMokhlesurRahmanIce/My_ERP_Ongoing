using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Production.Controllers
{
    public class DefectTypeController : Controller
    {
        //
        // GET: /Production/Configuration/
       [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}