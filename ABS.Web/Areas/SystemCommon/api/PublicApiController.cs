using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
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
//using System.Web.Mvc;

namespace ABS.Web.SystemCommon.api
{

    [RoutePrefix("SystemCommon/api/PublicApi")]
    public class PublicApiController : ApiController  
    {
        private iPublicApiMgt objService = null;
        [HttpPost]
        public IHttpActionResult GetItemMaster(object[] data)
        {
            IEnumerable<vmItem> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objService = new PublicApiMgt();
                objItemMaster = objService.GetItemMaster(objcmnParam, out recordsTotal);
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
        public IHttpActionResult GetItemMasterDeveloped(object[] data)
        {
            IEnumerable<vmItem> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objService = new PublicApiMgt();
                objItemMaster = objService.GetItemMasterDeveloped(objcmnParam, out recordsTotal);
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
        public IHttpActionResult GetFinishedItemMaster(object[] data)
        {
            IEnumerable<vmItem> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objService = new PublicApiMgt();
                objItemMaster = objService.GetFinishedItemMaster(objcmnParam, out recordsTotal);
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
    }
}
