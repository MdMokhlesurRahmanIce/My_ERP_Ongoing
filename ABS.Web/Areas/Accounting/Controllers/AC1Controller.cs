using ABS.Models;
using ABS.Utility.Attributes;
using ABS.Web.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class AC1Controller : Controller
    {
        private readonly ERP_Entities _db;

        public AC1Controller()
        {
            _db = new ERP_Entities();
        }

        [SessionTimeout]
        [MenuAuthorization]
        public ActionResult Index()
        {
            var accoutledgerLevel1 = _db.AccAC1
                .Select(r => new ViewModelAC1
                {
                    Id = r.Id,
                    AC1ManualCode = r.AC1ManualCode,
                    AC1Name = r.AC1Name,


                }).ToList();
            return View(accoutledgerLevel1);

        }


        public JsonResult GetAccountLedger1List()
        {
            return Json(
                new SelectList
                (from r in _db.AccAC1.ToList()
                 select new { Text = r.AC1Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

        }
        public ActionResult SubMenuLayout(int moduleId)
        {
            try
            {
                using (var db = new ERP_Entities())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    //var companyId = Convert.ToInt32(Session["CompanyId"].ToString());

                    //Permission of menu will come by user and companywise

                    //List<CmnMenu> menuList = db.CmnMenus.Where(r => r.ModuleID == moduleId).ToList();
                    var menuList = (from r in db.CmnMenus
                                    join m in db.CmnMenuPermissions
                                        on r.MenuID equals m.MenuID
                                    where r.ModuleID == moduleId && m.UserID == userId
                                    select new
                                    {
                                        MenuID = r.MenuID,
                                        MenuName = r.MenuName,
                                        MenuPath = r.MenuPath,
                                        MenuTypeID = r.MenuTypeID,
                                        ModuleID = r.ModuleID,
                                        ParentID = r.ParentID,
                                        StatusID = r.StatusID,
                                        CustomCode = r.CustomCode,
                                        CompanyID = r.CompanyID,
                                        MenuIconCss = r.MenuIconCss
                                    }).ToList().Select(r => new CmnMenu
                                              {
                                                  MenuID = r.MenuID,
                                                  MenuName = r.MenuName,
                                                  MenuPath = r.MenuPath,
                                                  MenuTypeID = r.MenuTypeID,
                                                  ModuleID = r.ModuleID,
                                                  ParentID = r.ParentID,
                                                  StatusID = r.StatusID,
                                                  CustomCode = r.CustomCode,
                                                  CompanyID = r.CompanyID,
                                                  MenuIconCss = r.MenuIconCss
                                              });


                    return View("_SubMenuLayout", menuList);



                }

            }
            catch (Exception exception)
            {
                throw exception;
            }



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