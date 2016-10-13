using ABS.Models;
using ABS.Web.Utility;
using ABS.Web.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    [SessionExpire]
    public class ACDetailsController : Controller
    {
        private readonly ERP_Entities _db;

        public ACDetailsController()
        {
            _db = new ERP_Entities();
        }

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
                var ac5List = (from ac5 in _db.AccACDetails
                               join ac1 in _db.AccAC1 on ac5.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac5.AC2Id equals ac2.Id
                               join ac3 in _db.AccAC3 on ac5.AC3Id equals ac3.Id
                               join ac4 in _db.AccAC4 on ac5.AC4Id equals ac4.Id

                               select new ViewModelAC5
                               {
                                   Id = ac5.Id,
                                   ACName = ac5.ACName,
                                   ACode = ac5.ACode,
                                   AC4Name = ac4.AC4Name,
                                   AC3Name = ac3.AC3Name,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
              ).ToList();


                return View(ac5List);


            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }


        #region Create
        [HttpGet]

        [MenuAuthorization]
        public ActionResult Create()
        {
            var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
            var userId = Convert.ToInt32(Session["UserID"]);

            var userPrev = UniqueCode.MenuPermission(menuId, userId);
            ViewBag.UserViewPrivilige = userPrev.EnableView;

            try
            {
                var autoNumber = UniqueCode.GetAutoNumber("AC5");
                ViewBag.Code = UniqueCode.GetAccountLedgerCode("AL5", autoNumber);
                return View();


            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                     ex.Message, "failure"));
            }
        }


        [HttpPost]

        [MenuAuthorization]
        public JavaScriptResult Create(ViewModelAC5 vmaC5)
        {
            try
            {
                var userId = Convert.ToInt32(Session["UserID"]);


                if (ModelState.IsValid)
                {
                    var aC5 = vmaC5.MakeAc5(userId);
                    _db.AccACDetails.Add(aC5);
                    _db.SaveChanges();

                    return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ACDetails/Create")));
                }

                return JavaScript(string.Format("UYResult('{0}','{1}')", "Please provide valid data!", "failure"));

            }


            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }
        }


        #endregion



        #region Edit

        [HttpGet]

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

                var ac5List = (from ac5 in _db.AccACDetails
                               join ac1 in _db.AccAC1 on ac5.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac5.AC2Id equals ac2.Id
                               join ac3 in _db.AccAC3 on ac5.AC3Id equals ac3.Id
                               join ac4 in _db.AccAC4 on ac5.AC4Id equals ac4.Id
                               where ac5.Id == id
                               select new ViewModelAC5
                               {
                                   Id = ac5.Id,
                                   AC1Id = ac5.AC1Id,
                                   AC2Id = ac5.AC2Id,
                                   AC3Id = ac5.AC3Id,
                                   AC4Id = ac5.AC4Id,
                                   ACName = ac5.ACName,
                                   ACode = ac5.ACode,
                                   AC4Name = ac4.AC4Name,
                                   AC3Name = ac3.AC3Name,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
                  ).SingleOrDefault();

                if (ac5List == null)
                {
                    return HttpNotFound();

                }
                return View(ac5List);

            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }

        }

        [HttpPost]

        [MenuAuthorization]
        public ActionResult Edit(ViewModelAC5 aC5)
        {

            try
            {

                var oldAc5 = _db.AccACDetails.SingleOrDefault(b => b.Id == aC5.Id);
                if (oldAc5 != null)
                {
                    oldAc5.ACName = aC5.ACName;
                    oldAc5.AC4Id = aC5.AC4Id;

                    oldAc5.AC3Id = aC5.AC3Id;

                    oldAc5.AC2Id = aC5.AC2Id;
                    oldAc5.AC1Id = aC5.AC1Id;

                    if (ModelState.IsValid)
                    {
                        _db.Entry(oldAc5).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                }



                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/ACDetails/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }





        }


        #endregion

        public JsonResult GetAccountLedgerList()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAccountLedgerListJournal()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       where r.AC3Id != 2
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAccountLedgerListCash()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       where r.AC4Id != 3
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetAccountLedgerListCashHo()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       //where r.AC4Id != 3 && r.Id != 2
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountLedgerListCashFo()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       //where r.AC4Id != 3 && r.Id != 42
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountLedgerListBank()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       where r.AC4Id != 2
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountLedgerListContra()
        {
            return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                       orderby r.ACName ascending
                                       where r.AC3Id == 2
                                       select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCompanyList()
        {
            using (var db = new ERP_Entities())
            {
                var userId = Convert.ToInt32(Session["UserID"].ToString());

                var result = (from p in db.CmnCompanies
                              join a in db.CmnUserWiseCompanies
                               on p.CompanyID equals a.CompanyID
                              where a.UserID == userId && p.IsDeleted == false
                              select new
                              {
                                  Text = p.CompanyName,
                                  value = p.CompanyID
                              }).Distinct().ToList();


                return Json(new SelectList(result, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetLedgerByLevel(string id)
        {
            var level = Convert.ToInt32(id);

            switch (level)
            {
                case 1:

                    return Json(new SelectList(from r in _db.AccAC1.ToList()
                                               orderby r.AC1Name ascending
                                               select new { Text = r.AC1Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

                    break;
                case 2:
                    return Json(new SelectList(from r in _db.AccAC2.ToList()
                                               orderby r.AC2Name ascending
                                               select new { Text = r.AC2Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

                    break;
                case 3:
                    return Json(new SelectList(from r in _db.AccAC3.ToList()
                                               orderby r.AC3Name ascending
                                               select new { Text = r.AC3Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

                    break;
                case 4:
                    return Json(new SelectList(from r in _db.AccAC4.ToList()
                                               orderby r.AC4Name ascending
                                               select new { Text = r.AC4Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

                    break;
                default:
                    {
                        return Json(new SelectList(from r in _db.AccACDetails.ToList()
                                                   orderby r.ACName ascending
                                                   select new { Text = r.ACName, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);

                        break;
                    }
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
