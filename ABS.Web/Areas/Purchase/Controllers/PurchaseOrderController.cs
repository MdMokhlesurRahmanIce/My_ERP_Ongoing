using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Purchase.Controllers
{
    public class PurchaseOrderController : Controller
    {
        //
        // GET: /Purchase/PurchaseOrder/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}