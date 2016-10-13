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
    [RoutePrefix("Production/api/ChemiclePreparation")]
    public class ChemiclePreparationController : ApiController
    {
        //private iChemiclePreparationMgt objCP = null;
        private ChemiclePreparationMgt objCP = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public ChemiclePreparationController()
        {
            objCP = new ChemiclePreparationMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFinishingProcessType(object[] data)
        {
            IEnumerable<PrdFinishingProcess> ListType = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListType = objCmnDDLService.GetFinishingProcessType(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListType
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetUnitSingle(object[] data)
        {
            vmProductionUOMDropDown ListUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListUnit = objCmnDDLService.GetUnitSingle(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListUnit
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetChemical(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListChemical = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListChemical = objCmnDDLService.GetArticle(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListChemical
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFinChemicalPreparationMaster(object[] data)
        {
            IEnumerable<vmFinishingChemicalPreparation> ListFinChem = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListFinChem = objCP.GetFinChemicalPreparationMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListFinChem
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFiniChemicalPrepDetailByID(object[] data)
        {
            IEnumerable<vmFinishingChemicalPreparation> ListFinChemByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListFinChemByID = objCP.GetFiniChemicalPrepDetailByID(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListFinChemByID
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateFiniChemicalMasterDetail(object[] data)
        {
            vmFinishingChemicalPreparation itemMaster = JsonConvert.DeserializeObject<vmFinishingChemicalPreparation>(data[0].ToString());
            List<vmFinishingChemicalPreparation> itemDetails = JsonConvert.DeserializeObject<List<vmFinishingChemicalPreparation>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemDetails.Count > 0)
                {
                    result = objCP.SaveUpdateFiniChemicalMasterDetail(itemMaster, itemDetails, objcmnParam);
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
        public IHttpActionResult DelUpdateFiniChemicalMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objCP.DelUpdateFiniChemicalMasterDetail(objcmnParam);
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


        [Route("GetChemicalPreparation")]
        [HttpPost]
        public IHttpActionResult GetChemicalPreparation(object[] data)
        {
            objCP = new ChemiclePreparationMgt();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            object[] objChemicalPreparation = null;

            try
            {
                objChemicalPreparation = objCP.GetChemicalPreparation(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objChemicalPreparation
            });
        }

    }
}
