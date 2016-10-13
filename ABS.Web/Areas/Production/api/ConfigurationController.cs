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
    [RoutePrefix("Production/api/Configuration")]
    public class ConfigurationController : ApiController
    {
        private iBreakageWastageStopMgt objBWSService = null;
        private iProductionDDLMgt objCmnDDLService = null;
        public ConfigurationController()
        {
            objBWSService = new BreakageWastageStopMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartment(object[] data)
        {
            IEnumerable<CmnOrganogram> ListDept = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListDept = objCmnDDLService.GetDepartment(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                ListDept
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBWSType(object[] data)
        {
            IEnumerable<CmnCombo> objListBWSType = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListBWSType = objCmnDDLService.GetBWSType(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objListBWSType
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBWSInfo(object[] data)
        {
            IEnumerable<PrdBWSlist> objBWSMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objBWSMaster = objBWSService.GetBWSInfo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objBWSMaster
            });
            //return objvmSalLCDetail;
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateBWS(object[] data)
        {
            PrdBWSlist itemMaster = JsonConvert.DeserializeObject<PrdBWSlist>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objBWSService.SaveUpdateBWS(itemMaster, objcmnParam);
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
        public IHttpActionResult DeleteUpdatePrdBWSlist(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string result = "";
            try
            {
                result = objBWSService.DeleteUpdatePrdBWSlist(objcmnParam);
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
