using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ABS.Web.Areas.Inventory.Controllers
{
    public class CostController : Controller
    {

        //
        // GET: /Inventory/Cost/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult IndexAccessment()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult IndexClearing()
        {
            return View();
        }
        
        [SessionTimeout]
        public ActionResult IndexTransport()
        {
            return View();
        }

        
	}
}