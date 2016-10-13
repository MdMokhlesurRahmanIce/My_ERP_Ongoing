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
    [RoutePrefix("Production/api/WeavingMachineConfigurtion")]
    public class WeavingMachineConfigurtionController : ApiController
    {
        private iProductionDDLMgt objCmnDDLService = null;
        private iWeavingMachineConfigurationMgt _objWeavingMachineConfiguration = null;

        public WeavingMachineConfigurtionController()
        {
            _objWeavingMachineConfiguration = new WeavingMachineMgt();
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
        public IHttpActionResult GetLines(object[] data)
        {
            IEnumerable<vmWeavingLine> LineList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            //int recordsTotal = 0;
            try
            {
                LineList = objCmnDDLService.GetWeavingLine(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                //recordsTotal,
                LineList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingMachineConfigurations(object[] data)
        {
            int recordsTotal = 0;
            List<vmWeavingLine> weavingMachines = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                weavingMachines = _objWeavingMachineConfiguration.GetWeavingMachineConfigurations(objcmnParam, out recordsTotal).ToList();

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
        public HttpResponseMessage SaveWeavingMachineConfi(object[] data)
        {
            string result = string.Empty;
            PrdWeavingMachinConfig model = JsonConvert.DeserializeObject<PrdWeavingMachinConfig>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                result = _objWeavingMachineConfiguration.SaveWeavingMachineConfi(model, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteWeavingMachineConfig(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = _objWeavingMachineConfiguration.DeleteWeavingMachineConfig(objcmnParam);

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
