using ABS.Models;
using ABS.Web.Utility;
using ABS.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class ContraVoucherController : Controller
    {
        private readonly ERP_Entities _db;

        public ContraVoucherController()
        {
            _db = new ERP_Entities();
        }


        [SessionExpire]
        [MenuAuthorization]
        public ActionResult Index()
        {
            var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
            var userId = Convert.ToInt32(Session["UserID"]);

            var userPrev = UniqueCode.MenuPermission(menuId, userId);
            ViewBag.UserAddPrivilige = userPrev.EnableInsert;
            ViewBag.UserEditPrivilige = userPrev.EnableUpdate;
            try
            {

                var vm = new ViewModelVoucherMaster();
                var vmList = vm.GetVoucherListByType((int)EmVoucherType.ContraVoucher);

                return View(vmList);
            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }


        #region Create

        [HttpGet]
        [SessionExpire]
        [MenuAuthorization]
        public ActionResult Create()
        {
            var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
            var userId = Convert.ToInt32(Session["UserID"]);

            var userPrev = UniqueCode.MenuPermission(menuId, userId);
            ViewBag.UserViewPrivilige = userPrev.EnableView;

            try
            {

                ViewBag.VoucherNo = TempData["VoucherNo"];
                ViewBag.Company = Convert.ToInt32(TempData["Company"]);
                ViewBag.VoucherDate = TempData["VoucherDate"];


                var cpv = new ViewModelVoucherMaster();
                return View(cpv);

            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                     ex.Message, "failure"));
            }
        }

        [HttpPost]
        [SessionExpire]
        [MenuAuthorization]
        public JavaScriptResult Create(ViewModelVoucherMaster vmCpv)
        {
            try
            {
                var userId = Convert.ToInt32(Session["UserID"].ToString());

                var saveMessage = vmCpv.VoucherSave(userId,EmVoucherType.ContraVoucher,  "CV");

                if (saveMessage.ToLower() != "ok")
                    return JavaScript(string.Format("UYResult('{0}','{1}')", saveMessage, "failure"));
                TempData["VoucherNo"] = vmCpv.VoucherNo;
                TempData["Company"] = vmCpv.CompanyId;
                TempData["VoucherDate"] = vmCpv.VoucherDateStr;

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                    "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ContraVoucher/Create")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }


        #endregion

        #region Edit

        [HttpGet]
        [SessionExpire]
        [MenuAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
            var userId = Convert.ToInt32(Session["UserID"]);

            var userPrev = UniqueCode.MenuPermission(menuId, userId);
            ViewBag.UserViewPrivilige = userPrev.EnableView;
            ViewBag.UserAddPrivilige = userPrev.EnableInsert;

            try
            {

                var voucher = new ViewModelVoucherMaster();
                voucher = voucher.GetVoucherById(Convert.ToInt32(id), (int)EmVoucherType.ReceiptVoucher);

                if (voucher == null)
                {
                    return HttpNotFound();

                }
                return View(voucher);
            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }

        [HttpPost]
        [SessionExpire]
        [MenuAuthorization]
        public ActionResult Edit(ViewModelVoucherMaster vmCpv)
        {

            try
            {
                var userId = Convert.ToInt32(Session["UserID"].ToString());

                var editMessage = vmCpv.VoucherEdit(userId, EmVoucherType.CashVoucherHeadOffice, "CV");

                if (editMessage.ToLower() != "ok")
                    return JavaScript(string.Format("UYResult('{0}','{1}')", editMessage, "failure"));
                TempData["VoucherNo"] = vmCpv.VoucherNo;
                TempData["Company"] = vmCpv.CompanyId;
                TempData["VoucherDate"] = vmCpv.VoucherDateStr;

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                            "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ContraVoucher/Create")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                      ex.Message, "failure"));
            }


        }



        #endregion

        [HttpPost]
        public ViewResult BlankResultRow()
        {
            var result = new ViewModelVoucherDetail();

            return View("_ContraVoucherDetail", result);
        }

        public JsonResult GetVoucherType(int id)
        {
            return Json(new SelectList(from r in _db.AccVoucherTypes.ToList().Where(r => r.Id == id)
                                       select new { Text = r.Description, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostCenter()
        {
            return Json(new SelectList(from r in _db.AccCostCenterInfoes.ToList()
                                       orderby r.CostCenterName descending
                                       select new { Text = r.CostCenterName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }



        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}