using ABS.Models;
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

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/CustomCode")]
    public class CustomCodeController : ApiController
    {
        private iCmnCustomCodeMgt objService = null;

        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetAllCustomCode/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCmnCustomCode))]
        [HttpGet]
        public IHttpActionResult GetAllCustomCode(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            objService = new CmnCustomCodeMgt();
            List<vmCmnCustomCode> finalList = null; int recordsTotal = 0;
            objService = new CmnCustomCodeMgt();
            try
            {
                finalList = objService.GetAllCustomCode(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                CustomCodeList = finalList
            });
        }

        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetCustomCodeDetailsByID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DetailsID:int}")]
        [ResponseType(typeof(vmCmnCustomCodeDetails))]
        [HttpGet]
        public IEnumerable<CmnCustomCodeDetail> GetCustomCodeDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            objService = new CmnCustomCodeMgt();
            List<CmnCustomCodeDetail> detailsList = new List<CmnCustomCodeDetail>();
            try
            {
                detailsList = objService.GetCustomCodeDetailsByID(companyID, loggedUser, pageNumber, pageSize, IsPaging, DetailsID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return detailsList;
        }


        [HttpPost]
        public HttpResponseMessage SaveCustomCode(object[] data)
        {
            CmnCustomCode itemMaster = JsonConvert.DeserializeObject<CmnCustomCode>(data[0].ToString());
            List<CmnCustomCodeDetail> itemDetails = JsonConvert.DeserializeObject<List<CmnCustomCodeDetail>>(data[1].ToString());
            CmnCustomCode obj = new CmnCustomCode();
            string result = "";
            try
            {
                result = "1";
                if (ModelState.IsValid)
                {
                    using (CmnCustomCodeMgt objService = new CmnCustomCodeMgt())
                    {
                        result = (objService.SavCustomCode(itemMaster, itemDetails)).ToString();
                    }
                       
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [Route("DeleteMaster/{RecordID:int}")]
        [HttpPost]
        public HttpResponseMessage DeleteMaster(int RecordID)
        {
            CmnCustomCode itemMaster = new CmnCustomCode();
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    objService = new CmnCustomCodeMgt();
                    result = (objService.UpdateMasterStatus(RecordID));
                    result = 1;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
