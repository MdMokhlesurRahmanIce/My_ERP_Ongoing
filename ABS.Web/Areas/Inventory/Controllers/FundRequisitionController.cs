﻿using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABS.Web.Areas.Inventory.Controllers
{
    public class FundRequisitionController : Controller
    {
        //
        // GET: /Inventory/FundRequisition/
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
	}
}