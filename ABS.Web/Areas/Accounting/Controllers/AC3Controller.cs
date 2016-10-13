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
    public class AC3Controller : Controller
    {
        private readonly ERP_Entities _db;

        public AC3Controller()
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


                var ac3List = (from ac3 in _db.AccAC3
                               join ac1 in _db.AccAC1 on ac3.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac3.AC2Id equals ac2.Id
                               select new ViewModelAC3
                               {
                                   Id = ac3.Id,
                                   AC3Name = ac3.AC3Name,
                                   AC3ManualCode = ac3.AC3ManualCode,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
                  ).ToList();




                return View(ac3List);


            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
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
                var autoNumber = UniqueCode.GetAutoNumber("AC3");
                ViewBag.Code = UniqueCode.GetAccountLedgerCode("AL3", autoNumber);
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
        public JavaScriptResult Create(ViewModelAC3 vmaC3)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var userId = Convert.ToInt32(Session["UserID"]);

                    //convert viewModel to Entity for leve 3
                    var ac3 = vmaC3.MakeAc3(userId);


                    if (vmaC3.IsAccountLedger)
                    {
                        var isNameAvailble4 = _db.AccAC4.Any(x => x.AC4Name == vmaC3.AC3Name);
                        var isNameAvailble5 = _db.AccACDetails.Any(x => x.ACName == vmaC3.AC3Name);
                        if (isNameAvailble4 || isNameAvailble5)
                        {
                            return JavaScript(string.Format("UYResult('{0}','{1}')", "Same Name already exists in Level 4 or Level 5!", "failure"));
                        }
                        var vmAc4 = new ViewModelAC4
                        {
                            AC1Id = ac3.AC1Id,
                            AC2Id = ac3.AC2Id,
                            AC3Id = ac3.Id,
                            AC4Name = ac3.AC3Name
                        };
                        var aC4 = vmAc4.MakeAc4(userId);



                        var vmAc5 = new ViewModelAC5
                        {
                            AC1Id = ac3.AC1Id,
                            AC2Id = ac3.AC2Id,
                            AC3Id = ac3.Id,
                            AC4Id = aC4.Id,
                            ACName = ac3.AC3Name
                        };
                        var aC5 = vmAc5.MakeAc5(userId);


                        _db.AccAC4.Add(aC4);
                        _db.AccACDetails.Add(aC5);

                    }

                    _db.AccAC3.Add(ac3);
                    _db.SaveChanges();

                    return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success",
                        "redirect", Url.Content("~/Accounting/AC3/Create")));
                }

                return JavaScript(string.Format("UYResult('{0}','{1}')", "Please provide all valid data!", "failure"));

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
            var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
            var userId = Convert.ToInt32(Session["UserID"]);

            var userPrev = UniqueCode.MenuPermission(menuId, userId);
            ViewBag.UserViewPrivilige = userPrev.EnableView;
            ViewBag.UserAddPrivilige = userPrev.EnableInsert;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {

                var ac3List = (from ac3 in _db.AccAC3
                               join ac1 in _db.AccAC1 on ac3.AC1Id equals ac1.Id
                               join ac2 in _db.AccAC2 on ac3.AC2Id equals ac2.Id
                               where ac3.Id == id
                               select new ViewModelAC3
                               {
                                   Id = ac3.Id,
                                   AC1Id = ac3.AC1Id,
                                   AC2Id = ac3.AC2Id,
                                   AC3Name = ac3.AC3Name,
                                   AC3ManualCode = ac3.AC3ManualCode,
                                   AC1Name = ac1.AC1Name,
                                   AC2Name = ac2.AC2Name
                               }
                    ).FirstOrDefault();


                if (ac3List == null)
                {
                    return HttpNotFound();

                }
                return View(ac3List);
            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }
        }


        [HttpPost]

        [MenuAuthorization]
        public ActionResult Edit(ViewModelAC3 aC3)
        {

            try
            {

                var oldAc3 = _db.AccAC3.SingleOrDefault(b => b.Id == aC3.Id);
                if (oldAc3 != null)
                {
                    oldAc3.AC3Name = aC3.AC3Name;
                    oldAc3.AC2Id = aC3.AC2Id;
                    oldAc3.AC1Id = aC3.AC1Id;

                    if (ModelState.IsValid)
                    {
                        _db.Entry(oldAc3).State = EntityState.Modified;
                        _db.SaveChanges();
                    }

                }



                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/AC3/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }





        }


        #endregion

        public JsonResult GetAccountLedger3List(int AC2Id)
        {
            return Json(new SelectList(from r in _db.AccAC3.Where(r => r.AC2Id == AC2Id).ToList()
                                       select new { Text = r.AC3Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsNameAvailble2(string AC2Name)//parameter same name with model name
        {
            return Json(!(_db.AccAC2.Any(x => x.AC2Name == AC2Name)), JsonRequestBehavior.AllowGet);
        }  
        public JsonResult IsNameAvailble3(string AC3Name)//parameter same name with model name
        {
            return Json(!(_db.AccAC3.Any(x => x.AC3Name == AC3Name)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsNameAvailble4(string AC4Name)//parameter same name with model name
        {
            return Json(!(_db.AccAC4.Any(x => x.AC4Name == AC4Name)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsNameAvailble5(string ACName)//parameter same name with model name
        {
            return Json(!(_db.AccACDetails.Any(x => x.ACName == ACName)), JsonRequestBehavior.AllowGet);
        }



        #region
        public ActionResult GetLedger(string query, string type)
        {
            return Json(UniqueCode.GetLedgerList(query, type), JsonRequestBehavior.AllowGet);
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