using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.SystemCommon.Controllers
{
    public class YarnController : Controller
    {
        //
        // GET: /SystemCommon/Yarn/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}