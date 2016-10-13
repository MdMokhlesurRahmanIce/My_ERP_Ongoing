using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Purchase.api
{
    [RoutePrefix("Purchase/api/Quotation")]
    public class QuotationController : ApiController 
    {

      //  private iQuotationMgt objQuotationService = null;
        
        public QuotationController()
        {
            //objQuotationService = new QuotationMgt(); 
        }

        QuotationMgt objQuotationService = new QuotationMgt();

        [Route("GetQuotationMasterById/{QuotationID:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmQuotation))]
        [HttpGet]
        public vmQuotation GetQuotationMasterById(int? QuotationId, int CompanyID)
        {
            vmQuotation Quotationlist = null;

            try
            {
                Quotationlist = objQuotationService.GetQuotationMasterById(QuotationId, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Quotationlist;
        }


        [Route("GetQuotationDetailById/{QuotationID:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmQuotation))]
        [HttpGet]
        public IEnumerable<vmQuotation> GetQuotationDetailById(int? QuotationID, int CompanyID)
        {
            IEnumerable<vmQuotation> Quotationlist = null;

            try
            {
                Quotationlist = objQuotationService.GetQuotationDetailById(QuotationID, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Quotationlist;
        }

        [HttpPost]
        public HttpResponseMessage GetDataBySuppplierID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string result = "";
            try
            {

                result = objQuotationService.GetDataBySuppplierID(objcmnParam);
             
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public IEnumerable<InvRequisitionMaster> GetSPR(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSPRNo = objQuotationService.GetSPR(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSPRNo;
        }


        [HttpPost]
        public IHttpActionResult GetQuotationMaster(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<vmQuotation> objQuotationMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());         
            try
            {
                objQuotationMaster = objQuotationService.GetQuotationMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objQuotationMaster
            });

        }

        [HttpPost]
        public HttpResponseMessage SaveQuotationMasterDetails(object[] data)
        {
            PurchaseQuotationMaster QuotationMaster = JsonConvert.DeserializeObject<PurchaseQuotationMaster>(data[0].ToString());
            List<vmQuotation> QuotationDetails = JsonConvert.DeserializeObject<List<vmQuotation>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);        
            string result = "";
            try
            {
                if (ModelState.IsValid && QuotationMaster != null && QuotationDetails.Count > 0 && menuID != null)
                {
                    result = objQuotationService.SaveQuotationMasterDetails(QuotationMaster, QuotationDetails, menuID);
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

        [Route("GetRequisitonDetailByRequisitionID/{RequisitionId:int}/{CompanyId:int}")]
        [ResponseType(typeof(vmRequisitionDetails))]
        [HttpGet]
        public IEnumerable<vmRequisitionDetails> GetRequisitonDetailByRequisitionID(int? RequisitionId, int CompanyId)
        {
            IEnumerable<vmRequisitionDetails> RequisitionItemList = null;

            try
            {
                RequisitionItemList = objQuotationService.GetRequisitonDetailByRequisitionID(RequisitionId, CompanyId);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return RequisitionItemList;
        }

        [HttpPost]
        public IHttpActionResult GetSPRNo(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objSPRNo = objQuotationService.GetSPRNo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objSPRNo
            });
        }

        [Route("GetAllUsers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{UserTypeID:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet]
        public List<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? UserTypeID, int? CompanyID)
        {
            List<CmnUser> users = null;

            try
            {
                users = objQuotationService.GetUsers(pageNumber, pageSize, IsPaging, UserTypeID, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return users;
        }


        [HttpPost]
        public IEnumerable<CmnCompany> GetCompany(object[] data) 
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            IEnumerable<CmnCompany> objCompany = null;
            try
            {
                objCompany = objQuotationService.GetCompany(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCompany;
        }


        [HttpPost]
        public IEnumerable<CmnOrganogram> GetDeptByCompanyID(object[] data) 
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int companyID = Convert.ToInt16(data[1]);

            IEnumerable<CmnOrganogram> lstCmnOrganogram = null;
            try
            {

                lstCmnOrganogram = objQuotationService.GetDeptByCompanyID(objcmnParam, companyID); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstCmnOrganogram;
        }


        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailBySPRID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 SprID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailBySPRID = null;
            try
            {

                lstItemDetailBySPRID = objQuotationService.GetItemDetailBySPRID(objcmnParam, SprID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailBySPRID;
        }

        [Route("GetParty/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")] 
        [ResponseType(typeof(vmCmnUser))]
        [HttpGet]
        public IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCmnUser> lstParty = null;
            try
            {
                lstParty = objQuotationService.GetParty(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstParty;
        }

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

        [Route("GetItemSampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmItemGroup))]
        [HttpGet]
        public IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmItemGroup> lstSampleNo = null;
            try
            {
                lstSampleNo = objQuotationService.GetItemSampleNo(pageNumber, pageSize, IsPaging);  
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSampleNo;
        }

        [Route("GetChallanTrnsTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging) 
        {
            IEnumerable<CmnCombo> lstChallanTrnsTypes = null;
            try
            {
                lstChallanTrnsTypes = objQuotationService.GetChallanTrnsTypes(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanTrnsTypes;
        }

        [Route("GetCurrency/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(AccCurrencyInfo))]
        [HttpGet]
        public IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<AccCurrencyInfo> lstChallanTrnsTypes = null;
            try
            {
                lstChallanTrnsTypes = objQuotationService.GetCurrency(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanTrnsTypes;
        }


        [HttpPost]
        public IHttpActionResult GetItemMasterByGroupID(object[] data)
        {
            IEnumerable<vmChallan> objItemMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                string groupId = data[1].ToString();
                objItemMaster = objQuotationService.GetItemMasterById(objcmnParam, groupId, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objItemMaster
            });
        }

        [HttpPost]
        public IHttpActionResult GetChallanDetailByChallanID(object[] data) 
        {
            IEnumerable<vmChallan> lstChallanDetail = null;
            
            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 challanID = Convert.ToInt64(data[1]);
                lstChallanDetail = objQuotationService.GetChallanDetailByChallanID(objcmnParam, challanID, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstChallanDetail
            }); 
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateChallanMasterNdetails(object[] data) 
        {
            InvRChallanMaster itemMaster = JsonConvert.DeserializeObject<InvRChallanMaster>(data[0].ToString());
            List<InvRChallanDetail> itemDetails = JsonConvert.DeserializeObject<List<InvRChallanDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]); 
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemMaster.TransactionTypeID.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objQuotationService.SaveUpdateChallanMasterNdetails(itemMaster, itemDetails, menuID);
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

        [HttpPost]
        public IHttpActionResult GetChallanMasterList(object[] data)
        {
            IEnumerable<vmChallan> lstVmChallanMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                lstVmChallanMaster = objQuotationService.GetChallanMasterList(objcmnParam, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                lstVmChallanMaster
            });
        }

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

