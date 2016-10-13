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
    [RoutePrefix("Production/api/SizingChemicaleSetup")]
    public class SizingChemicaleSetupController : ApiController
    {
        private SizingChemicalSetupMgt objChemicalService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public SizingChemicaleSetupController()
        {
            objChemicalService = new SizingChemicalSetupMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetArticle(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListArticle = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListArticle = objCmnDDLService.GetArticle(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListArticle
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetByArticalNo(object[] data)
        {
            IEnumerable<vmSetDetail> ListSet = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSet = objCmnDDLService.GetSetByArticalNo(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSet
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetUnitSingle(object[] data)
        {
            vmProductionUOMDropDown ListUOMSingle = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListUOMSingle = objCmnDDLService.GetUnitSingle(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListUOMSingle
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSizingChemicalSetupMaster(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> ListChemicalMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListChemicalMaster = objChemicalService.GetSizingChemicalSetupMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListChemicalMaster
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSizingChemicalSetupMasterByID(object[] data)
        {
            vmChemicalSetupMasterDetail ListMasterByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMasterByID = objChemicalService.GetSizingChemicalSetupMasterByID(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMasterByID
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSizingChemicalSetupDetailByID(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> ListDetailByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListDetailByID = objChemicalService.GetSizingChemicalSetupDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListDetailByID
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveUpdateSizingChemicalMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmChemicalSetupMasterDetail Master = JsonConvert.DeserializeObject<vmChemicalSetupMasterDetail>(data[0].ToString());
            List<vmChemicalSetupMasterDetail> Detail = JsonConvert.DeserializeObject<List<vmChemicalSetupMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objChemicalService.SaveUpdateSizingChemicalMasterDetail(Master, Detail, objcmnParam);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DelUpdateSizingChemicalMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objChemicalService.DelUpdateSizingChemicalMasterDetail(objcmnParam);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }
    }
}
