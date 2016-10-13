using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/FinishingChemicalConsumption")]
    public class FinishingChemicalConsumptionController : ApiController
    {
        private ProductionEntryMgt objProductionEntryService = null;
        private iFinishingChemicalConsumptionMgt objFCC = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public FinishingChemicalConsumptionController()
        {
            objProductionEntryService = new ProductionEntryMgt();
            objFCC = new FinishingChemicalConsumptionMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public vmPrdFinishingMRRMasterShrinkage GetWeavingSetInformation(object[] data)
        {
            vmPrdFinishingMRRMasterShrinkage model = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                model = objProductionEntryService.GetWeavingSetInformation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return model;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingSetNo(object[] data)
        {
            IEnumerable<PrdWeavingMRRMaster> AllWSetNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                AllWSetNo = objCmnDDLService.GetWeavingSetNo(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                AllWSetNo
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFinishingType(object[] data)
        {
            IEnumerable<PrdFinishingType> AllFinishingType = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                AllFinishingType = objProductionEntryService.GetFinishingType(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                AllFinishingType
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetUnit(object[] data)
        {
            IEnumerable<vmProductionUOMDropDown> ListUOM = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListUOM = objCmnDDLService.GetUnit(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListUOM
            });
            //return objDOMaster.ToList();
        }

        public HttpResponseMessage SaveUpdateChemConsumptionInfo(object[] data)
        {
            vmChemicalSetupMasterDetail itemMaster = JsonConvert.DeserializeObject<vmChemicalSetupMasterDetail>(data[0].ToString());
            List<vmChemicalSetupMasterDetail> itemDetails = JsonConvert.DeserializeObject<List<vmChemicalSetupMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objFCC.SaveUpdateChemConsumptionInfo(itemMaster, itemDetails, objcmnParam);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public IHttpActionResult GetConsumptionMasterList(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> ListFiniChemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListFiniChemMaster = objFCC.GetFiniChemConsumptionMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListFiniChemMaster
            });
        }

        [HttpPost]
        public IHttpActionResult GetFiniChemConsumptionDetailByID(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> objMachineSetupInfo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objMachineSetupInfo = objFCC.GetFiniChemConsumptionByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objMachineSetupInfo
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteFiniChemConsumptionMD(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objFCC.DeleteFiniChemConsumptionMD(objcmnParam);
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
        }
    }
}