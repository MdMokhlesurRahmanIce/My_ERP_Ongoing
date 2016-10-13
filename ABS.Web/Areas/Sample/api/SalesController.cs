using ABS.Models.Sample;
using ABS.Service.Sample.Factories;
using ABS.Service.Sample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Sample.api
{
    [RoutePrefix("Sample/api/Sales")]
    public class SalesController : ApiController
    {
        private iSalesMgt objSalesService = null;

        public SalesController()
        {
            objSalesService = new SalesMgt();
        }

        [Route("GetSales/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(tbl_Sales))]
        [HttpGet]
        public IEnumerable<tbl_Sales> GetSales(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<tbl_Sales> objListSales = null;
            try
            {
                objListSales = objSalesService.GetSales(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSales;

        }

        // GET: GetSalesById/1
        [Route("GetSalesById/{id:int}")]
        [ResponseType(typeof(tbl_Sales))]
        [HttpGet]
        public tbl_Sales GetSalesById(int id)
        {
            tbl_Sales objSales = null;
            try
            {
                objSales = objSalesService.GetSalesById(id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSales;
        }

        // POST SaveSales
        [ResponseType(typeof(tbl_Sales))]
        [HttpPost]
        public HttpResponseMessage SaveUpdateSales(tbl_Sales model)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = objSalesService.SaveUpdateSales(model);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteSales/1
        [Route("DeleteSales/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSales(int id)
        {
            int result = 0;
            try
            {
                result = objSalesService.DeleteSales(id);
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
