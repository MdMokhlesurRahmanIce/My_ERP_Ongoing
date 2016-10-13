using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Purchase;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Purchase.api
{
    [RoutePrefix("Purchase/api/ComparativeStatement")]
    public class ComparativeStatementController : ApiController
    {

        private iPurchaseOrderMgt objPurchaseOrder = null;

        private iComparativeStatementMgt objComparative = null;
        private static string PONo { get; set; } 

        public ComparativeStatementController() 
        {
            objPurchaseOrder = new PurchaseOrderMgt();
            objComparative = new ComparativeStatementMgt();
        }


        [HttpPost]
        public IHttpActionResult GetCSMaster(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<vmComparativeStatement> objCSMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objCSMaster = objComparative.GetCSMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objCSMaster
            });

        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateCSMasterNdetails(object[] data)
        {
            PurchaseCSMaster itemMaster = JsonConvert.DeserializeObject<PurchaseCSMaster>(data[0].ToString());
            List<PurchaseCSDetail> itemDetails = JsonConvert.DeserializeObject<List<PurchaseCSDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemDetails.Count > 0 && menuID > 0)
                {
                    result = objComparative.SaveUpdateCSMasterNdetails(itemMaster, itemDetails, menuID);
                    PONo = result;
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

        [Route("GetQuotationInfoDetail/{QuotationID:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmQuotation))]
        [HttpGet]
        public IEnumerable<vmQuotation> GetQuotationInfoDetail(int QuotationID, int CompanyID)
        {
            IEnumerable<vmQuotation> lstQuotationDetail = null;
            try
            {
                lstQuotationDetail = objComparative.GetQuotationInfoDetail(QuotationID, CompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstQuotationDetail;
        }

        [HttpPost]
        public IEnumerable<vmComparativeStatement> GetQuotationRequisitionID(object[] data)
        {          
            IEnumerable<vmComparativeStatement> objQuotationDtls = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objQuotationDtls = objComparative.GetQuotationRequisitionID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objQuotationDtls;
        }

        [HttpPost]
        public IEnumerable<InvRequisitionMaster> GetSPR(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSPRNo = objComparative.GetSPR(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSPRNo;
        }

        [HttpPost]
        public IEnumerable<CmnCombo> GetOrderType(object[] data) 
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ComboType =  data[1].ToString();

            IEnumerable<CmnCombo> lstOrderType = null;
            try
            {
                lstOrderType = objPurchaseOrder.GetOrderType(objcmnParam, ComboType);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstOrderType;
        }

    
        [Route("GetParty/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCmnUser))]
        [HttpGet]
        public IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCmnUser> lstParty = null;
            try
            {
                lstParty = objPurchaseOrder.GetParty(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstParty;
        }

        [HttpPost]
        public IHttpActionResult GetPOMasterList(object[] data)
        {
            IEnumerable<vmChallan> lstPOMaster = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstPOMaster = objPurchaseOrder.GetPOMasterList(objcmnParam, out recordsTotal); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                lstPOMaster
            });
        }

    }
}
