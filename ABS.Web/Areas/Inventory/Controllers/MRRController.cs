using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory.Controllers
{
    public class MRRController : Controller
    {
        // GET: /Inventory/MRR/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult InternalReceive()
        {
            return View();

        }

        [SessionTimeout]
        public ActionResult IssueReceive() 
        {
            return View();

        }

        [SessionTimeout]
        public ActionResult LoanReceive()
        {
            return View(); 

        }

        [SessionTimeout]
        public ActionResult ReturnReceive()
        {
            return View(); 

        } 

	}
}