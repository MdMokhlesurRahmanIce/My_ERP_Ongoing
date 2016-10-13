using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/Challan")]
    public class ChallanController : ApiController
    {

        private iChallanMgt objChallanService = null;

        public ChallanController()
        {
            objChallanService = new ChallanMgt(); 
        }

        [HttpPost]
        public IHttpActionResult GetSPRNo(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objSPRNo = objChallanService.GetSPRNo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objSPRNo
            });
        }
         
        [HttpPost]
        public IHttpActionResult GetLocation(object[] data) 
        {
            IEnumerable<CmnAddressCountry> objLocation = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objLocation = objChallanService.GetLocation(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objLocation
            });
        }

        [HttpPost]
        public IHttpActionResult GetPackingUnit(object[] data) 
        {
            IEnumerable<CmnUOM> objPackingUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objPackingUnit = objChallanService.GetPackingUnit(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objPackingUnit
            });
        }

        [HttpPost]
        public IHttpActionResult GetWeightUnit(object[] data)
        {
            IEnumerable<CmnUOM> objWeightUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objWeightUnit = objChallanService.GetWeightUnit(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objWeightUnit
            });
        }
         
        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailBySPRID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 SprID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailBySPRID = null;
            try
            {
                lstItemDetailBySPRID = objChallanService.GetItemDetailBySPRID(objcmnParam, SprID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailBySPRID;
        }

        [HttpPost]
        public IEnumerable<vmChallan> GetItmDetailByItmCode(object[] data) 
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ItemCode = data[1].ToString();

            IEnumerable<vmChallan> objItemDtls = null;
            try
            {
                objItemDtls = objChallanService.GetItmDetailByItmCode(objcmnParam, ItemCode);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemDtls;
        }
         
        [Route("GetParty/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")] 
        [ResponseType(typeof(vmCmnUser))]
        [HttpGet]
        public IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCmnUser> lstParty = null;
            try
            {
                lstParty = objChallanService.GetParty(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstParty;
        }

        //[Route("GetPISalesPerson/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnUser))]
        //[HttpGet]
        //public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<CmnUser> objListPISalesPerson = null;
        //    try
        //    {
        //        objListPISalesPerson = objPIService.GetPISalesPerson(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListPISalesPerson;
        //}

        [Route("GetItemSampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmItemGroup))]
        [HttpGet]
        public IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmItemGroup> lstSampleNo = null;
            try
            {
                lstSampleNo = objChallanService.GetItemSampleNo(pageNumber, pageSize, IsPaging);  
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSampleNo;
        }

        [Route("GetChallanTrnsTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging) 
        {
            IEnumerable<CmnCombo> lstChallanTrnsTypes = null;
            try
            {
                lstChallanTrnsTypes = objChallanService.GetChallanTrnsTypes(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanTrnsTypes;
        }

        [Route("GetCurrency/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(AccCurrencyInfo))]
        [HttpGet]
        public IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<AccCurrencyInfo> lstChallanTrnsTypes = null;
            try
            {
                lstChallanTrnsTypes = objChallanService.GetCurrency(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanTrnsTypes;
        }


        [HttpPost]
        public IHttpActionResult GetItemMasterByGroupID(object[] data)
        {
            IEnumerable<vmChallan> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objItemMaster = objChallanService.GetItemMasterById(objcmnParam,  out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objItemMaster
            });
        }

        [HttpPost]
        public IHttpActionResult GetChallanDetailByChallanID(object[] data) 
        {
            IEnumerable<vmChallan> lstChallanDetail = null;
            
            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 challanID = Convert.ToInt64(data[1]);
                lstChallanDetail = objChallanService.GetChallanDetailByChallanID(objcmnParam, challanID, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstChallanDetail
            }); 
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateChallanMasterNdetails(object[] data) 
        {
            InvRChallanMaster itemMaster = JsonConvert.DeserializeObject<InvRChallanMaster>(data[0].ToString());
            List<InvRChallanDetail> itemDetails = JsonConvert.DeserializeObject<List<InvRChallanDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]); 
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemMaster.CHDate.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objChallanService.SaveUpdateChallanMasterNdetails(itemMaster, itemDetails, menuID);
                }
                else
                {
                    result = "";
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
        public IHttpActionResult GetChallanMasterList(object[] data)
        {
            IEnumerable<vmChallan> lstVmChallanMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                lstVmChallanMaster = objChallanService.GetChallanMasterList(objcmnParam, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                lstVmChallanMaster
            });
        }

    }
}

