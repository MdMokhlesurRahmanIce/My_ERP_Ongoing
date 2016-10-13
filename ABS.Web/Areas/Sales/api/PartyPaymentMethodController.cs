using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Service.Sales.Factories;
using ABS.Service.Sales.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;


namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/PartyPaymentMethod")]
    public class PartyPaymentMethodController : ApiController
    {
        private PartyPaymentMethodMgt objDOService = null;

        public PartyPaymentMethodController()
        {
            objDOService = new PartyPaymentMethodMgt();
        }

        #region Party Payment Method

        // GET: GetLCByBuyerId/1
        [Route("GetLCByBuyerId/{id:int}/{ComId:int}")]
        [ResponseType(typeof(SalLCMaster))]
        [HttpGet]
        public IEnumerable<SalLCMaster> GetLCByBuyerId(int? id, int? ComId)
        {
            IEnumerable<SalLCMaster> objLCBYBId = null;
            try
            {
                objLCBYBId = objDOService.GetLCByBuyerId(id, ComId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLCBYBId;
        }

        // GET: GetDocType/1/10/0
        [Route("GetDocTypeOrPaymentMode/{TabId:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public IEnumerable<CmnCombo> GetDocTypeOrPaymentMode(int? TabId, int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objDoc = null;
            try
            {
                objDoc = objDOService.GetDocTypeOrPaymentMode(TabId, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDoc.ToList();
        }

        // GET: GetBankCharge/1/10/0
        [Route("GetBankCharge/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public IEnumerable<CmnCombo> GetBankCharge(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objCharge = null;
            try
            {
                objCharge = objDOService.GetBankCharge(pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objCharge.ToList();
        }

        // GET: GetBankByLCID/1/10/0
        [Route("GetBankByLCID/{id:int}")]
        [ResponseType(typeof(CmnBank))]
        [HttpGet]
        public IEnumerable<CmnBank> GetBankByLCID(int? id)
        {
            IEnumerable<CmnBank> objListBank = null;
            try
            {
                objListBank = objDOService.GetBankByLCID(id).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBank.ToList();
        }

        // GET: GetBillByBuyerID/1/1
        [Route("GetBillByBuyerID/{id:int}/{TabId:int}")]
        [ResponseType(typeof(SalPPBillingMaster))]
        [HttpGet]
        public IEnumerable<SalPPBillingMaster> GetBillByBuyerID(int? id, int? TabId)
        {
            IEnumerable<SalPPBillingMaster> objListBill = null;
            try
            {
                objListBill = objDOService.GetBillByBuyerID(id, TabId).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBill.ToList();
        }

        // GET: GetBillNo/1/10/0
        [Route("GetBillNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(SalPPBillingMaster))]
        [HttpGet]
        public IEnumerable<SalPPBillingMaster> GetBillNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<SalPPBillingMaster> objBill_Doc = null;
            try
            {
                objBill_Doc = objDOService.GetBillNo(pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objBill_Doc.ToList();
        }

        // GET: GetDocValByBillID/1
        [Route("GetDocValByBillID/{id:int}")]
        [ResponseType(typeof(SalPPBillingMaster))]
        [HttpGet]
        public SalPPBillingMaster GetDocValByBillID(int? id)
        {
            SalPPBillingMaster objDocVal = null;
            try
            {
                objDocVal = objDOService.GetDocValByBillID(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objDocVal;
        }

        //POST SaveUpdatePartyPayment
        [ResponseType(typeof(vmSalPPBillingMaster))]
        [HttpPost]
        public HttpResponseMessage SaveUpdatePartyPayment(vmSalPPBillingMaster model)
        {
            string result = string.Empty;
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objDOService.SaveUpdatePartyPayment(model);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = string.Empty;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //POST SaveUpdateAdjustment
        [ResponseType(typeof(vmSalPPBAMasterDetail))]
        [HttpPost]
        public HttpResponseMessage SaveUpdateAdjustment(List<vmSalPPBAMasterDetail> model)
        {
            string result = string.Empty;
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objDOService.SaveUpdateAdjustment(model);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = string.Empty;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion Party Payment Method
    }
}