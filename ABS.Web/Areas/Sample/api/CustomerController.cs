using ABS.Models;
using ABS.Service.Sample.Factories;
using ABS.Service.Sample.Interfaces;
using ABS.Utility.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;



namespace ABS.Web.Areas.Sample.api
{
    [RoutePrefix("Sample/api/Customer")]
    public class CustomerController : ApiController
    {
        private iCustomerMgt objCusService = null;

        public CustomerController()
        {
            this.objCusService = new CustomerMgt();
        }

        // GET: GetCustomers/0/10/0
        [Route("GetCustomers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(tbl_Customer))]
        [HttpGet]
        public IEnumerable<tbl_Customer> GetCustomers(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<tbl_Customer> objListCustomer = null;
            try
            {
                objListCustomer = objCusService.GetCustomers(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCustomer;
        }

        // GET: GetCustomerByID/1
        [Route("GetCustomerByID/{id:int}")]
        [ResponseType(typeof(tbl_Customer))]
        [HttpGet]
        public tbl_Customer GetCustomerByID(int? id)
        {
            tbl_Customer objCustomer = null;
            try
            {
                objCustomer = objCusService.GetCustomerByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCustomer;
        }

        // POST SaveCustomer
        [ResponseType(typeof(tbl_Customer))]
        [HttpPost]
        public HttpResponseMessage SaveCustomer(tbl_Customer model)
        {
            int result = 0;
            try
            {
                result = objCusService.SaveCustomer(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // PUT UpdateCustomer/1
        [ResponseType(typeof(tbl_Customer))]
        [HttpPut]
        public HttpResponseMessage UpdateCustomer(tbl_Customer model)
        {
            int result = 0;
            try
            {
                result = objCusService.UpdateCustomer(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteCustomer/1
        [Route("DeleteCustomer/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteCustomer(int? id)
        {
            int result = 0;
            try
            {
                result = objCusService.DeleteCustomer(id);
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
