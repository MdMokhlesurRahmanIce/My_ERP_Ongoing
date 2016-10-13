using ABS.Models;
using ABS.Models.ViewModel.Production;
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
    [RoutePrefix("Production/api/WeavingGriageReceive")]
    public class WeavingGriageReceiveController : ApiController
    {
        private iProductionDDLMgt objCmnDDLService = null;
        private iWeavingGrieageReceiveMgt _objWeavingGrieageReceive = null;
        public WeavingGriageReceiveController()
        {
            _objWeavingGrieageReceive = new WeavingGrieageReceiveMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMachine(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListMachine = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListMachine = objCmnDDLService.GetDetailsMachine(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListMachine
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetShifts(object[] data)
        {
            IEnumerable<vmShiftName> ShiftList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ShiftList = objCmnDDLService.GetShifts(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ShiftList
            });
        }
        
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetOperators(object[] data)
        {
            IEnumerable<vmOperator> OperatorList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                OperatorList = objCmnDDLService.GetOperators(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                OperatorList
            });
        }
        
        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetWeavingMachineDetailByID(object[] data)
        {
            vmWeavingGriage _objWeavingMachinesGriage = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                _objWeavingMachinesGriage = _objWeavingGrieageReceive.GetWeavingMachines(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, _objWeavingMachinesGriage);
        }
                        
        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveWeavingGriage(object[] data)
        {
            int result = 0;
            PrdWeavingMRRMaster model = JsonConvert.DeserializeObject<PrdWeavingMRRMaster>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                result = _objWeavingGrieageReceive.SaveWeavingGriage(model, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult WeavingGriageDetails(object[] data)
        {
            int recordsTotal = 0;
            List<vmWeavingGriage> WeavingGriage = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                WeavingGriage = _objWeavingGrieageReceive.WeavingGriageDetails(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                WeavingGrages = WeavingGriage
            });

        }
        
        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetWeavingGriageDetailsById(object[] data)
        {
            vmWeavingGriage _objVmWeavingGriage = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                _objVmWeavingGriage = _objWeavingGrieageReceive.GetWeavingGriageDetailsById(objcmnParam);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, _objVmWeavingGriage);
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage DeleteWeavingGriageById(object[] data)
        {
            int result = 0;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());            
            try
            {
                result = _objWeavingGrieageReceive.DeleteWeavingGriageById(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
