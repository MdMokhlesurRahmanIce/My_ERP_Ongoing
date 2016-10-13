using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ABS.Models;
using ABS.Service.Sample.Factories;
using ABS.Service.Sample.Interfaces;
using System.Web.Http.Description;


namespace ABS.Web.Areas.Sample.api
{

    [RoutePrefix("Sample/api/Employee")]
    public class EmployeeController : ApiController
    {
        private iEmpMasterMgt objEmpService = null;
        

        public EmployeeController()
        {
            this.objEmpService = new EmpMasterMgt();
        }

        
        // GET: GetCustomers/0/10/0
        [Route("GetEmployee/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(EmpMaster))]
        [HttpGet]
        public IEnumerable<EmpMaster> GetEmployee(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<EmpMaster> objListEmployee = null;
            try
            {
                objListEmployee = objEmpService.GetEmployee(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListEmployee;
        }

        // GET: GetCustomerByID/1
        [Route("GetEmployeeByID/{id:int}")]
        [ResponseType(typeof(EmpMaster))]
        [HttpGet]
        public EmpMaster GetEmployeeByID(int? id)
        {
            EmpMaster objEmployee = null;
            try
            {
                objEmployee = objEmpService.GetEmployeeByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objEmployee;
        }

        
        // POST SaveCustomer
        [ResponseType(typeof(EmpMaster))]
        [HttpPost]
        public HttpResponseMessage SaveEmployee(EmpMaster model)
        {
            int result = 0;
            try
            {
                result = objEmpService.SaveEmployee(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        // PUT UpdateCustomer/1
        [ResponseType(typeof(EmpMaster))]
        [HttpPut]
        public HttpResponseMessage UpdateCustomer(EmpMaster model)
        {
            int result = 0;
            try
            {
                result = objEmpService.UpdateEmployee(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }



        // DELETE DeleteCustomer/1
        [Route("DeleteEmployee/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int? id)
        {
            int result = 0;
            try
            {
                result = objEmpService.DeleteEmployee(id);
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
