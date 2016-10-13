using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ABS.Models;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.Inventory.Factories;
using Newtonsoft.Json;
using ABS.Models.ViewModel.Sales;
using System.Web.Http.Description;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using System.Collections;
using System.IO;
using ABS.Web.Attributes;


namespace ABS.Web.Areas.Inventory.api
{

    [RoutePrefix("Inventory/api/OpeningStock")]
    public class OpeningStockController : ApiController 
    {

        private iOpeningStockMgt objOpeningStockService = null;  
        private static string GRRNo { get; set; }

        private static int TransactionTypeID { get; set; }

        public OpeningStockController()
        {
            objOpeningStockService = new OpeningStockMgt();
        }


        [HttpPost]
        public IHttpActionResult GetSPRNo(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objSPRNo = objOpeningStockService.GetSPRNo(objcmnParam, out recordsTotal);
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

        [HttpPost]
        public IHttpActionResult GetPONo(object[] data)
        {
            IEnumerable<PurchasePOMaster> objPONo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objPONo = objOpeningStockService.GetPONo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objPONo
            });
        }


        [HttpPost]
        public IHttpActionResult GetLocation(object[] data)
        {
            IEnumerable<CmnAddressCountry> objLocation = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objLocation = objOpeningStockService.GetLocation(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objLocation
            });
        }

      
        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailBySPRID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 SprID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailBySPRID = null;
            try
            {
                lstItemDetailBySPRID = objOpeningStockService.GetItemDetailBySPRID(objcmnParam, SprID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailBySPRID;
        }

        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailByPOID(object[] data) 
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 POID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailByPOID = null;
            try
            {
                lstItemDetailByPOID = objOpeningStockService.GetItemDetailByPOID(objcmnParam, POID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByPOID;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmChallan> GetItmDetailByItmCode(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ItemCode = data[1].ToString();

            IEnumerable<vmChallan> objItemDtls = null;
            try
            {
                objItemDtls = objOpeningStockService.GetItmDetailByItmCode(objcmnParam, ItemCode);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemDtls;
        }

        [Route("GetParty/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmCmnUser))]
        [HttpGet]
        public IEnumerable<vmCmnUser> GetParty(int pageNumber, int pageSize, int IsPaging)
        {
            IEnumerable<vmCmnUser> lstParty = null;
            try
            {
                lstParty = objOpeningStockService.GetParty(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstParty;
        }


        [Route("GetItemSampleNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vmItemGroup))]
        [HttpGet]
        public IEnumerable<vmItemGroup> GetItemSampleNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<vmItemGroup> lstSampleNo = null;
            try
            {
                lstSampleNo = objOpeningStockService.GetItemSampleNo(pageNumber, pageSize, IsPaging);
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
                lstChallanTrnsTypes = objOpeningStockService.GetChallanTrnsTypes(pageNumber, pageSize, IsPaging);
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
                lstChallanTrnsTypes = objOpeningStockService.GetCurrency(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanTrnsTypes;
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemMasterByGroupID(object[] data)
        {
            IEnumerable<vmChallan> objItemMaster = null;
            
            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                objItemMaster = objOpeningStockService.GetItemMasterById(objcmnParam, out recordsTotal);
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
        public IHttpActionResult GetGrrDetailByGrrID(object[] data)
        {
            IEnumerable<vmChallan> lstChallanDetail = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 grrID = Convert.ToInt64(data[1]);
                lstChallanDetail = objOpeningStockService.GetGrrDetailByGrrID(objcmnParam, grrID, out recordsTotal);
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

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateMrrMasterNdetails(object[] data)
        {
            InvMrrMaster mrrMaster = JsonConvert.DeserializeObject<InvMrrMaster>(data[0].ToString());
            List<InvMrrDetail> mrrDetails = JsonConvert.DeserializeObject<List<InvMrrDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);

            string result = "";
            try
            {
                if (ModelState.IsValid && mrrMaster != null && mrrMaster.MrrDate.ToString() != "" && mrrDetails.Count > 0 && menuID != 0)
                {
                    result = objOpeningStockService.SaveUpdateMrrMasterNdetails(mrrMaster, mrrDetails, menuID); 
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
        public HttpResponseMessage SaveLot(object[] data)
        {
            CmnLot objCmnLot = JsonConvert.DeserializeObject<CmnLot>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && objCmnLot != null && objCmnLot.LotNo.ToString() != "")
                {
                    result = objOpeningStockService.SaveLot(objCmnLot);
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
        public HttpResponseMessage SaveBatch(object[] data)
        {
            CmnBatch objCmnBatch = JsonConvert.DeserializeObject<CmnBatch>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && objCmnBatch != null && objCmnBatch.BatchNo.ToString() != "")
                {
                    result = objOpeningStockService.SaveBatch(objCmnBatch);
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
        public IHttpActionResult GetMrrMasterList(object[] data) 
        {
            IEnumerable<vmChallan> lstVmChallanMaster = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
             
                lstVmChallanMaster = objOpeningStockService.GetMrrMasterList(objcmnParam, out recordsTotal);
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

        [HttpPost()]
        public void UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC/");

            //sPath = @"D:/Upload/LC/";

            //var directory = @"D:/Upload/LC/";
            CmnDocumentPath objDocumentPath = objOpeningStockService.GetUploadPath(TransactionTypeID);

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
                        hpf.SaveAs(directory + GRRNo + "_Doc_" + fileSerial + exttension);
                        iUploadedCnt = iUploadedCnt + 1;
                        hpf.InputStream.Dispose();
                    }
                }
            }
            GRRNo = "";

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
        [Route("GetFileDetailsById/{id:int}/{TransTypeID:int}")]
        [ResponseType(typeof(CmnDocument))]
        [HttpGet]
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int id, int TransTypeID)
        {
            IEnumerable<vmCmnDocument> objFileDetail = null;
            try
            {
                objFileDetail = objOpeningStockService.GetFileDetailsById(id, TransTypeID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }
    }
}
