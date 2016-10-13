using ABS.Models;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Web.Areas.SystemCommon.Hubs;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Company")]
    public class CompanyController : ApiController
    {
        private iCmnCompanyMgt objComService = null;

        public CompanyController()
        {
            this.objComService = new CmnCompanyMgt();
        }
        //Get
        //BasicAuthorization
        [ HttpGet, Route("GetCompanies/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        // GET: GetCustomers/0/10/0
        //[Route("GetCompanies/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCompany))]

        public IHttpActionResult GetCompanies(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCompany> objListCompanies = null; int recordsTotal = 0;
            try
            {
                objListCompanies = objComService.GetCompanies(pageNumber, pageSize, IsPaging);
                recordsTotal = objListCompanies.Count();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objListCompanies
            });
        }

        //// GET: GetCustomerByID/1
        [BasicAuthorization]
        [Route("GetCompanyByID/{id:int}")]
        [ResponseType(typeof(CmnCompany))]
        [HttpGet]
        public CmnCompany GetCompanyByID(int? id)
        {
            CmnCompany objCustomer = null;
            try
            {
                objCustomer = objComService.GetCompanyByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCustomer;
        }

        //// POST SaveCustomer
        [ResponseType(typeof(CmnCompany))]
        [HttpPost]
        public HttpResponseMessage SaveCompany(CmnCompany model)
        {
            int result = 0;
            try
            {
                //By Default Company Status Will be Active(1)
                //  model.StatusID = 1;
                result = objComService.SaveCompany(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        //Post
        [BasicAuthorization,HttpPost]
        public IHttpActionResult SaveCompanyParam(object[] data)
        {
            int result = 0;
            CmnCompany modelFirst = JsonConvert.DeserializeObject<CmnCompany>(data[0].ToString());
            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());

            var loggedCompnyID = commonEntity.loggedCompnyID;
            var loggedUserID = commonEntity.loggedUserID;
            var MenuID = commonEntity.currentMenuID;
            var loggedUserBranchID = commonEntity.loggedUserBranchID;
            var currentModuleID = commonEntity.currentModuleID;

            var TransactionTypeID = commonEntity.currentTransactionTypeID;
            List<CmnMenu> menuList = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.MenuList.ToString());
            List<CmnMenu> ChildMenues = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.ChildMenues.ToString());

            #endregion Shared Entity
            try
            {

                //  result = objComService.SaveCompany(modelFirst);
                result = objComService.SaveCompanyParam(modelFirst, commonEntity);
                //  NotificationHubs.BroadcastData();
                NotificationHubs.BroadcastData(new NotificationEntity());
                result = 1;

            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return Json(new
            {
                result
            });
            //return _finishGoodes;
        }



        //// PUT UpdateCustomer/1
        [BasicAuthorization]
        [ResponseType(typeof(CmnCompany))]
        [HttpPost]
        public HttpResponseMessage UpdateCompany(object[] data)
        {
            int result = 0;
            try
            {

                CmnCompany model = JsonConvert.DeserializeObject<CmnCompany>(data[0].ToString());
                UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());
                result = objComService.UpdateCompany(model, commonEntity);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// DELETE DeleteCustomer/1
        [BasicAuthorization]
        [HttpPost]
        public HttpResponseMessage DeleteCompany(object[] data)
        {
            int result = 0;
            try
            {
                var id = JsonConvert.DeserializeObject<int>(data[0].ToString());
                UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());
                result = objComService.DeleteCompany(id, commonEntity);
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
