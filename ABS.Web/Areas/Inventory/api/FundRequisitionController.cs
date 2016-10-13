using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ABS.Models;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using System.Web.Http.Description;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Models.ViewModel.SystemCommon;
using Newtonsoft.Json;
using ABS.Models.ViewModel.Inventory;
using ABS.Service.AllServiceClasses;

namespace ABS.Web.Areas.Inventory.api
{
     [RoutePrefix("Inventory/api/FundRequisition")]
    public class FundRequisitionController : ApiController
    {
         iFundRequisitionMgt objObjectService = new FundRequisitionMgt();
        public FundRequisitionController()
        {
           
        }
         
         [Route("GetAllBank/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
         [ResponseType(typeof(CmnBank))]
         [HttpGet]
         public IEnumerable<CmnBank> GetAllBank(int? pageNumber, int? pageSize, int? IsPaging)
          {          
              IEnumerable<CmnBank> objBank = null;            
              try
              {               
                  objBank = objObjectService.GetAllBank(pageNumber, pageSize, IsPaging).ToList();
                
              }
              catch (Exception e)
              {
                  e.ToString();
              }
              return objBank;
              
          }

         [HttpPost]
         public HttpResponseMessage SaveFundRequisition(object[] data)
         {
             PurchaseFR purchaseFR = JsonConvert.DeserializeObject<PurchaseFR>(data[0].ToString());          
             int menuID = Convert.ToInt16(data[1]);
             string result = "";
             try
             {
                 if (ModelState.IsValid && purchaseFR != null &&  menuID != null)
                 {
                     result = objObjectService.SaveFundRequisition(purchaseFR, menuID);
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

    }
}