using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Production.Controllers
{
    public class SetSetupController : Controller
    {
        //
        // GET: /Production/SetSetup/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}