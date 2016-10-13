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
    [RoutePrefix("Sales/api/PI")]
    public class PIController : ApiController
    {

        private iPIMgt objPIService = null;

        public PIController()
        {
            objPIService = new PIMgt();
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

        [Route("GetPISalesPerson/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(CmnUser)), BasicAuthorization]
        public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnUser> objListPISalesPerson = null;
            try
            {
                objListPISalesPerson = objPIService.GetPISalesPerson(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListPISalesPerson;
        }

        [Route("GetPISampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), HttpGet, ResponseType(typeof(vmItemGroup)), BasicAuthorization]
        public IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmItemGroup> objListPISampleNo = null;
            try
            {
                objListPISampleNo = objPIService.GetPISampleNo(pageNumber, pageSize, IsPaging);
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

        [Route("GetPIShipment/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objListShipment = null;
            try
            {
                objListShipment = objPIService.GetPIShipment(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListShipment;
        }

        [Route("GetPIValidity/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objListValidity = null;
            try
            {
                objListValidity = objPIService.GetPIValidity(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListValidity;
        }

        [Route("GetPIStatus/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetPIStatus(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objPIStatusList = null;
            try
            {
                objPIStatusList = objPIService.GetPIStatus(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPIStatusList;
        }


        [Route("GetIncoterm/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(SalIncoterm)), HttpGet, BasicAuthorization]
        public IEnumerable<SalIncoterm> GetIncoterm(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SalIncoterm> objIncoterm = null;
            try
            {
                objIncoterm = objPIService.GetIncoterm(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objIncoterm;
        }

        [Route("GetBookingList/{buyerID:int}/{companyID:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), 
            ResponseType(typeof(SalBookingMaster)), HttpGet, BasicAuthorization]
        public IEnumerable<SalBookingMaster> GetBookingList(int buyerID, int companyID, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SalBookingMaster> objIncoterm = null;
            try
            {
                objIncoterm = objPIService.GetBookingList(buyerID, companyID, pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objIncoterm;
        }


        [Route("GetAcceptableQuantity/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetAcceptableQuantity(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objAcceptPerc = null;
            try
            {
                objAcceptPerc = objPIService.GetAcceptableQuantity(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objAcceptPerc;
        }
        [Route("GetPISight/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objListSight = null;
            try
            {
                objListSight = objPIService.GetPISight(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSight;
        }

        [Route("GetSalesItemConstructionType/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetSalesItemConstructionType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objItemConstructionType = null;
            try
            {
                objItemConstructionType = objPIService.GetSalesItemConstructionType(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemConstructionType;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPIMasterByPIActive(object[] data)
        {
            IEnumerable<vmPIMaster> objVmPIMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmPIMaster = objPIService.GetPIMasterByPIActive(objcmnParam, out recordsTotal);
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
        public HttpResponseMessage GetBankAdvisingListByCompanyID(object companyID)
        {
            IEnumerable<vmPIMaster> objCmnBank = null;
            try
            {
                int Id = Convert.ToInt16(companyID);
                objCmnBank = objPIService.GetBankAdvisingListByCompanyID(Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objCmnBank);
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetBranchListByBankID(object bankID)
        {
            IEnumerable<vmPIMaster> objCmnBankBranch = null;
            try
            {
                int Id = Convert.ToInt16(bankID);
                objCmnBankBranch = objPIService.GetBranchListByBankID(Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objCmnBankBranch);
        }

        [HttpPost, BasicAuthorization]       
        public IHttpActionResult GetBookingDetailByID(object[] data)
        {
            IEnumerable<vmItem> objPIItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                string groupId = data[1].ToString();
                objPIItemMaster = objPIService.GetBookingDetailByID(objcmnParam, groupId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objPIItemMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(object activePI)
        {
            IEnumerable<vmPIDetail> objPIItemDetails = null;
            try
            {
                Int64 Id = (Int64)activePI;
                objPIItemDetails = objPIService.GetPIDetailsListByActivePI(Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objPIItemDetails;
        }
        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdatePIItemMasterNdetails(object[] data)
        {
            SalPIMaster itemMaster = JsonConvert.DeserializeObject<SalPIMaster>(data[0].ToString());
            List<SalPIDetail> itemDetails = JsonConvert.DeserializeObject<List<SalPIDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);
            SalPIMaster obj = new SalPIMaster();
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemMaster.EmployeeID.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objPIService.SaveUpdatePIItemMasterNdetails(itemMaster, itemDetails, menuID);
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
        [Route("DeleteSalPIMasterNSalPIDetail/{id:int}"), HttpDelete, BasicAuthorization]
        public HttpResponseMessage DeleteSalPIMasterNSalPIDetail(int id)
        {
            int result = 0;
            try
            {
                result = objPIService.DeleteSalPIMasterNSalPIDetail(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetPIDailyData/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmPIDetail))]
        [HttpGet]
        public IEnumerable<vmPIDetail> GetPIDailyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmPIDetail> objPIInfo = null;
            try
            {
                objPIInfo = objPIService.GetPIDailyData(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPIInfo;
        }

        [Route("GetPIMonthlyData/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmPIDetail))]
        [HttpGet]
        public IEnumerable<vmPIDetail> GetPIMonthlyData(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmPIDetail> objPIInfo = null;
            try
            {
                objPIInfo = objPIService.GetPIMonthlyData(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPIInfo;
        }
    }
}
