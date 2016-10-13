using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.Sales;
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
    [RoutePrefix("Production/api/QA")]
    public class QAController : ApiController
    {
        private iProductionDDLMgt objCmnDDLService = null;
        private iQAMgt _iQmgt = null;

        public QAController()
        {
            _iQmgt = new QAMgt();
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
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllGrade(object[] data)
        {
            IEnumerable<vmGrade> ListGrade = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListGrade = objCmnDDLService.GetAllGrade(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListGrade
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetInspactionDetailsByIDAndDates(object[] data)
        {
            List<vmFinishingInspactionDetail> FinishingInspactionDetails = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                FinishingInspactionDetails = _iQmgt.GetInspactionDetailsByIDAndDates(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, FinishingInspactionDetails);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetQAMasterList(object[] data)
        {
            List<vmFinishingInspactionDetail> ListQAMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListQAMaster = _iQmgt.GetQAMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListQAMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateQAMasterDetail(object[] data)
        {
            vmFinishingInspactionDetail Master = JsonConvert.DeserializeObject<vmFinishingInspactionDetail>(data[0].ToString());
            List<vmFinishingInspactionDetail> Detail = JsonConvert.DeserializeObject<List<vmFinishingInspactionDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = string.Empty;
            try
            {
                result = _iQmgt.SaveUpdateQAMasterDetail(Master, Detail, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateQAMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = _iQmgt.DeleteUpdateQAMasterDetail(objcmnParam);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Json(new
            {
                result
            });
        }
    }
}
