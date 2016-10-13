using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
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
    [RoutePrefix("Inventory/api/StockEntry")]
    public class StockEntryController : ApiController
    {
        iStockEntryMgt objStockService = null;

        iSystemCommonDDL objCmnItemGroupMgtEF = null;
        iCmnRawMaterial objCmnRawMaterial = null;
        public StockEntryController()
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objCmnRawMaterial = new CmnRawMaterialMgt();
            objStockService = new StockEntryMgt();

        }

        [Route("GetItemList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public IEnumerable<CmnItemMaster> GetFinishItemDescription(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemMaster> objListGrrItem = null;
            try
            {
                objListGrrItem = objStockService.GetFinishItemDescription(pageNumber, pageSize, IsPaging);               
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListGrrItem;
        }

        //GetLotByItemId
        [Route("GetLotNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnLot> objListLot = null;
            try
            {
                objListLot = objStockService.GetLotNo(pageNumber, pageSize, IsPaging);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListLot.ToList();
        }

        //GetLotByItemId
        [Route("GetGrade/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemGrade> objListLot = null;
            try
            {
                objListLot = objStockService.GetGrade(pageNumber, pageSize, IsPaging);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListLot.ToList();
        }

        [ResponseType(typeof(InvStockMaster))]
        [HttpPost]
        public HttpResponseMessage SaveStockEntry(InvStockMaster model)
        {
            int result = 0;
            try
            {
                result = objStockService.SaveStockEntry(model);
            }
            catch (Exception e)
            {
                e.ToString();
                
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }       

        [HttpPost]
        public IHttpActionResult GetItemList(object[] data)
        {
            IEnumerable<vmStockMaster> objStockMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objStockMaster = objStockService.GetItemList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objStockMaster
            });
        }              

     
    }
}
