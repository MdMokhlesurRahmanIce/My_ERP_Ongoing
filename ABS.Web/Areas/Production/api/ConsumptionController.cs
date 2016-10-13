using ABS.Models;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/Consumption")]
    public class ConsumptionController : ApiController
    {
        private iConsumptionMgt objConsumption = null;

        public ConsumptionController()
        {
            objConsumption = new ConsumptionMgt();
        }

        [ResponseType(typeof(RndConsumptionType))]
        [HttpPost]
        public HttpResponseMessage SaveConsumption(RndConsumptionType model)
        {
            int result = 0;
            try
            {
                result = objConsumption.SaveConsumption(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetConsumptions/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(RndConsumptionType))]
        [HttpGet]
        public IEnumerable<RndConsumptionType> GetConsumptions(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<RndConsumptionType> Consumptions = null;
            try
            {
                Consumptions = objConsumption.GetConsumptions(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Consumptions;
        }

        [Route("GetConsumptionById/{id:int}")]
        [ResponseType(typeof(RndConsumptionType))]
        [HttpGet]
        public RndConsumptionType GetConsumptionById(int id)
        {
            RndConsumptionType objConsumptionType = null;
            try
            {
                objConsumptionType = objConsumption.GetConsumptionByID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objConsumptionType;
        }

        [ResponseType(typeof(RndConsumptionType))]
        [HttpPut]
        public HttpResponseMessage UpdateConsumption(RndConsumptionType model)
        {            
            int result = 0;
            try
            {
                result = objConsumption.UpdateConsumption(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [ResponseType(typeof(RndConsumptionType))]
        [HttpPut]
        public HttpResponseMessage DeleteConsumption(RndConsumptionType model)
        {
            int result = 0;          
            try
            {               
                result = objConsumption.DeleteConsumption(model);
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
