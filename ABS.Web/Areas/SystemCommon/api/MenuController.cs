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

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/Menu")]
    public class MenuController : ApiController
    {
        private iCmnMenuMgt objMenuService = null;
        public MenuController()
        {
            this.objMenuService = new CmnMenuMgt();
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
                objListCompany = objMenuService.GetCompanyOnDemand().ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }


        // GET: GetCustomers/0/10/0
        [Route("GetMenues/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnMenu))]
        [HttpGet]
        public IEnumerable<vmCmnMenu> GetMenues(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmCmnMenu> objListMenues = null;
            try
            {
                objListMenues = objMenuService.GetMenues(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListMenues;
        }

        //// GET: GetCustomerByID/1
        [Route("GetMenuByID/{id:int}")]
        [ResponseType(typeof(CmnMenu))]
        [HttpGet]
        public vmCmnMenu GetMenuByID(int? id)
        {
            vmCmnMenu objMenu = null;
            try
            {
                objMenu = objMenuService.GetMenuByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objMenu;
        }

        //// POST SaveCustomer
        [ResponseType(typeof(CmnMenu))]
        [HttpPost]
        public HttpResponseMessage SaveMenu(CmnMenu model)
        {
            int result = 0;
            try
            {
                //By Default Company Status Will be Active(1)
                model.StatusID = 1;
                result = objMenuService.SaveMenu(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// PUT UpdateCustomer/1
        [ResponseType(typeof(CmnMenu))]
        [HttpPut]
        public HttpResponseMessage UpdateMenu(CmnMenu model)
        {
            int result = 0;
            try
            {
                result = objMenuService.UpdateMenu(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //// DELETE DeleteCustomer/1
        [Route("DeleteMenu/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteMenu(int? id)
        {
            int result = 0;
            try
            {
                result = objMenuService.DeleteMenu(id);
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
