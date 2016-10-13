using ABS.Models;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace ABS.Web.Areas.Maintenance.Controllers
{
    public class DashboardController : Controller
    {
        // GET: SystemCommon/Dashboard
        [SessionTimeout]
        public ActionResult Index()
        {
            return View();
        }
        #region ModifyCompanySession  
        public JsonResult ModifyCompanySession(int id)
        {
            try
            {
                Session["CompanyID"] = id;
                Session["CompanyName"] = new CmnCompanyMgt().GetCompanyByID(id).CompanyName;
                Session["CompanyShortName"] = new CmnCompanyMgt().GetCompanyByID(id).CompanyShortName;
                return Json(new { ComapnyID = Session["CompanyID"].ToString(), ComapnyName = Session["CompanyName"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ModifyCompanySession  
    }
}