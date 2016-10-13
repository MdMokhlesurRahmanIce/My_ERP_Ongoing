using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory.Controllers
{
    public class IssueController : Controller
    {
       
        // GET: /Inventory/Issue/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult InternalTransfer()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult InternalReturn()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult MrrReturn()
        {
            return View();
        }

        [SessionTimeout]
        public ActionResult DamageGoodsEntry()
        {
            return View();
        }
    }
}