using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Sample.Controllers
{
    public class ItemSaleController : Controller
    {
        // GET: Sample/ItemSale
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
    }
}