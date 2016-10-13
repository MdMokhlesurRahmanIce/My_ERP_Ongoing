using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory.Controllers
{
    public class MRRAccountController : Controller
    {
        //
        // GET: /Inventory/MRRAccount/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]

        public ActionResult LoanAccount()
        {
            return View();
        }
	}
}