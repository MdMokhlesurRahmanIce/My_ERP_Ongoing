using ABS.Models;
using ABS.Models.ViewModel.Accounting;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Accounting.Factories;
using ABS.Service.Accounting.Interfaces;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ABS.Web.Areas.Accounting.api
{
    [RoutePrefix("Accounting/api/BillOfMaterial")]
    public class BillOfMaterialController : ApiController
    {
        private iBillOfMaterialMgt objBillOfMaterial = null;
        private iSystemCommonDDL objSystemCommonDDL = null;

        public BillOfMaterialController()
        {
            objBillOfMaterial = new BillOfMaterialMgt();
            objSystemCommonDDL = new SystemCommonDDL();
        }

 

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemDetailByItemID(object[] data) 
        {
            IEnumerable<vmBOM> lstBOM = null; 


            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 itemID = Convert.ToInt64(data[1]);
                lstBOM = objBillOfMaterial.GetItemDetailByItemID(objcmnParam, itemID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstBOM
            });
        }
 
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDetailListByBOMID(object[] data) 
        {
            // IEnumerable<vmBOM> lstBOM = null; 

            IEnumerable<object> lstDetailInfoByBOMID = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                vmCmnParameters objcmnParam1 = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());

                Int64 bomID = Convert.ToInt64(data[2]);
                lstDetailInfoByBOMID = objBillOfMaterial.GetDetailListByBOMID(objcmnParam, objcmnParam1, bomID, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstDetailInfoByBOMID
            });
        }



        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemDetailFDying(object[] data) 
        {
            IEnumerable<vmBOM> lstDying = null;


            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int32 itemTypeID = Convert.ToInt32(data[1]);
                Int32 itemGroupID = Convert.ToInt32(data[2]);
                lstDying = objBillOfMaterial.GetItemDetailFDying(objcmnParam, itemTypeID, itemGroupID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstDying
            });
        }

         [HttpPost, BasicAuthorization]
         public IHttpActionResult GetItemDetailFSizing(object[] data)
         {
             IEnumerable<vmBOM> lstSizing = null;


             int recordsTotal = 0;
             try
             {
                 vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                 Int32 itemTypeID = Convert.ToInt32(data[1]);
                 Int32 itemGroupID = Convert.ToInt32(data[2]);
                 lstSizing = objBillOfMaterial.GetItemDetailFSizing(objcmnParam, itemTypeID, itemGroupID, out recordsTotal); 
             }
             catch (Exception e)
             {
                 e.ToString();
             }
             return Json(new
             {
                 recordsTotal,
                 lstSizing
             });
         }


    

         [HttpPost, BasicAuthorization]
         public IHttpActionResult GetFinishingByItemID(object[] data)
         {
             IEnumerable<vmBOM> lstFinishing = null; 

             int recordsTotal = 0;
             try
             {
                 vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                 Int64 itemID = Convert.ToInt32(data[1]);
                 lstFinishing = objBillOfMaterial.GetFinishingByItemID(objcmnParam, itemID, out recordsTotal);
             }
             catch (Exception e)
             {
                 e.ToString();
             }
             return Json(new
             {
                 recordsTotal,
                 lstFinishing
             });
         }
         
 

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBOMMasterList(object[] data) 
        {
            IEnumerable<vmBOM> lstBOMMaster = null;

            int recordsTotal = 0;
            try
            {

                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstBOMMaster = objBillOfMaterial.GetBOMMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstBOMMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveNUpdateBOM(object[] data) 
        {
            string result = "";
            try
            {
                PrdBOMMaster objPrdBOMMaster = JsonConvert.DeserializeObject<PrdBOMMaster>(data[0].ToString());

                int menuID = Convert.ToInt16(data[1]);
                List<PrdBOMDying> lstPrdBOMDying = JsonConvert.DeserializeObject<List<PrdBOMDying>>(data[2].ToString());
                List<PrdBOMSize> lstPrdBOMSize = JsonConvert.DeserializeObject<List<PrdBOMSize>>(data[3].ToString());

                if (ModelState.IsValid && objPrdBOMMaster != null && objPrdBOMMaster.BOMDate.ToString() != "" && menuID != 0)
                {
                    result = objBillOfMaterial.SaveNUpdateBOM(objPrdBOMMaster, lstPrdBOMDying, lstPrdBOMSize, menuID);
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

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage DeleteBOM(object[] data) 
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 bomID = Convert.ToInt64(data[1]);

                if (bomID != 0)
                {
                    result = objBillOfMaterial.DeleteBOM(objcmnParam, bomID);
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
