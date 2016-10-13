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
    public class ReceiptVoucherController : Controller
    {
        private readonly ERP_Entities _db;

        public ReceiptVoucherController()
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
                var vmList = vm.GetVoucherListByType((int)EmVoucherType.ReceiptVoucher);

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
                var cpv = new ViewModelVoucherMaster();

                ViewBag.VoucherNo = TempData["VoucherNo"];
                ViewBag.Company = Convert.ToInt32(TempData["Company"]);
                ViewBag.VoucherDate = TempData["VoucherDate"];


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

                var saveMessage = vmCpv.VoucherSave(userId,EmVoucherType.ReceiptVoucher,  "RV");

                if (saveMessage.ToLower() != "ok")
                    return JavaScript(string.Format("UYResult('{0}','{1}')", saveMessage, "failure"));

                TempData["VoucherNo"] = vmCpv.VoucherNo;
                TempData["Company"] = vmCpv.CompanyId;
                TempData["VoucherDate"] = vmCpv.VoucherDateStr;

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                            "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ReceiptVoucher/Create")));
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

                var editMessage = vmCpv.VoucherEdit(userId, EmVoucherType.ReceiptVoucher, "RV");

                if (editMessage.ToLower() != "ok")
                    return JavaScript(string.Format("UYResult('{0}','{1}')", editMessage, "failure"));
                TempData["VoucherNo"] = vmCpv.VoucherNo;
                TempData["Company"] = vmCpv.CompanyId;
                TempData["VoucherDate"] = vmCpv.VoucherDateStr;
                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                            "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ReceiptVoucher/Create")));
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

            return View("_ReceiptVoucherDetail", result);
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



        #region Ajax Data Provider
        //[OutputCache(Duration=60)]
        public ActionResult AjaxDataProvider(JQueryDataTableParamModel param)
        {
            List<AccVoucherMaster> soList = _db.AccVoucherMasters.Where(e => e.VoucherTypeId == 3).OrderByDescending(o => o.VoucherNo).ToList();

            IEnumerable<AccVoucherMaster> filteredSo;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var firstNameFilter = Convert.ToString(Request["sSearch_1"]);
                var lastNameFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isFirstNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isLastNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);

                filteredSo = soList.Where(c => isFirstNameSearchable && c.VoucherNo.ToLower().Contains(param.sSearch.ToLower())
                                            || isLastNameSearchable);
            }
            else
            {
                filteredSo = soList;
            }

            var isStudentIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isFirstNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isLastNameSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AccVoucherMaster, string> orderingFunction = (c => sortColumnIndex == 1 && isStudentIdSortable ? Convert.ToString(c.VoucherNo) :
                                                          sortColumnIndex == 2 && isFirstNameSortable ? c.VoucherNo : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredSo = filteredSo.OrderBy(orderingFunction);
            else
                filteredSo = filteredSo.OrderByDescending(orderingFunction);

            var pbmlSdmSoMasters = filteredSo as IList<AccVoucherMaster> ?? filteredSo.ToList();
            var displayedSo = pbmlSdmSoMasters.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedSo select new[] { c.VoucherNo, Convert.ToString(c.VoucherDate), Convert.ToString(c.IsActive) };


            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = soList.Count(),
                iTotalDisplayRecords = pbmlSdmSoMasters.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion




        



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