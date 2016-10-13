using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Purchase;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Web.Attributes;
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
    [RoutePrefix("Purchase/api/PurchaseOrder")]
    public class PurchaseOrderController : ApiController
    {

        private iPurchaseOrderMgt objPurchaseOrder = null;
        private static string PONo { get; set; } 

        public PurchaseOrderController() 
        {
            objPurchaseOrder = new PurchaseOrderMgt();
        }

        [HttpPost]
        public IHttpActionResult GetTermCondition(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<vmTermsCondition> objTerms = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());          
            try
            {
                objTerms = objPurchaseOrder.GetTermCondition(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objTerms
            });

        }


        [HttpPost]
        public IHttpActionResult GetStatementNo(object[] data)
        {
            IEnumerable<PurchaseQuotationMaster> objStatementNo = null; 
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objStatementNo = objPurchaseOrder.GetStatementNo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objStatementNo
            });
        }

    
        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailByStatementNo(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 StatementID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailByStatementNo = null;
            try
            {
                lstItemDetailByStatementNo = objPurchaseOrder.GetItemDetailByStatementNo(objcmnParam, StatementID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByStatementNo;
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

        [HttpPost]
        public IEnumerable<CmnCombo> GetMoneyTrnsType(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ComboType = data[1].ToString();

            IEnumerable<CmnCombo> lstMoneyTrnsType = null;
            try
            {
                lstMoneyTrnsType = objPurchaseOrder.GetMoneyTrnsType(objcmnParam, ComboType);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstMoneyTrnsType;
        }


        //[HttpPost]
        //public IEnumerable<vmChallan> GetItemDetailBySPRID(object[] data)
        //{
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    Int64 SprID = Convert.ToInt64(data[1]);

        //    IEnumerable<vmChallan> lstItemDetailBySPRID = null;
        //    try
        //    {
        //        lstItemDetailBySPRID = objGRRService.GetItemDetailBySPRID(objcmnParam, SprID);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return lstItemDetailBySPRID;
        //}

        //[HttpPost]
        //public IEnumerable<vmChallan> GetItmDetailByItmCode(object[] data)
        //{
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    string ItemCode = data[1].ToString();

        //    IEnumerable<vmChallan> objItemDtls = null;
        //    try
        //    {
        //        objItemDtls = objGRRService.GetItmDetailByItmCode(objcmnParam, ItemCode);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return objItemDtls;
        //}

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

        ////[Route("GetPISalesPerson/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        ////[ResponseType(typeof(CmnUser))]
        ////[HttpGet]
        ////public List<CmnUser> GetPISalesPerson(int? pageNumber, int? pageSize, int? IsPaging)
        ////{
        ////    List<CmnUser> objListPISalesPerson = null;
        ////    try
        ////    {
        ////        objListPISalesPerson = objPIService.GetPISalesPerson(pageNumber, pageSize, IsPaging);
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        e.ToString();
        ////    }
        ////    return objListPISalesPerson;
        ////}

        //[Route("GetItemSampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(vmItemGroup))]
        //[HttpGet]
        //public IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<vmItemGroup> lstSampleNo = null;
        //    try
        //    {
        //        lstSampleNo = objGRRService.GetItemSampleNo(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return lstSampleNo;
        //}

        //[Route("GetChallanTrnsTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(CmnCombo))]
        //[HttpGet]
        //public IEnumerable<CmnCombo> GetChallanTrnsTypes(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<CmnCombo> lstChallanTrnsTypes = null;
        //    try
        //    {
        //        lstChallanTrnsTypes = objGRRService.GetChallanTrnsTypes(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return lstChallanTrnsTypes;
        //}

        //[Route("GetCurrency/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(AccCurrencyInfo))]
        //[HttpGet]
        //public IEnumerable<AccCurrencyInfo> GetCurrency(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    IEnumerable<AccCurrencyInfo> lstChallanTrnsTypes = null;
        //    try
        //    {
        //        lstChallanTrnsTypes = objGRRService.GetCurrency(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return lstChallanTrnsTypes;
        //}


        //[HttpPost]
        //public IHttpActionResult GetItemMasterByGroupID(object[] data)
        //{
        //    IEnumerable<vmChallan> objItemMaster = null;
        //    vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
        //    int recordsTotal = 0;
        //    try
        //    {
        //        objItemMaster = objGRRService.GetItemMasterById(objcmnParam, out recordsTotal);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return Json(new
        //    {
        //        recordsTotal,
        //        objItemMaster
        //    });
        //}

        [HttpPost]
        public IHttpActionResult GetPODetailByPOID(object[] data) 
        {
            IEnumerable<vmChallan> lstPODetail = null; 

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 poID = Convert.ToInt64(data[1]);
                lstPODetail = objPurchaseOrder.GetPODetailByPOID(objcmnParam, poID, out recordsTotal);  
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstPODetail
            });
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdatePOMasterNdetails(object[] data) 
        {
            PurchasePOMaster itemMaster = JsonConvert.DeserializeObject<PurchasePOMaster>(data[0].ToString());
            List<PurchasePODetail> itemDetails = JsonConvert.DeserializeObject<List<PurchasePODetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);
            ArrayList fileNames = JsonConvert.DeserializeObject<ArrayList>(data[3].ToString());
            List<vmTermsCondition> termslist = JsonConvert.DeserializeObject<List<vmTermsCondition>>(data[4].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemMaster.PODate.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objPurchaseOrder.SaveUpdatePOMasterNdetails(itemMaster, itemDetails, menuID, fileNames, termslist);
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

        //[HttpPost()]
        [HttpPost]
        public void UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC/");

            //sPath = @"D:/Upload/LC/";

            //var directory = @"D:/Upload/LC/";
            CmnDocumentPath objDocumentPath = objPurchaseOrder.GetUploadPath();

            //string documentPath = objDocumentPath.PhysicalPath.ToString();
            var directory = @objDocumentPath.PhysicalPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        //hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        string exttension = System.IO.Path.GetExtension(hpf.FileName);
                        int fileSerial = iCnt + 1;
                        hpf.SaveAs(directory + PONo + "_Doc_" + fileSerial + exttension);
                        iUploadedCnt = iUploadedCnt + 1;
                        hpf.InputStream.Dispose();
                    }
                }
            }
            PONo = "";

            // RETURN A MESSAGE (OPTIONAL).
            //if (iUploadedCnt > 0)
            //{
            //    return iUploadedCnt + " Files Uploaded Successfully";
            //}
            //else
            //{
            //    return "Upload Failed";
            //}
        }

        // GET: GetPIDetailsById/1
        [Route("GetFileDetailsById/{id:int}")]
        [ResponseType(typeof(CmnDocument))]
        [HttpGet]
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int id)
        {
            IEnumerable<vmCmnDocument> objFileDetail = null;
            try
            {
                objFileDetail = objPurchaseOrder.GetFileDetailsById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }
    }
}
