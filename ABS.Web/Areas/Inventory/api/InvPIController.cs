using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/InvPI")]
    public class InvPIController : ApiController
    {

        private iInvPIMgt objInvPIService = null;

        public InvPIController()
        {
            objInvPIService = new InvPIMgt();
        }

        [HttpPost]
        public IHttpActionResult LoadSPRNO(object[] data)
        {
            IEnumerable<InvRequisitionMaster> lstSprono = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                lstSprono = objInvPIService.LoadSPRNO(objcmnParam, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                lstSprono
            });
        }

        //[HttpPost]
        //public HttpResponseMessage GetBankAdvisingList(object companyID)
        //{
        //    IEnumerable<CmnBankAdvising> objCmnBank = null;
        //    try
        //    {
        //          int Id = Convert.ToInt16(companyID);
        //          objCmnBank = objInvPIService.GetBankAdvisingList(Id); 
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, objCmnBank);
        //}


        //[Route("GetPIBuyer/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(vmCmnUser))]
        //[HttpGet]
        //public List<vmCmnUser> GetPIBuyer(int pageNumber, int pageSize, int IsPaging)
        //{
        //    List<vmCmnUser> objListPIBuyer = null;
        //    try
        //    {
        //        objListPIBuyer = objPIService.GetPIBuyer(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListPIBuyer;
        //}

        //[Route("GetPISalesPerson/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnUser))]
        //[HttpGet]
        //public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<CmnUser> objListPISalesPerson = null;
        //    try
        //    {
        //        objListPISalesPerson = objPIService.GetPISalesPerson(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListPISalesPerson;
        //}

        //[Route("GetPISampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(vmItemGroup))]
        //[HttpGet]
        //public IEnumerable<vmItemGroup> GetPISampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<vmItemGroup> objListPISampleNo = null;
        //    try
        //    {
        //        objListPISampleNo = objPIService.GetPISampleNo(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListPISampleNo;
        //}

        //[HttpPost]
        //public IEnumerable<CmnCompany> GetPICompany(object LoginUserID)
        //{
        //    int userID = Convert.ToInt16(LoginUserID);
        //    IEnumerable<CmnCompany> objListPICompany = null;
        //    try
        //    {
        //        objListPICompany = objPIService.GetPICompany(userID);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListPICompany;
        //}
        //[Route("GetPIShipment/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCombo))]
        //[HttpGet]
        //public IEnumerable<CmnCombo> GetPIShipment(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objListShipment = null;
        //    try
        //    {
        //        objListShipment = objPIService.GetPIShipment(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListShipment;
        //}

        //[Route("GetPIValidity/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCombo))]
        //[HttpGet]
        //public IEnumerable<CmnCombo> GetPIValidity(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objListValidity = null;
        //    try
        //    {
        //        objListValidity = objPIService.GetPIValidity(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListValidity;
        //}
        //[Route("GetPISight/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCombo))]
        //[HttpGet]
        //public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> objListSight = null;
        //    try
        //    {
        //        objListSight = objPIService.GetPISight(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objListSight;
        //}

      

        //[HttpPost]
        //public IHttpActionResult GetItemMasterByGroupID(object[] data)
        //{
        //    IEnumerable<vmItem> objPIItemMaster = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    int recordsTotal = 0;
        //    try
        //    {
        //        string groupId = data[1].ToString();
        //        objPIItemMaster = objPIService.GetItemMasterById(objcmnParam, groupId, out recordsTotal);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        recordsTotal,
        //        objPIItemMaster
        //    });
        //}

        ////[HttpPost]
        ////public HttpResponseMessage GetItemMasterByUniqueCode(object uniqueCode)
        ////{
        ////    IEnumerable<vmItem> objPIItemMaster = null;
        ////    try
        ////    {
        ////        string Id = uniqueCode.ToString();
        ////        objPIItemMaster = objPIService.GetItemMasterById(Id);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return Request.CreateResponse(HttpStatusCode.OK, objPIItemMaster);
        ////}

        //[HttpPost]
        //public HttpResponseMessage GetBankAdvisingListByCompanyID(object companyID)
        //{
        //    IEnumerable<vmPIMaster> objCmnBank = null;
        //    try
        //    {
        //        int Id = Convert.ToInt16(companyID);
        //        objCmnBank = objPIService.GetBankAdvisingListByCompanyID(Id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, objCmnBank);
        //}

        //[HttpPost]
        //public HttpResponseMessage GetBranchListByBankID(object bankID)
        //{
        //    IEnumerable<vmPIMaster> objCmnBankBranch = null;
        //    try
        //    {
        //        int Id = Convert.ToInt16(bankID);
        //        objCmnBankBranch = objPIService.GetBranchListByBankID(Id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, objCmnBankBranch);
        //}

        //[HttpPost]
        //public IEnumerable<vmPIDetail> GetPIDetailsListByActivePI(object activePI)
        //{
        //    IEnumerable<vmPIDetail> objPIItemDetails = null;
        //    try
        //    {
        //        Int64 Id = (Int64)activePI;
        //        objPIItemDetails = objPIService.GetPIDetailsListByActivePI(Id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }

        //    return objPIItemDetails;
        //}
        //[HttpPost]
        //public HttpResponseMessage SaveUpdatePIItemMasterNdetails(object[] data)
        //{
        //    SalPIMaster itemMaster = JsonConvert.DeserializeObject<SalPIMaster>(data[0].ToString());
        //    List<SalPIDetail> itemDetails = JsonConvert.DeserializeObject<List<SalPIDetail>>(data[1].ToString());
        //    int menuID = Convert.ToInt16(data[2]);
        //    SalPIMaster obj = new SalPIMaster();
        //    string result = "";
        //    try
        //    {
        //        if (ModelState.IsValid && itemMaster != null && itemMaster.EmployeeID.ToString() != "" && itemDetails.Count > 0 && menuID != null)
        //        {
        //            result = objPIService.SaveUpdatePIItemMasterNdetails(itemMaster, itemDetails, menuID);
        //        }
        //        else
        //        {
        //            result = "";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = "";
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
        //[Route("DeleteSalPIMasterNSalPIDetail/{id:int}")]
        //[HttpDelete]
        //public HttpResponseMessage DeleteSalPIMasterNSalPIDetail(int id)
        //{
        //    int result = 0;
        //    try
        //    {
        //        result = objPIService.DeleteSalPIMasterNSalPIDetail(id);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = -0;
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
    }
}
