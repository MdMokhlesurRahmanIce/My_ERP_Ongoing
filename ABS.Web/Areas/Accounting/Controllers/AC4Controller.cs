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
    public class AC4Controller : Controller
    {
        private readonly ERP_Entities _db;

        public AC4Controller()
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


                var ac4List = (from ac4 in _db.AccAC4
                               join ac1 in _db.AccAC1 on ac4.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac4.AC2Id equals ac2.Id
                               join ac3 in _db.AccAC3 on ac4.AC3Id equals ac3.Id
                               select new ViewModelAC4
                               {
                                   Id = ac4.Id,
                                   AC4Name = ac4.AC4Name,
                                   AC4ManualCode = ac4.AC4ManualCode,
                                   AC3Name = ac3.AC3Name,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
               ).ToList();

                return View(ac4List);

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
                var autoNumber = UniqueCode.GetAutoNumber("AC4");
                ViewBag.Code = UniqueCode.GetAccountLedgerCode("AL4", autoNumber);
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
        public JavaScriptResult Create(ViewModelAC4 vmaC4)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var userId = Convert.ToInt32(Session["UserID"]);

                    //Convert viewmodel to entity
                    var ac4 = vmaC4.MakeAc4(userId);


                    if (vmaC4.IsAccountLedger)
                    {

                        var isNameAvailble5 = _db.AccACDetails.Any(x => x.ACName == vmaC4.AC4Name);
                        if (isNameAvailble5)
                        {
                            return JavaScript(string.Format("UYResult('{0}','{1}')", "Same Name already exists in Level 5!", "failure"));
                        }

                        var vmAc5 = new ViewModelAC5
                        {
                            AC1Id = ac4.AC1Id,
                            AC2Id = ac4.AC2Id,
                            AC3Id = ac4.AC3Id,
                            AC4Id = ac4.Id,
                            ACName = ac4.AC4Name
                        };
                        var aC5 = vmAc5.MakeAc5(userId);

                        _db.AccACDetails.Add(aC5);

                    }

                    _db.AccAC4.Add(ac4);
                    _db.SaveChanges();


                    return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success",
                       "redirect", Url.Content("~/Accounting/AC4/Create")));

                }

                return JavaScript(string.Format("UYResult('{0}','{1}')", "Please provide valid data", "failure"));
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

                var ac4List = (from ac4 in _db.AccAC4
                               join ac1 in _db.AccAC1 on ac4.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac4.AC2Id equals ac2.Id
                               join ac3 in _db.AccAC3 on ac4.AC3Id equals ac3.Id
                               where ac4.Id == id
                               select new ViewModelAC4
                               {
                                   Id = ac4.Id,
                                   AC1Id = ac4.AC1Id,
                                   AC2Id = ac4.AC2Id,
                                   AC3Id = ac4.AC3Id,
                                   AC4Name = ac4.AC4Name,
                                   AC4ManualCode = ac4.AC4ManualCode,
                                   AC3Name = ac3.AC3Name,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
                ).SingleOrDefault();

                if (ac4List == null)
                {
                    return HttpNotFound();

                }
                return View(ac4List);

            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }


        [HttpPost]

        [MenuAuthorization]
        public ActionResult Edit(ViewModelAC4 aC4)
        {

            try
            {

                var oldAc4 = _db.AccAC4.SingleOrDefault(b => b.Id == aC4.Id);
                if (oldAc4 != null)
                {
                    oldAc4.AC4Name = aC4.AC4Name;
                    oldAc4.AC3Id = aC4.AC3Id;

                    oldAc4.AC2Id = aC4.AC2Id;
                    oldAc4.AC1Id = aC4.AC1Id;

                    if (ModelState.IsValid)
                    {
                        _db.Entry(oldAc4).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                }



                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/AC4/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }





        }


        #endregion

        public JsonResult GetAccountLedger4List(int AC3Id)
        {
            return Json(new SelectList(from r in _db.AccAC4.Where(r => r.AC3Id == AC3Id).ToList()
                                       select new { Text = r.AC4Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
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
