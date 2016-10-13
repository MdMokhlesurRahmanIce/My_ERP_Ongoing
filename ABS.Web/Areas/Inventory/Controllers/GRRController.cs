using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory.Controllers
{
    public class GRRController : Controller
    {
        // GET: /Inventory/Grr/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
        [SessionTimeout]
        public ActionResult GRRFromSPR()
        { 
            return View();
        }

        [SessionTimeout]
        public ActionResult GRRFromLoanSPR()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult GRRFromLoanRtrnIssue() 
        {
            return View();
        }

	}
}