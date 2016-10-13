using ABS.Models;
using ABS.Service.Commercial.Factories;
using ABS.Service.Commercial.Interfaces;
using Newtonsoft.Json;
using ABS.Models.ViewModel.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Web.Attributes;
using ABS.Models.ViewModel.Commercial;

namespace ABS.Web.Areas.Commercial.api
{
    [RoutePrefix("Commercial/api/DC")]
    public class DCController : ApiController
    {
        private iDCMgt objDCService = null;

        public DCController()
        {
            objDCService = new DCMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllFDONo(object[] data)
        {
            IEnumerable<SalFDOMaster> objListFDONo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objListFDONo = objDCService.GetAllFDONo(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objListFDONo
            });
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateDC(object[] data)
        {
            SalDCMaster itemMaster = JsonConvert.DeserializeObject<SalDCMaster>(data[0].ToString());
            List<SalDCDetail> itemDetails = JsonConvert.DeserializeObject<List<SalDCDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objDCService.SaveUpdateDC(itemMaster, itemDetails, objcmnParam);
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
        public IHttpActionResult GetDCMaster(object[] data)
        {
            IEnumerable<vmSalDCMasterDetail> objSalDCMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objSalDCMaster = objDCService.GetDCMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objSalDCMaster
            });
            //return objvmSalLCDetail;
        }

        [Route("GetFDOQty/{id:int}"), ResponseType(typeof(vmSalFDODetail)), HttpGet, BasicAuthorization]
        public IEnumerable<vmSalFDODetail> GetFDOQty(int id)
        {
            IEnumerable<vmSalFDODetail> objFDOQty = null;
            try
            {
                objFDOQty = objDCService.GetFDOQty(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFDOQty;
        }

        [Route("GetBank/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnBank)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnBank> objListBank = null;
            try
            {
                objListBank = objDCService.GetBank(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBank;
        }

        [Route("GetDCDailyData/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmSalDCMasterDetail))]
        [HttpGet]
        public IEnumerable<vmSalDCMasterDetail> GetDCDailyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmSalDCMasterDetail> objDCInfo = null;
            try
            {
                objDCInfo = objDCService.GetDCDailyData(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDCInfo;
        }

        [Route("GetDCMonthlyData/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmSalDCMasterDetail))]
        [HttpGet]
        public IEnumerable<vmSalDCMasterDetail> GetDCMonthlyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmSalDCMasterDetail> objDCInfo = null;
            try
            {
                objDCInfo = objDCService.GetDCMonthlyData(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDCInfo;
        }

    }
}
