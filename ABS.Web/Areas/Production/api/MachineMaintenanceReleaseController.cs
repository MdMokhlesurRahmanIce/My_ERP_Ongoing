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

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/MachineMaintenanceRelease")]
    public class MachineMaintenanceReleaseController : ApiController
    {
        private MachineMaintenanceOrderMgt objMMOEntryService = null;
        private MachineMaintenanceReleaseMgt objMMREntryService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public MachineMaintenanceReleaseController()
        {
            objMMOEntryService = new MachineMaintenanceOrderMgt();
            objMMREntryService = new MachineMaintenanceReleaseMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMaintenanceMachine(object[] data)
        {
            IEnumerable<vmPrdWeavingMachineConfigMasterDetail> ListMaintenance = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListMaintenance = objMMREntryService.GetMaintenanceMachine(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListMaintenance
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMaintenanceMachineData(object[] data)
        {
            vmPrdWeavingMachineConfigMasterDetail MData = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                MData = objMMREntryService.GetMaintenanceMachineData(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                MData
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMntMachineMaintenanceOrde(object[] data)
        {
            int recordsTotal = 0;
            List<vmPrdWeavingMachineConfigMasterDetail> MMMOList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                MMMOList = objMMOEntryService.GetMntMachineMaintenanceOrde(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                MMMOList
            });

        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateMachineMaintenanceRelease(object[] data)
        {
            string result = string.Empty;
            vmPrdWeavingMachineConfigMasterDetail model = JsonConvert.DeserializeObject<vmPrdWeavingMachineConfigMasterDetail>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                result = objMMREntryService.SaveUpdateMachineMaintenanceRelease(model, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateMachineMaintenanceOrder(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objMMOEntryService.DeleteUpdateMachineMaintenanceOrder(objcmnParam);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                result
            });

        }
    }
}
