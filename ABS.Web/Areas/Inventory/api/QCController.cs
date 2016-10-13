using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
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

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/QC")]
    public class QCController : ApiController
    {
        private iQCMgt objQCService = null;
        private iSystemCommonDDL objSystemCommonDDL = null;
        private static string QCNo { get; set; }
        private static int TransactionTypeID { get; set; }

        public QCController()
        {
            objQCService = new QCMgt();
            objSystemCommonDDL = new SystemCommonDDL();
        }

        [Route("GetSuppliers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmBuyer))]
        [HttpGet]
        public List<vmBuyer> GetSuppliers(int pageNumber, int pageSize, int IsPaging, int? CompanyID)
        {
            List<vmBuyer> objListSuppliers = null;
            try
            {
                objListSuppliers = objSystemCommonDDL.GetSuppliers(pageNumber, pageSize, IsPaging, CompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSuppliers;
        }


        [HttpPost]
        public IEnumerable<InvRequisitionMaster> GetSPR(object[] data)
        {
            IEnumerable<InvRequisitionMaster> objSPRNo = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSPRNo = objQCService.GetSPR(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objSPRNo;
        }

        //// GET: CompanyonDemand
        //[Route("GetUser/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        ////[ResponseType(typeof(CmnCompany))]
        [HttpPost]
        public List<vmUser> GetUser(object[] data)
        {
            List<vmUser> obj = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                obj = objSystemCommonDDL.GetUserForDropDownList(objcmnParam.loggedCompany, objcmnParam.loggeduser, objcmnParam.pageNumber, objcmnParam.pageSize, objcmnParam.IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return obj;
        }

        [HttpPost]
        public IEnumerable<InvMrrQcMaster> ChkDuplicateNo(object[] data)
        {

            IEnumerable<InvMrrQcMaster> objNo = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                string MNo = data[1].ToString();
                objNo = objQCService.ChkDuplicateNo(objcmnParam, MNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objNo;
        }


        [Route("GetSPRPOLCType/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public List<CmnCombo> GetSPRPOLCType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCombo> lstSPRPOLCType = null;
            try
            {
                lstSPRPOLCType = objQCService.GetSPRPOLCType(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstSPRPOLCType;
        }

        [Route("GetChallanInvoiceReceiptTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCombo))]
        [HttpGet]
        public List<CmnCombo> GetChallanInvoiceReceiptTypes(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCombo> lstChallanInvoiceReceiptTypes = null;
            try
            {
                lstChallanInvoiceReceiptTypes = objQCService.GetChallanInvoiceReceiptTypes(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstChallanInvoiceReceiptTypes;
        }

        //[Route("GetGRRNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(InvGrrMaster))]
        //[HttpGet]
        //public List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<InvGrrMaster> lstInvGrrMaster = null;
        //    try
        //    {
        //        lstInvGrrMaster = objQCService.GetGRRNo(pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return lstInvGrrMaster;
        //}


        [HttpPost]
        public IHttpActionResult GetGRRNo(object[] data)
        {
            IEnumerable<vmQC> objGrrNo = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int32 TransTypeID = Convert.ToInt32(data[1]);
                objGrrNo = objQCService.GetGRRNo(objcmnParam, TransTypeID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objGrrNo
            });
        }

        [HttpPost]
        public IHttpActionResult GetSPRPOLCNoByID(object[] data)
        {
            IEnumerable<vmSPRPOLCNo> lstSPRPOLCNo = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                int SPRPOLCTypeID = Convert.ToInt16(data[1]);
                lstSPRPOLCNo = objQCService.GetSPRPOLCNoByID(objcmnParam, SPRPOLCTypeID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstSPRPOLCNo
            });
        }
        [HttpPost]
        public IHttpActionResult GetChallanInvoiceReceiptNoByID(object[] data)
        {
            IEnumerable<vmChallanInvoiceReceipt> lstChallanInvoiceReceipt = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                int CIRTypeID = Convert.ToInt16(data[1]);
                lstChallanInvoiceReceipt = objQCService.GetChallanInvoiceReceiptNoByID(objcmnParam, CIRTypeID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstChallanInvoiceReceipt
            });
        }

        [HttpPost]
        public HttpResponseMessage GetSPRPOLCDateByNo(object[] SPRPOLCTypeNo)
        {
            vmSPRPOLCNo objDate = null;
            try
            {
                int SprpolcType = Convert.ToInt16(SPRPOLCTypeNo[0]);
                Int64 SprpolcNo = Convert.ToInt64(SPRPOLCTypeNo[1]);
                objDate = objQCService.GetSPRPOLCDateByNo(SprpolcType, SprpolcNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objDate);
        }

        [HttpPost]
        public HttpResponseMessage GetCIRDateByNo(object[] CIRTypeNo)
        {
            vmChallanInvoiceReceipt objDate = null;
            try
            {
                int CIRType = Convert.ToInt16(CIRTypeNo[0]);
                Int64 CIRNo = Convert.ToInt64(CIRTypeNo[1]);
                objDate = objQCService.GetCIRDateByNo(CIRType, CIRNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objDate);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItemDetailByGrrNo(object[] data)
        {
            List<vmQC> lstQC = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                int grrID = Convert.ToInt16(data[1]);
                lstQC = objQCService.GetItemDetailByGrrNo(objcmnParam, grrID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstQC
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetQCMasterList(object[] data)
        {
            List<vmQC> objVmQC = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmQC = objQCService.GetQCMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objVmQC
            });
        }

        [HttpPost, BasicAuthorization]
        public List<vmQC> GetQCDetailsListByQCMasterID(object[] data)
        {
            List<vmQC> objQCDetails = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int64 Id = (Int64)data[1];
                objQCDetails = objQCService.GetQCDetailsListByQCMasterID(objcmnParam, Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return objQCDetails;
        }


        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateQCMasterNdetails(object[] data)
        {
            
            //  ArrayList fileNames = JsonConvert.DeserializeObject<ArrayList>(data[3].ToString());

            string result = "";
            try
            {
                InvMrrQcMaster qcMaster = JsonConvert.DeserializeObject<InvMrrQcMaster>(data[0].ToString());
                List<InvMrrQcDetail> qcDetails = JsonConvert.DeserializeObject<List<InvMrrQcDetail>>(data[1].ToString());
                int menuID = Convert.ToInt16(data[2]);

                if (ModelState.IsValid && qcMaster != null && qcMaster.MrrQcDate.ToString() != "" && qcDetails.Count > 0 && menuID > 0)
                {
                    result = objQCService.SaveUpdateQCMasterNdetails(qcMaster, qcDetails, menuID);
                    QCNo = result;
                    TransactionTypeID = qcMaster.TransactionTypeID ?? 0;
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

        [HttpPost()]
        public void UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC/");

            //sPath = @"D:/Upload/LC/";

            //var directory = @"D:/Upload/LC/";
            CmnDocumentPath objDocumentPath = objQCService.GetUploadPath(TransactionTypeID);

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
                        hpf.SaveAs(directory + QCNo + "_Doc_" + fileSerial + exttension);
                        iUploadedCnt = iUploadedCnt + 1;
                        hpf.InputStream.Dispose();
                    }
                }
            }
            QCNo = "";

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
                objFileDetail = objQCService.GetFileDetailsById(id, TransTypeID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }
    }
}
