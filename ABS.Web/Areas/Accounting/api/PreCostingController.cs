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
    [RoutePrefix("Accounting/api/PreCosting")]
    public class PreCostingController : ApiController 
    {
        private iPreCostingMgt objPreCosting = null; 
        private iSystemCommonDDL objSystemCommonDDL = null;

        public PreCostingController()
        {
            objPreCosting = new PreCostingMgt();
            objSystemCommonDDL = new SystemCommonDDL();
        } 

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBomNArticleNo(object[] data)
        {
            List<vmBOM> lstBomNArticle = null;

            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstBomNArticle = objPreCosting.GetBomNArticleNo(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                lstBomNArticle
            });
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
                lstBOM = objPreCosting.GetItemDetailByItemID(objcmnParam, itemID, out recordsTotal);
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
        public IHttpActionResult GetDetailListByPrCostID(object[] data)  
        {
            // IEnumerable<vmBOM> lstBOM = null; 

            IEnumerable<object> lstDetailInfoByCostID = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString()); 

                Int64 costingID = Convert.ToInt64(data[1]);
                lstDetailInfoByCostID = objPreCosting.GetDetailListByPrCostID(objcmnParam, costingID, out recordsTotal);  
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstDetailInfoByCostID
            });
        }



        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDyingByBomID(object[] data) 
        {
            IEnumerable<vmBOM> lstDying = null;  
            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 bomID = Convert.ToInt32(data[1]);
                lstDying = objPreCosting.GetDyingByBomID(objcmnParam, bomID, out recordsTotal); 
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
        public IHttpActionResult GetSizingByBomID(object[] data)
         {
             IEnumerable<vmBOM> lstSizing = null; 
             int recordsTotal = 0;
             try
             {
                 vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                 Int64 bomID = Convert.ToInt32(data[1]);
                 lstSizing = objPreCosting.GetSizingByBomID(objcmnParam, bomID, out recordsTotal); 
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
                 lstFinishing = objPreCosting.GetFinishingByItemID(objcmnParam, itemID, out recordsTotal);
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
         public IHttpActionResult GetYarnByItemID(object[] data)
         {
             IEnumerable<vmBOM> lstYarn = null;

             int recordsTotal = 0;
             try
             {
                 vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                 Int64 itemID = Convert.ToInt32(data[1]);
                 lstYarn = objPreCosting.GetYarnByItemID(objcmnParam, itemID, out recordsTotal); 
             }
             catch (Exception e)
             {
                 e.ToString();
             }
             return Json(new
             {
                 recordsTotal,
                 lstYarn
             });
         }
         

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCostMasterList(object[] data)  
        {
            IEnumerable<vmBOM> lstCostMaster = null;

            int recordsTotal = 0;
            try
            {

                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstCostMaster = objPreCosting.GetCostMasterList(objcmnParam, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstCostMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveNUpdatePreCosting(object[] data)  
        {
            string result = "";
            try
            {
                PrdPreCostingMaster objMaster = JsonConvert.DeserializeObject<PrdPreCostingMaster>(data[0].ToString());

                int menuID = Convert.ToInt16(data[1]);

                List<PrdPreCostingDying> lstDying = JsonConvert.DeserializeObject<List<PrdPreCostingDying>>(data[2].ToString());
                List<PrdPreCostingSize> lstSize = JsonConvert.DeserializeObject<List<PrdPreCostingSize>>(data[3].ToString());
                List<PrdPreCostingFinishing> lstFinish = JsonConvert.DeserializeObject<List<PrdPreCostingFinishing>>(data[4].ToString());
                List<PrdPreCostingYarn> lstYarn = JsonConvert.DeserializeObject<List<PrdPreCostingYarn>>(data[5].ToString());
                List<PrdPreCostingDetail> lstDetail = JsonConvert.DeserializeObject<List<PrdPreCostingDetail>>(data[6].ToString());

                if (ModelState.IsValid && objMaster != null && objMaster.CostingDate.ToString() != "" && menuID != 0)
                {
                    result = objPreCosting.SaveNUpdatePreCosting(objMaster, lstDying, lstSize, lstFinish, lstYarn, lstDetail, menuID);
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
        public HttpResponseMessage DeletePreCosting(object[] data)  
        {
            string result = "";
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 costingID = Convert.ToInt64(data[1]);

                if (costingID != 0)
                {
                    result = objPreCosting.DeletePreCosting(objcmnParam, costingID);
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
