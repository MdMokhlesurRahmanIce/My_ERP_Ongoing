using ABS.Models.ViewModel.EmailService;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility;
using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ABS.Web.Controllers
{
    public class AccountController : Controller
    {
        private iCmnUserMgt objUserService = null;
        private EmailService objMailService = null;

        public AccountController()
        {
            this.objUserService = new CmnUserMgt();
            this.objMailService = new EmailService();
        }

        // GET: Account
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View();
            }
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View();
            }
        }

        // GET: Account/Profile
        //[SessionTimeout]
        public ActionResult MyProfile()
        {
            return View();
        }

        // GET: Account/Login
        [HttpPost]
        [AllowAnonymous]
        public JsonResult Login(vmLoginUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = 0; vmAuthenticatedUser objAuthMember = null;
                    objAuthMember = objUserService.Get_CmnUserAuthentication(model);
                    if (objAuthMember != null)
                    {
                        result = Convert.ToInt32(objAuthMember.ReturnValue);
                        if (result == 1)
                        {
                            Session["UserID"] = objAuthMember.UserID;
                            Session["CompanyID"] = objAuthMember.CompanyID;
                            Session["CompanyName"] = objAuthMember.CompanyName;
                            Session["CustomCode"] = objAuthMember.CustomCode;
                            Session["UserFirstName"] = objAuthMember.UserFirstName;
                            Session["UserFullName"] = objAuthMember.UserFullName;
                            Session["UserTypeID"] = objAuthMember.UserTypeID;
                            Session["UserTypeName"] = objAuthMember.UserTypeName;
                            Session["UserGroupID"] = objAuthMember.UserGroupID;
                            Session["GroupName"] = objAuthMember.GroupName;
                            Session["ClientIP"] = HostService.GetIP();
                            Session["CompanyShortName"] = objAuthMember.CompanyShortName;
                            string ShortName = objAuthMember.CompanyShortName;
                        }
                    }

                    return Json(new { status = result });
                }
                catch (Exception e)
                {
                    return Json(new { status = false, errors = e.ToString() });
                }
            }
            return Json(new
            {
                status = false,
                errors = ModelState.Keys.SelectMany(i => ModelState[i].Errors).Select(m => m.ErrorMessage).ToArray()
            });
        }
        
        // GET: Account/Recover
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Recover(vmRecoverUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = 0; int emailResult = 0;
                    vmRecoverUser objRecUser = null;
                    objRecUser = objUserService.Get_CmnUserRecovery(model);
                    if (objRecUser != null)
                    {
                        result = Convert.ToInt32(objRecUser.ReturnValue);
                        if (result == 1)
                        {
                            //send recovery mail 
                            emailResult = await objMailService.PasswordRecovery(objRecUser);
                        }
                    }

                    return Json(new { status = result });
                }
                catch (Exception e)
                {
                    return Json(new { status = false, errors = e.ToString() });
                }
            }
            return Json(new
            {
                status = false,
                errors = ModelState.Keys.SelectMany(i => ModelState[i].Errors).Select(m => m.ErrorMessage).ToArray()
            });
        }

        // GET: Account/LogOut
        [HttpPost]
        public JsonResult LogOut()
        {
            int result = 0;
            FormsAuthentication.SignOut();
            Session.Abandon();
            result = 1;
            return Json(new { status = result });
        }

       
    }
}