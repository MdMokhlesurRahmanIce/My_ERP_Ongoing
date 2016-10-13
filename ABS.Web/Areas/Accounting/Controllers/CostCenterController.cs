using ABS.Models;
using ABS.Web.Utility;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ABS.Web.Areas.Accounting.Controllers
{
    public class CostCenterController : Controller
    {

        private readonly ERP_Entities db = new ERP_Entities();
        //const int MenuId = 20;

        // GET: Admin/CostCenter
        [SessionExpire]
        [MenuAuthorization]
        public ActionResult Index()
        {


            try
            {

                var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
                var userId = Convert.ToInt32(Session["UserID"]);

                var userPrev = UniqueCode.MenuPermission(menuId, userId);
                ViewBag.UserAddPrivilige = userPrev.EnableInsert;
                ViewBag.UserEditPrivilige = userPrev.EnableUpdate;


                return View(db.AccCostCenterInfoes.ToList());


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
            try
            {
                var menuId = Convert.ToInt32(RouteData.Values["menuId"].ToString());
                var userId = Convert.ToInt32(Session["UserID"]);

                var userPrev = UniqueCode.MenuPermission(menuId, userId);
                ViewBag.UserViewPrivilige = userPrev.EnableView;


                return View();



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
        public JavaScriptResult Create(AccCostCenterInfo costCenter)
        {
            try
            {

                var autoNumber = GetAutoNumber();
                costCenter.Id = Convert.ToInt32(autoNumber);
                costCenter.IsActive = true;

                if (ModelState.IsValid)
                {
                    db.AccCostCenterInfoes.Add(costCenter);
                    db.SaveChanges();
                }



                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')",
                    "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/CostCenter/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')",
                     ex.Message, "failure"));
            }

        }


        #endregion


        #region Edit
        // GET: Accounts/AC2/Edit/5

        [HttpGet]
        [SessionExpire]
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


                AccCostCenterInfo costCenter = db.AccCostCenterInfoes.Find(id);



                if (costCenter == null)
                {
                    return HttpNotFound();
                }
                return View(costCenter);


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
        public ActionResult Edit(AccCostCenterInfo costCenter)
        {

            try
            {
                var oldCostCenter = db.AccCostCenterInfoes.SingleOrDefault(b => b.Id == costCenter.Id);
                if (oldCostCenter != null)
                {
                    oldCostCenter.CostCenterName = costCenter.CostCenterName;
                    if (ModelState.IsValid)
                    {
                        db.Entry(oldCostCenter).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }



                return JavaScript(string.Format("UYResult('{0}','{1}','{2}','{3}')", "Data saved successfully.", "success", "redirect", Url.Content("~/Accounting/CostCenter/Index")));
            }
            catch (Exception ex)
            {
                return JavaScript(string.Format("UYResult('{0}','{1}')", ex.Message, "failure"));
            }





        }


        #endregion


        private string GetAutoNumber()
        {
            string id;
            try
            {

                var data = db.AccCostCenterInfoes.ToList();
                id = data.Max(r => Convert.ToInt32(r.Id) + 1).ToString();


            }
            catch (Exception)
            {
                id = "1";
            }
            return id;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}