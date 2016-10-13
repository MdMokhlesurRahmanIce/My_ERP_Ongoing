using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;

namespace ABS.Web.SystemCommon.api
{
    
    [RoutePrefix("SystemCommon/api/Module")]
    public class ModuleController :ApiController  

    {
        private iCmnModuleMgt objModService = null;

        public ModuleController()
        {
            this.objModService = new CmnModuleMgt();
        }


        // GET: CompanyonDemand
        [Route("GetCompany/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(tbl_ProductOutlet))]
        [HttpGet]
        public List<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCompany> objListCompany= null;
            try
            {
                objListCompany = objModService.GetCompanyOnDemand().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }


        // GET: GetCustomers/0/10/0
        [Route("GetModules/{companyID:int}/{userID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public IEnumerable<vmCmnModule> GetModules(int? companyID, int? userID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnModule> objListModules = null;
            try
            {
                objListModules = objModService.GetModules(companyID, userID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListModules;
        }

        //// GET: GetCustomerByID/1
        [Route("GetModuleByID/{id:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public vmCmnModule GetCompanyByID(int? id)
        {
            vmCmnModule objModule = null;
            try
            {
                objModule = objModService.GetModuleByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objModule;
        }

        //// POST SaveCustomer
        [ResponseType(typeof(CmnModule))]
        [HttpPost]
        public HttpResponseMessage SaveModule(CmnModule model)
        {
            int result = 0;
            try
            {
                //By Default Company Status Will be Active(1)
                model.StatusID = 1;
                result = objModService.SaveModule(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// PUT UpdateCustomer/1
        [ResponseType(typeof(CmnModule))]
        [HttpPut]
        public HttpResponseMessage UpdateModule(CmnModule model)
        {
            int result = 0;
            try
            {
                //By Default Company Status Will be Active(1)
                model.StatusID = 1;
                result = objModService.UpdateModule(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// DELETE DeleteCustomer/1
        [Route("DeleteModule/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteModule(int? id)
        {
            int result = 0;
            try
            {
                result = objModService.DeleteModule(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [ResponseType(typeof(MemberUserStatusPost))]
        [HttpPost]
        public HttpResponseMessage GetData(MemberUserStatusPost objPost)
        {

            int result = 0;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        
    }
}
