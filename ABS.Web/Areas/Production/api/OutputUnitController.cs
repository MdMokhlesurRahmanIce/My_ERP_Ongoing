using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/OutputUnit")]
    public class OutputUnitController : ApiController
    {
        private iOutputUnitMgt objOutputUnitService = null;

        public OutputUnitController()
        {
            objOutputUnitService = new OutputUnitMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetOutputName(object[] data)
        {
            IEnumerable<CmnOrganogram> ListOutputUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListOutputUnit = objOutputUnitService.GetOutputName(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                ListOutputUnit
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetOutputUnitInfo(object[] data)
        {
            IEnumerable<PrdOutputUnit> objOutputUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objOutputUnit = objOutputUnitService.GetOutputUnitInfo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objOutputUnit
            });
            //return objvmSalLCDetail;
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateOutputUnit(object[] data)
        {
            string result = string.Empty;
            PrdOutputUnit itemMaster = JsonConvert.DeserializeObject<PrdOutputUnit>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    result = objOutputUnitService.SaveUpdateOutputUnit(itemMaster, objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateOutPutList(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objOutputUnitService.DeleteUpdateOutPutList(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                result
            });
            //return objvmSalLCDetail;
        }
    }
}
