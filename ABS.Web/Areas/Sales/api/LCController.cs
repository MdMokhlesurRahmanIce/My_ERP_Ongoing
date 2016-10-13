using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Sales.Factories;
using ABS.Service.Sales.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;


namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/LC")]
    public class LCController : ApiController
    {
        private iLCMgt objLCService = null;
        private static string lcReferenceNo { get; set; }

        public LCController()
        {
            objLCService = new LCMgt();
        }

        #region LCEntry

        [Route("GetCompany/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"),ResponseType(typeof(CmnCompany)),HttpGet, BasicAuthorization]
        public IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCompany> objListCompany = null;
            try
            {
                objListCompany = objLCService.GetCompany(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany;
        }

        [Route("GetBuyer/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnUser)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnUser> GetBuyer(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnUser> objListBuyer = null;
            try
            {
                objListBuyer = objLCService.GetBuyer(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBuyer;
        }

        [Route("GetBank/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnBank)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnBank> GetBank(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnBank> objListBank = null;
            try
            {
                objListBank = objLCService.GetBank(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBank;
        }

        [Route("GetPISight/{pageNumber:int}/{pageSize:int}/{IsPaging:int}"), ResponseType(typeof(CmnCombo)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnCombo> GetPISight(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnCombo> objListSight = null;
            try
            {
                objListSight = objLCService.GetPISight(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSight;
        }

        [Route("GetBankBranchById/{id:int}"), ResponseType(typeof(CmnBankBranch)), HttpGet, BasicAuthorization]
        public IEnumerable<CmnBankBranch> GetBankBranchById(int id)
        {
            IEnumerable<CmnBankBranch> objListBankBranch = null;
            try
            {
                objListBankBranch = objLCService.GetBankBranchById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListBankBranch;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmSalLCDetail> GetPendingPI(object[] data)
        {
            IEnumerable<vmSalLCDetail> objPendingPIInfo = null;
            try
            {
                int buyerId = Convert.ToInt16(data[0]);
                int compnayId = Convert.ToInt16(data[1]);
                objPendingPIInfo = objLCService.GetPendingPI(buyerId, compnayId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objPendingPIInfo;
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage saveLC(object[] data)
        {
            SalLCMaster itemMaster = JsonConvert.DeserializeObject<SalLCMaster>(data[0].ToString());
            List<SalLCDetail> itemDetails = JsonConvert.DeserializeObject<List<SalLCDetail>>(data[1].ToString());

            // List<CmnDocument> fileInfo = JsonConvert.DeserializeObject<List<CmnDocument>>(data[2].ToString());
            // ArrayList ar = new ArrayList();
            ArrayList fileNames = JsonConvert.DeserializeObject<ArrayList>(data[2].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[3].ToString());

            SalLCMaster obj = new SalLCMaster();
            string result = "";
            try
            {
                //if (ModelState.IsValid)
                //{

                if (ModelState.IsValid && itemMaster != null && itemMaster.BuyerID.ToString() != "" && itemDetails.Count > 0)
                {
                    result = objLCService.SaveUpdateLC(itemMaster, itemDetails, fileNames, objcmnParam);
                    lcReferenceNo = result;
                }
                else
                {
                    result = "";
                }

                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            // System.Web.HttpContext.Current.Session.Add("LCReferenceNo", result);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCMaster(object[] data)
        {
            IEnumerable<vmSalLCDetail> objvmSalLCDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objvmSalLCDetail = objLCService.GetLCMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objvmSalLCDetail
            });
            //return objvmSalLCDetail;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCDetailByID(object[] data)
        {
            IEnumerable<vmSalLCDetail> objLCById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objLCById = objLCService.GetLCDetailByID(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objLCById
            });
        }

        [Route("GetLCMasterById/{id:int}"), ResponseType(typeof(SalLCMaster)), HttpGet, BasicAuthorization]
        public IEnumerable<vmSalLCDetail> GetLCMasterById(int id)
        {
            IEnumerable<vmSalLCDetail> objLCMasterById = null;
            try
            {
                objLCMasterById = objLCService.GetLCMasterById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLCMasterById;
        }

        #endregion  LCEntry


        //[HttpPost(), BasicAuthorization]
        public void UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            //sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC/");

            //sPath = @"D:/Upload/LC/";

            //var directory = @"D:/Upload/LC/";
            CmnDocumentPath objDocumentPath = objLCService.GetUploadPath();

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
                        hpf.SaveAs(directory + lcReferenceNo + "_Doc_" + fileSerial + exttension);
                        iUploadedCnt = iUploadedCnt + 1;
                        hpf.InputStream.Dispose();
                    }
                }
            }
            lcReferenceNo = "";

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
        [Route("GetFileDetailsById/{id:int}"), ResponseType(typeof(CmnDocument)), HttpGet, BasicAuthorization]
        public IEnumerable<vmCmnDocument> GetFileDetailsById(int id)
        {
            IEnumerable<vmCmnDocument> objFileDetail = null;
            try
            {
                objFileDetail = objLCService.GetFileDetailsById(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }

    }
}
