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

    [RoutePrefix("SystemCommon/api/Organogram")]
    public class OrganogramController :ApiController  
    {
        private iCmnOrganogramMgt objOrgnagramService = null;

        public OrganogramController()
        {
            this.objOrgnagramService = new CmnOrganogramMgt();
        }

        // GET: GetOrganograms/0/10/0
        [Route("GetOrganograms/{companyID:int}/{loggeduser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet]
        public IEnumerable<vmCmnOrganogram> GetOrganograms(int? companyID,int? loggeduser ,int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnOrganogram> objOrganogram = null;
            try
            {
                objOrganogram = objOrgnagramService.GetOrganograms(companyID, loggeduser,pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objOrganogram;
        }

        //// GET: GetCustomerByID/1
        [Route("GetOrganogramByID/{id:int}")]
        [ResponseType(typeof(CmnModule))]
        [HttpGet]
        public vmCmnOrganogram GetOrganogramByID(int? id)
        {
            vmCmnOrganogram obj = null;
            try
            {
                obj = objOrgnagramService.GetOrganogramByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return obj;
        }

        //// POST SaveCustomer
        [ResponseType(typeof(CmnOrganogram))]
        [HttpPost]
        public HttpResponseMessage SaveOrganogram(CmnOrganogram model)
        {
            int result = 0;
            try
            {
                result = objOrgnagramService.SaveOrganogram(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// PUT UpdateCustomer/1
        [ResponseType(typeof(CmnOrganogram))]
        [HttpPut]
        public HttpResponseMessage UpdateOrganogram(CmnOrganogram model)
        {
            int result = 0;
            try
            {
                result = objOrgnagramService.UpdateOrganogram(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// DELETE DeleteCustomer/1
        [Route("DeleteOrganogram/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrganogram(int? id)
        {
            int result = 0;
            try
            {
                result = objOrgnagramService.DeleteOrganogram(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        
    }
}
