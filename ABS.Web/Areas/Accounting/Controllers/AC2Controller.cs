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

    public class AC2Controller : Controller
    {
        private readonly ERP_Entities _db;

        public AC2Controller()
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
                var ac2List = (from ac2 in _db.AccAC2
                               join ac1 in _db.AccAC1 on ac2.AC1Id equals ac1.Id
                               select new ViewModelAC2
                               {
                                   Id = ac2.Id,
                                   AC2Name = ac2.AC2Name,
                                   AC2ManualCode = ac2.AC2ManualCode,
                                   AC1Name = ac1.AC1Name
                               }
               ).ToList();





                return View(ac2List);


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

            var autoNumber = UniqueCode.GetAutoNumber("AC2");
            ViewBag.Code = UniqueCode.GetAccountLedgerCode("AL2", autoNumber);




            try
            {

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
        public ActionResult Create(ViewModelAC2 vmAC2)
        {
            try
            {

                var aC2 = vmAC2.viewModelEntity();


                var autoNumber = UniqueCode.GetAutoNumber("AC2");
                aC2.Id = Convert.ToInt32(autoNumber);
                aC2.AC2ManualCode = UniqueCode.GetAccountLedgerCode("AL2", autoNumber);
                aC2.IsActive = true;
                aC2.Transfered = false;
                aC2.DateAdded = DateTime.Now;
                aC2.AddedBy = Convert.ToInt32(Session["UserID"]);
                aC2.DateUpdated = DateTime.Now;
                aC2.UpdatedBy = Convert.ToInt32(Session["UserID"]);
                if (ModelState.IsValid)
                {
                    _db.AccAC2.Add(aC2);
                    _db.SaveChanges();


                }
                //ModelState.Clear();

                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                    "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/AC2/Create")));


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

                var ac2List = (from ac2 in _db.AccAC2
                               join ac1 in _db.AccAC1 on ac2.AC1Id equals ac1.Id
                               where ac2.Id == id
                               select new ViewModelAC2
                               {
                                   Id = ac2.Id,
                                   AC1Id = ac2.AC1Id,
                                   AC2Name = ac2.AC2Name,
                                   AC2ManualCode = ac2.AC2ManualCode,
                                   AC1Name = ac1.AC1Name
                               }
              ).FirstOrDefault();

                if (ac2List == null)
                {
                    return HttpNotFound();

                }
                return View(ac2List);


            }

            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                    ex.Message, "failure"));
            }





        }

        [HttpPost]

        [MenuAuthorization]
        public ActionResult Edit(ViewModelAC2 aC2)
        {

            try
            {

                var oldAc2 = _db.AccAC2.SingleOrDefault(b => b.Id == aC2.Id);
                if (oldAc2 == null)
                    return
                        JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data Not Found",
                            "failure", "redirect", Url.Content("~/Accounting/AC2/Create")));
                oldAc2.AC2Name = aC2.AC2Name;
                oldAc2.AC1Id = aC2.AC1Id;
                if (ModelState.IsValid)
                {
                    _db.Entry(oldAc2).State = EntityState.Modified;
                    _db.SaveChanges();
                }


                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/AC2/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }





        }


        #endregion





        public JsonResult GetAccountLedger2List(int AC1Id)
        {
            return Json(new SelectList(from r in _db.AccAC2.Where(r => r.AC1Id == AC1Id).ToList()
                                       select new { Text = r.AC2Name, Value = r.Id }, "Value", "Text"), JsonRequestBehavior.AllowGet);
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