using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ABS.Models;
using ABS.Service.Sales.Interfaces;
using ABS.Service.Sales.Factories;
using Newtonsoft.Json;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Web.Attributes;


namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/Booking")]
    public class BookingController : ApiController
    {

        private iBookingMgt objPIService = null;

        public BookingController()
        {
            objPIService = new BookingMgt();
        }

        [Route("GetPIBuyer/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmCmnUser)), BasicAuthorization]
        public List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging)
        {
            List<vmCmnUser> objListPIBuyer = null;
            try
            {
                objListPIBuyer = objPIService.GetPIBuyer(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListPIBuyer;
        }

        [Route("GetBuyerReference/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(CmnUser)), BasicAuthorization]
        public List<CmnUser> GetBuyerReference(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnUser> objListPISalesPerson = null;
            try
            {
                objListPISalesPerson = objPIService.GetBuyerReference(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListPISalesPerson;
        }

        [Route("GetPISampleNo/{companyID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmItemGroup)), BasicAuthorization]
        public IEnumerable<vmItemGroup> GetPISampleNo(int companyID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmItemGroup> objListPISampleNo = null;
            try
            {
                objListPISampleNo = objPIService.GetPISampleNo(companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListPISampleNo;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<CmnCompany> GetPICompany(object LoginUserID)
        {
            int userID = Convert.ToInt16(LoginUserID);
            IEnumerable<CmnCompany> objListPICompany = null;
            try
            {
                objListPICompany = objPIService.GetPICompany(userID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListPICompany;
        }
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBookingMaster(object[] data)
        {
            IEnumerable<vmBookingMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmPIMaster = objPIService.GetBookingMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objVmPIMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemMasterByGroupID(object[] data)
        {
            IEnumerable<vmItem> objPIItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                string groupId = data[1].ToString();
                objPIItemMaster = objPIService.GetItemMasterById(objcmnParam, groupId, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objPIItemMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmBookingDetail> GetBookingDetail(object activePI)
        {
            IEnumerable<vmBookingDetail> objPIItemDetails = null;
            try
            {
                Int64 Id = (Int64)activePI;
                objPIItemDetails = objPIService.GetBookingDetail(Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIItemDetails;
        }
        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateBookingItemMasterNdetails(object[] data)
        {
            SalBookingMaster itemMaster = JsonConvert.DeserializeObject<SalBookingMaster>(data[0].ToString());
            List<SalBookingDetail> itemDetails = JsonConvert.DeserializeObject<List<SalBookingDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);
            SalBookingMaster obj = new SalBookingMaster();
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemMaster.EmployeeID.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objPIService.SaveUpdateBookingItemMasterNdetails(itemMaster, itemDetails, menuID);
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
        [Route("DeleteMasterDetail/{id:int}"), HttpDelete, BasicAuthorization]
        public HttpResponseMessage DeleteMasterDetail(int id)
        {
            int result = 0;
            try
            {
                result = objPIService.DeleteMasterDetail(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
