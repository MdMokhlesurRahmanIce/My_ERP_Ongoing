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
    [RoutePrefix("Inventory/api/StockInfo")]
    public class StockInfoController : ApiController
    {

        [HttpPost]
        public IHttpActionResult GetAllStockItems(object[] data)
        {
            iStockInfoMgt objStockService = new StockInfoMgt();
            int recordsTotal = 0;
            IEnumerable<vmStockMaster> objListItem = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListItem = objStockService.GetAllStockItems(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objListItem
            });

        }

    }
}
