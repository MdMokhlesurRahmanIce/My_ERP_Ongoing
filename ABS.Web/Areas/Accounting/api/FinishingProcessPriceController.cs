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
    [RoutePrefix("Accounting/api/FinishingProcessPrice")]
    public class FinishingProcessPriceController : ApiController
    {
        private iFinishingProcessPriceMgt objFPR = null;
        private iSystemCommonDDL objSystemCommonDDL = null;

        public FinishingProcessPriceController()
        {
            objFPR = new FinishingProcessPriceMgt();
            objSystemCommonDDL = new SystemCommonDDL();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFinishingProcess(object[] data)
        {
            List<PrdFinishingProcess> lstFinishingProcess = null;

            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstFinishingProcess = objFPR.GetFinishingProcess(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                lstFinishingProcess
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFinPricChngeGrdByFProcessID(object[] data)
        {
            IEnumerable<vmFinishingProcessPriceSetup> lstFinishingPriceProcess = null;


            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int32 finishProcessID = Convert.ToInt32(data[1]);
                lstFinishingPriceProcess = objFPR.GetFinPricChngeGrdByFProcessID(objcmnParam, finishProcessID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstFinishingPriceProcess
            });
        }


        [HttpPost]
        public IHttpActionResult GetFPPMasterList(object[] data)
        {
            IEnumerable<vmFinishingProcessPriceSetup> lstFppMaster = null;

            int recordsTotal = 0;
            try
            {

                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstFppMaster = objFPR.GetFPPMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstFppMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveFinishProPricSetup(object[] data)
        {
            string result = "";
            try
            {
                prdFinishingProcessPriceSetup objPrdFinishingProcessPriceSetup = JsonConvert.DeserializeObject<prdFinishingProcessPriceSetup>(data[0].ToString());

                int menuID = Convert.ToInt16(data[1]);

                if (ModelState.IsValid && objPrdFinishingProcessPriceSetup != null && objPrdFinishingProcessPriceSetup.FinishingProcessID.ToString() != "" && menuID != 0)
                {
                    result = objFPR.SaveFinishProPricSetup(objPrdFinishingProcessPriceSetup, menuID);
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
