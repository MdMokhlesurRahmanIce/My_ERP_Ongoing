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
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;


namespace ABS.Web.Areas.Inventory.api
{

    [RoutePrefix("Inventory/api/GRR")]
    public class GRRController : ApiController
    {

        private iGRRMgt objGRRService = null;

        private iSystemCommonDDL objSystemCommonDll = null;
        private static string GRRNo { get; set; }

        private static int TransactionTypeID { get; set; }

        public GRRController()
        {
            objGRRService = new GRRMgt();
        }

        [HttpPost]
        public IEnumerable<InvGrrMaster> ChkDuplicateNo(object[] data)
        {

            IEnumerable<InvGrrMaster> objNo = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                string MNo = data[1].ToString();
                objNo = objGRRService.ChkDuplicateNo(objcmnParam, MNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objNo;
        }


        [HttpPost]
        public IEnumerable<InvGrrMaster> ChkDuplicateGrrNo(object[] data)
        {

            IEnumerable<InvGrrMaster> objNo = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                string MNo = data[1].ToString();
                objNo = objGRRService.ChkDuplicateGrrNo(objcmnParam, MNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objNo;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartmentDetails(object[] data)
        {
         
            IEnumerable<vmDepartment> ListDeptDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListDeptDetail = objGRRService.GetDepartmentParentList(objcmnParam.pageNumber, objcmnParam.pageSize, objcmnParam.IsPaging, objcmnParam.DepartmentID, objcmnParam.loggedCompany).ToList(); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListDeptDetail
            });
            //return objDOMaster.ToList();
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLoggedDeptName(object[] data) 
        {
            
            IEnumerable<vmDepartment> dept = null;
            
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                dept = objGRRService.GetLoggedDeptName(objcmnParam).ToList(); 
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                dept
            });
            //return objDOMaster.ToList();
        }


        [HttpPost]
        public IHttpActionResult GetSPRNo(object[] data)
        {
            IEnumerable<vmChallan> objSPRNo = null;
           
            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int32 ReqTypeID = Convert.ToInt32(data[1]);
                objSPRNo = objGRRService.GetSPRNo(objcmnParam, ReqTypeID, out recordsTotal);
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
        public IHttpActionResult GetLoanReturnIssueNo(object[] data)
        {
            IEnumerable<vmChallan> lstIssue = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                Int32 ReqTypeID = Convert.ToInt32(data[1]);
                lstIssue = objGRRService.GetLoanReturnIssueNo(objcmnParam, ReqTypeID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstIssue
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
                objPONo = objGRRService.GetPONo(objcmnParam, out recordsTotal);
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
                objLocation = objGRRService.GetLocation(objcmnParam, out recordsTotal);
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
        public IHttpActionResult GetPackingUnit(object[] data)
        {
            IEnumerable<CmnUOM> objPackingUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objPackingUnit = objGRRService.GetPackingUnit(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objPackingUnit
            });
        }

        [HttpPost]
        public IHttpActionResult GetWeightUnit(object[] data)
        {
            IEnumerable<CmnUOM> objWeightUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objWeightUnit = objGRRService.GetWeightUnit(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objWeightUnit
            });
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmChallan> GetItemDetailBySPRID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 SprID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailBySPRID = null;
            try
            {
                lstItemDetailBySPRID = objGRRService.GetItemDetailBySPRID(objcmnParam, SprID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailBySPRID;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmChallan> GetItemDetailFGrrByIssueID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 IssueID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailByIssueID = null;
            try
            {
                lstItemDetailByIssueID = objGRRService.GetItemDetailFGrrByIssueID(objcmnParam, IssueID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstItemDetailByIssueID;
        }




        [HttpPost]
        public IEnumerable<vmChallan> GetItemDetailByPOID(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            Int64 POID = Convert.ToInt64(data[1]);

            IEnumerable<vmChallan> lstItemDetailByPOID = null;
            try
            {
                lstItemDetailByPOID = objGRRService.GetItemDetailByPOID(objcmnParam, POID);
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
                objItemDtls = objGRRService.GetItmDetailByItmCode(objcmnParam, ItemCode);
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
                lstParty = objGRRService.GetParty(pageNumber, pageSize, IsPaging);
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
                lstSampleNo = objGRRService.GetItemSampleNo(pageNumber, pageSize, IsPaging);
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
                lstChallanTrnsTypes = objGRRService.GetChallanTrnsTypes(pageNumber, pageSize, IsPaging);
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
                lstChallanTrnsTypes = objGRRService.GetCurrency(pageNumber, pageSize, IsPaging);
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
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objItemMaster = objGRRService.GetItemMasterById(objcmnParam, out recordsTotal);
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetGrrDetailByGrrID(object[] data)
        {
            IEnumerable<vmChallan> lstChallanDetail = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                Int64 grrID = Convert.ToInt64(data[1]);
                lstChallanDetail = objGRRService.GetGrrDetailByGrrID(objcmnParam, grrID, out recordsTotal);
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
        public HttpResponseMessage SaveUpdateChallanMasterNdetails(object[] data)
        {
            
            string result = "";
            try
            {
                InvGrrMaster itemMaster = JsonConvert.DeserializeObject<InvGrrMaster>(data[0].ToString());
                List<InvGrrDetail> itemDetails = JsonConvert.DeserializeObject<List<InvGrrDetail>>(data[1].ToString());
                int menuID = Convert.ToInt16(data[2]);
              //  long issueID = Convert.ToInt64(data[3]);

                ArrayList fileNames = JsonConvert.DeserializeObject<ArrayList>(data[3].ToString());

                if (ModelState.IsValid && itemMaster != null && itemMaster.GrrDate.ToString() != "" && itemDetails.Count > 0 && menuID != null)
                {
                    result = objGRRService.SaveUpdateChallanMasterNdetails(itemMaster, itemDetails, menuID, fileNames);
                    GRRNo = result;
                    TransactionTypeID = itemMaster.TransactionTypeID ?? 0;
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
                    result = objGRRService.SaveLot(objCmnLot);
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
                    result = objGRRService.SaveBatch(objCmnBatch);
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetGrrMasterList(object[] data)
        {
            IEnumerable<vmChallan> lstVmChallanMaster = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                bool IsSPR = Convert.ToBoolean(data[1]);
                lstVmChallanMaster = objGRRService.GetGrrMasterList(objcmnParam, IsSPR, out recordsTotal);
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
            CmnDocumentPath objDocumentPath = objGRRService.GetUploadPath(TransactionTypeID);

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
                objFileDetail = objGRRService.GetFileDetailsById(id, TransTypeID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }
    }
}
