using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility;
using ABS.Utility.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/User")]
    public class UserController : ApiController
    {
        private iCmnUserMgt objUserService = null;
        private iSystemCommonDDL objddlService = null;
        private EmailService objMailService = null;

        public UserController()
        {
            this.objUserService = new CmnUserMgt();
            this.objddlService = new SystemCommonDDL();
            this.objMailService = new EmailService();
        }

        #region GroupDDL
        // GET: GetUserGroupddl
        [Route("GetUserGroupddl/{companyID:int}/{loggedUser:int}/{userType:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmUserGroup))]
        [HttpGet]
        public List<vmUserGroup> GetUserGroupddl(int? companyID, int? loggedUser,int? userType, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmUserGroup> objListGroup = null;
            try
            {
                objListGroup = objddlService.GetUserGroupForDropDownList(companyID, loggedUser, userType, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListGroup;
        }

        // GET: GetUserGroupddl
        [Route("GetUserTypeddl/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmUserGroup))]
        [HttpGet]
        public List<vmUserType> GetUserTypeddl(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmUserType> objListType = null;
            try
            {
                objListType = objddlService.GetUserTypeForDropDownList(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListType;
        }
        #endregion

        #region Create
        // POST SaveUser
        [ResponseType(typeof(vmUser))]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveUser(object[] data)//vmUser model)
        {
            vmUser model = JsonConvert.DeserializeObject<vmUser>(data[0].ToString());
            List<vmCompany> companylist = JsonConvert.DeserializeObject<List<vmCompany>>(data[1].ToString());

            int result = 0; int emailResult = 0; vmUser objUser = null;
            try
            {
                if (model != null)
                {
                    //result = objUserService.SaveUser(model);
                    objUser = objUserService.SaveUser(model, companylist);
                    if (objUser != null)
                    {
                        result = Convert.ToInt32(objUser.ReturnValue);
                        if (result == 1)
                        {
                            emailResult = await objMailService.UserRegistration(objUser);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST SaveUserGroup
        [ResponseType(typeof(vmUserGroup))]
        [HttpPost]
        public HttpResponseMessage SaveUserGroup(vmUserGroup model)
        {
            int result = 0;
            try
            {
                if (model != null)
                    result = objUserService.SaveUserGroup(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // POST SaveUserType
        [ResponseType(typeof(vmUserType))]
        [HttpPost]
        public HttpResponseMessage SaveUserType(vmUserType model)
        {
            int result = 0;
            try
            {
                if (model != null)
                    result = objUserService.SaveUserType(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region Read
        // GET: GetUserGroup
        [HttpPost]
        public IHttpActionResult GetUserGroup(object[] data)
        {
            List<vmUserGroup> listUsersGroup = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsersGroup = objUserService.GetUserGroup(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                listUsersGroup
            });
        }

        // GET: GetUserType
        [HttpPost]
        public IHttpActionResult GetUserType(object[] data)
        {
            List<vmUserType> listUsersType = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsersType = objUserService.GetUserType(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                listUsersType
            });
        }

        // GET: GetUser
        [HttpPost]
        public IHttpActionResult GetUser(object[] data)
        {
            List<vmUser> listUsers = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                listUsers = objUserService.GetUser(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                listUsers
            });
        }

        // GET GetUserByID/1
        [Route("GetUserByID/{id:int}/{companyID:int}/{loggedUser:int}")]
        [HttpGet]
        public HttpResponseMessage GetUserByID(int? id, int? companyID, int? loggedUser)
        {
            vmUser objUser = null;
            try
            {
                if (id != null)
                    objUser = objUserService.GetUserByID(id, companyID, loggedUser);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Request.CreateResponse(HttpStatusCode.OK, objUser);
        }
        #endregion

        #region Update
        // PUT UpdateUserType/1
        [ResponseType(typeof(vmUserType))]
        [HttpPut]
        public HttpResponseMessage UpdateUserType(vmUserType model)
        {
            int result = 0;
            try
            {
                if (model != null)
                    result = objUserService.UpdateUserType(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT UpdateUserGroup/1
        [ResponseType(typeof(vmUserGroup))]
        [HttpPut]
        public HttpResponseMessage UpdateUserGroup(vmUserGroup model)
        {
            int result = 0;
            try
            {
                if (model != null)
                    result = objUserService.UpdateUserGroup(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region Delete
        // DELETE DeleteUserType/1
        [Route("DeleteUser/{id:int}/{companyID:int}/{loggedUser:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(int? id, int? companyID, int? loggedUser)
        {
            int result = 0;
            try
            {
                if (id != null)
                    result = objUserService.DeleteUser(id, companyID, loggedUser);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteUserType/1
        [Route("DeleteUserType/{id:int}/{companyID:int}/{loggedUser:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserType(int? id, int? companyID, int? loggedUser)
        {
            int result = 0;
            try
            {
                if (id != null)
                    result = objUserService.DeleteUserType(id, companyID, loggedUser);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteUserType/1
        [Route("DeleteUserGroup/{id:int}/{companyID:int}/{loggedUser:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserGroup(int? id, int? companyID, int? loggedUser)
        {
            int result = 0;
            try
            {
                if (id != null)
                    result = objUserService.DeleteUserGroup(id, companyID, loggedUser);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region Avatar
        [HttpPost]
        public HttpResponseMessage UploadAvatar()
        {
            int iUploadedCnt = 0;
            string sPath = string.Empty; string result = string.Empty;
            try
            {
               // var directory = @"D:/Upload/Avatar/";
                var directory = @"E:/Upload/Avatar/";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];
                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                            string exttension = System.IO.Path.GetExtension(hpf.FileName);
                            int fileSerial = iCnt + 1;
                            string fileName = "Avt_" + newName + fileSerial + exttension;
                            string filePath = directory + fileName;
                            hpf.SaveAs(filePath);
                            result = fileName;
                            iUploadedCnt = iUploadedCnt + 1;
                            hpf.InputStream.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public void DeleteAvatar(vmUser _vUser)
        {
            string fileName = _vUser.ImageUrl.ToString();

            try
            {
                var directory = @"E:/Upload/Avatar/";
                string filePath = directory + fileName;

                if (fileName != null)
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion

        #region Finger
        #endregion

        #region Signature
        [HttpPost]
        public HttpResponseMessage UploadSignature()
        {
            int iUploadedCnt = 0;
            string sPath = string.Empty; string result = string.Empty;
            try
            {
                var directory = @"E:/Upload/Signature/";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];
                    if (hpf.ContentLength > 0)
                    {
                        if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                        {
                            string newName = DateTime.Now.ToString("ddMMMyyhhmmsstt");
                            string exttension = System.IO.Path.GetExtension(hpf.FileName);
                            int fileSerial = iCnt + 1;
                            string fileName = "Sig_" + newName + fileSerial + exttension;
                            string filePath = directory + fileName;
                            hpf.SaveAs(filePath);
                            result = fileName;
                            iUploadedCnt = iUploadedCnt + 1;
                            hpf.InputStream.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public void DeleteSignature(vmUser _vUser)
        {
            string fileName = _vUser.SignatUrl.ToString();

            try
            {
                var directory = @"E:/Upload/Signature/";
                string filePath = directory + fileName;

                if (fileName != null)
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        #endregion     
        [Route("GetCurrentUserPassword/{companyID:int}/{loggedUser:int}")]
        [HttpGet]
        public string GetCurrentUserPassword(int companyID, int loggedUser)
        {
            string currentPassword = "";
            try
            {
                 currentPassword = objUserService.getCurrentPassword(companyID, loggedUser);
            }
            catch 
            {
                
                
            }
            return currentPassword;
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword(CmnUserAuthentication model)
        {
            int result = 0;
            try
            {

                result = objUserService.ChangePassword(model);
            }
            catch
            {
                
               
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }


}
