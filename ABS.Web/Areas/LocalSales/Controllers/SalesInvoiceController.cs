using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.LocalSales.Controllers
{
    public class SalesInvoiceController : Controller
    {
       
        // GET: /LocalSales/LocalInvoice/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DamageSalesInvoice()
        {
            return View();
        }

	}
}