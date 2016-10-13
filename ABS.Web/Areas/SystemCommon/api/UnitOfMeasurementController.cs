using ABS.Models;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/UnitOfMeasurement")]
    public class UnitOfMeasurementController : ApiController 
    { 
       
        private iUnitOfMeasurementMgt objUnitOfMeasurementService = null;

        public UnitOfMeasurementController()
        {
            objUnitOfMeasurementService = new UnitOfMeasurementMgt();
        }

        [Route("GetUnitOfMeasurement/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnUOM))]
        [HttpGet]
        public IEnumerable<CmnUOM> GetUnitOfMeasurement(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnUOM> objListUnitOfMeasurement = null;
            try
            {
                objListUnitOfMeasurement = objUnitOfMeasurementService.GetUnitOfMeasurement(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListUnitOfMeasurement;
        }

        [Route("GetUOMGroup/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnUOMGroup))]
        [HttpGet]
        public IEnumerable<CmnUOMGroup> GetUOMGroup(int? pageNumber, int? pageSize, int? IsPaging) 
        {
            IEnumerable<CmnUOMGroup> objListUOMGroup = null;
            try
            {
                objListUOMGroup = objUnitOfMeasurementService.GetUOMGroup(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListUOMGroup;
        }

        // GET: GetUnitOfMeasurementById/1
        [Route("GetUnitOfMeasurementById/{id:int}")]
        [ResponseType(typeof(CmnUOM))]
        [HttpGet]
        public CmnUOM GetUnitOfMeasurementById(int id)
        {
            CmnUOM objUnitOfMeasurement = null;
            try
            {
                objUnitOfMeasurement = objUnitOfMeasurementService.GetUnitOfMeasurementById(id).FirstOrDefault();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objUnitOfMeasurement;
        }

        // POST SaveUpdateUnitOfMeasurement
        [ResponseType(typeof(CmnUOM))]
        [HttpPost]
        public HttpResponseMessage SaveUpdateUnitOfMeasurement(CmnUOM model)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    result = objUnitOfMeasurementService.SaveUpdateUnitOfMeasurement(model);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE DeleteUnitOfMeasurement/1
        [Route("DeleteUnitOfMeasurement/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUnitOfMeasurement(int id)
        {
            int result = 0;
            try
            {
                result = objUnitOfMeasurementService.DeleteUnitOfMeasurement(id); 
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
