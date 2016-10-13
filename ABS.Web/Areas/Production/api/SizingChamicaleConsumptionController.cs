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
    [RoutePrefix("Production/api/SizingChamicaleConsumption")]
    public class SizingChamicaleConsumptionController : ApiController
    {
        private SizingChamicaleConsumptionMgt objChemConsumpService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public SizingChamicaleConsumptionController()
        {
            objChemConsumpService = new SizingChamicaleConsumptionMgt();
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
        public IHttpActionResult GetBatches(object[] data)
        {
            IEnumerable<vmCmnBatch> BatcheList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                BatcheList = objCmnDDLService.GetAllBatches(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                BatcheList
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSupplier(object[] data)
        {
            IEnumerable<vmBuyer> SupplierList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                SupplierList = objChemConsumpService.GetSupplier(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                SupplierList
            });
            //return objListBuyer.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCurrentStock(object[] data)
        {
            vmBallInfo singleCStock = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                singleCStock = objChemConsumpService.GetCurrentStock(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                singleCStock
            });
            //return objListBuyer.ToList();
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
        public IHttpActionResult GetSizingChemicalConsumptionMaster(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> ListChemicalMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListChemicalMaster = objChemConsumpService.GetSizingChemicalConsumptionMaster(objcmnParam, out recordsTotal).ToList();
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

        //[HttpPost, BasicAuthorization]
        //public IHttpActionResult GetSizingChemicalSetupMasterByID(object[] data)
        //{
        //    vmChemicalSetupMasterDetail ListMasterByID = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    int recordsTotal = 0;
        //    try
        //    {
        //        ListMasterByID = objChemConsumpService.GetSizingChemicalSetupMasterByID(objcmnParam, out recordsTotal);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return Json(new
        //    {
        //        recordsTotal,
        //        ListMasterByID
        //    });
        //    //return objDOMaster.ToList();
        //}

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSizingChemicalConsumptionDetailByID(object[] data)
        {
            IEnumerable<vmChemicalSetupMasterDetail> ListDetailByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());            
            try
            {
                ListDetailByID = objChemConsumpService.GetSizingChemicalConsumptionDetailByID(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
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
                result = objChemConsumpService.SaveUpdateSizingChemicalMasterDetail(Master, Detail, objcmnParam);
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
                result = objChemConsumpService.DelUpdateSizingChemicalMasterDetail(objcmnParam);
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
