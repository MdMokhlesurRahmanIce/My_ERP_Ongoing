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
    [RoutePrefix("Production/api/DFSMachineConfiguration")]
    public class DFSMachineConfigurationController : ApiController
    {
        private iProductionDDLMgt objCmnDDLService = null;
        //private iWeavingMachineConfigurationMgt _objWeavingMachineConfiguration = null;
        private DFSMachineConfigMgt objweavingservice = null;
        public DFSMachineConfigurationController()
        {
            objweavingservice = new DFSMachineConfigMgt();
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
        public IHttpActionResult GetMachines(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListMachine = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMachine = objCmnDDLService.GetItemAsMachine(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMachine
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingMachineConfigMaster(object[] data)
        {
            int recordsTotal = 0;
            List<vmPrdWeavingMachineConfigMasterDetail> weavingMachines = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                weavingMachines = objweavingservice.GetWeavingMachineConfigMaster(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                weavingMachines
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingMachineConfigById(object[] data)
        {
            IEnumerable<vmPrdWeavingMachineConfigMasterDetail> MConfigByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                MConfigByID = objweavingservice.GetWeavingMachineConfigById(objcmnParam);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                MConfigByID
            });
        }  

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateWeavingMasterDetail(object[] data)
        {
            vmPrdWeavingMachineConfigMasterDetail itemMaster = JsonConvert.DeserializeObject<vmPrdWeavingMachineConfigMasterDetail>(data[0].ToString());
            List<vmPrdWeavingMachineConfigMasterDetail> itemDetails = JsonConvert.DeserializeObject<List<vmPrdWeavingMachineConfigMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemDetails.Count > 0)
                {
                    result = objweavingservice.SaveUpdateWeavingMasterDetail(itemMaster, itemDetails, objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            // System.Web.HttpContext.Current.Session.Add("LCReferenceNo", result);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateWeavingMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objweavingservice.DeleteUpdateWeavingMasterDetail(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Json(new
            {
                result
            });
            //return objDOMaster.ToList();
        }
    }
}
