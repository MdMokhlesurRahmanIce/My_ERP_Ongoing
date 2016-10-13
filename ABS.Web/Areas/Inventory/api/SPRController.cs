using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ABS.Models;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using System.Web.Http.Description;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Models.ViewModel.SystemCommon;
using Newtonsoft.Json;
using ABS.Models.ViewModel.Inventory;
using ABS.Web.Attributes;
using System.Threading.Tasks;
using ABS.Web.Areas.SystemCommon.Hubs;
using System.Collections;
using System.IO;
using ABS.Models.ViewModel.Sales;

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/SPR")]
    public class SPRController : ApiController
    {
        iSystemCommonDDL objCmnItemMgtEF = null;
        iCmnRawMaterial objCmnRawMaterial = null;
        iSPRMgt objRequisitionService = new SPRMgt();
        private static int TransactionTypeID { get; set; }
        private static string SPRNo { get; set; }
        public SPRController()
        {
          //  objCmnItemMgtEF = new SystemCommonDDL();
         //   objCmnRawMaterial = new CmnRawMaterialMgt();
            
        }

        [Route("GetCompany/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{companyID:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnCompany> GetCompany(int? pageNumber, int? pageSize, int? IsPaging, int companyID)
        {
            IEnumerable<CmnCompany> objListCompany = null;
            try
            {
                objListCompany = objRequisitionService.GetCompany(pageNumber, pageSize, IsPaging, companyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListCompany.ToList();
        }

        //GetLotByItemId
        [Route("GetGrade/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnItemGrade> GetGrade(int? pageNumber, int? pageSize, int? IsPaging)
        {
            IEnumerable<CmnItemGrade> objListGrade = null;
            try
            {
                objListGrade = objRequisitionService.GetGrade(pageNumber, pageSize, IsPaging);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListGrade.ToList();
        }

        [Route("GetAllSupplier/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(vmUsers))]
        [HttpGet]
        public IEnumerable<vmUsers> GetAllSupplier(int? pageNumber, int? pageSize, int? IsPaging, int ItemId, int LoginCompanyID)
        {
            IEnumerable<vmUsers> listSupplier = null;
            try
            {
                listSupplier = objRequisitionService.GetAllSupplier(pageNumber, pageSize, IsPaging, ItemId, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listSupplier;
        }


        #region GetBatch
        [Route("GetAllBatch/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnBatch))]
        [HttpGet]
        public IEnumerable<CmnBatch> GetAllBatch(int? pageNumber, int? pageSize, int? IsPaging,int ItemId,int LoginCompanyID)
        {
            IEnumerable<CmnBatch> listBatch = null;
            try
            {
                listBatch = objRequisitionService.GetAllBatch(pageNumber, pageSize, IsPaging, ItemId, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return listBatch;
        }
        #endregion GetBatch

        //GetLotByItemId
        [Route("GetLotNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnLot))]
        [HttpGet]
        public IEnumerable<CmnLot> GetLotNo(int? pageNumber, int? pageSize, int? IsPaging, int ItemID, int LoginCompanyID)
        {
            IEnumerable<CmnLot> objListLot = null;
            try
            {
                objListLot = objRequisitionService.GetLotNo(pageNumber, pageSize, IsPaging, ItemID, LoginCompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListLot.ToList();
        }

        [HttpPost]
        public IEnumerable<InvRequisitionMaster> ChkDuplicateNo(object[] data)
        {

            IEnumerable<InvRequisitionMaster> objNo = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                string MNo = data[1].ToString();
                objNo = objRequisitionService.ChkDuplicateNo(objcmnParam, MNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objNo;
        }


        [Route("GetUnits/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}"), ResponseType(typeof(CmnUOM)), HttpGet, BasicAuthorization]
        public List<vmUnit> GetUnits(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            List<vmUnit> Units = null;
            SystemCommonDDL objCmnItemMgtEF = new SystemCommonDDL();
            try
            {
                Units = objCmnItemMgtEF.GetAllUnit(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Units;
        }


        [Route("GetAllUsers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{UserTypeID:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet, BasicAuthorization]
        public List<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging, int? UserTypeID,int? CompanyID)
        {
            List<CmnUser> users = null;

            try
            {
                users = objRequisitionService.GetUsers(pageNumber, pageSize, IsPaging, UserTypeID, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return users;
        }

        [Route("GetAllRequisitionType/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnTransactionType))]
        [HttpGet, BasicAuthorization]
        public List<CmnTransactionType> GetAllRequisitionType(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnTransactionType> RequisitionType = null;

            try
            {
                RequisitionType = objRequisitionService.GetAllRequisitionType(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return RequisitionType;
        }

        [Route("GetAllItemGroup/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet, BasicAuthorization]
        public List<CmnItemGroup> GetAllItemGroup(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnItemGroup> IstemGroup = null;

            try
            {
                IstemGroup = objRequisitionService.GetAllItemGroup(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return IstemGroup;
        }

        [Route("GetAllCompany/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnCompany))]
        [HttpGet, BasicAuthorization]
        public List<CmnCompany> GetAllCompany(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnCompany> CmnCompanyList = null;

            try
            {
                CmnCompanyList = objRequisitionService.GetAllCompany(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return CmnCompanyList;
        }


        [Route("GetDepartmentByCompanyID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet, BasicAuthorization]
        public List<vmCmnOrganogram> GetDepartmentByCompanyID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            List<vmCmnOrganogram> Departmentlist = null;

            try
            {
                Departmentlist = objRequisitionService.GetDepartmentByCompanyID(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Departmentlist;
        }

        [Route("GetItemListByGroupID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{GroupID:int}")]
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet, BasicAuthorization]
        public List<CmnItemMaster> GetItemListByGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? GroupID)
        {
            List<CmnItemMaster> Itemlist = null;

            try
            {
                Itemlist = objRequisitionService.GetItemListByGroupID(pageNumber, pageSize, IsPaging, GroupID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Itemlist;
        }

        [Route("LaodItemRateNUnit/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{itemid:int}")]
        [ResponseType(typeof(vmRequisition))]
        [HttpGet, BasicAuthorization]
        public List<vmRequisition> LaodItemRateNUnit(int? pageNumber, int? pageSize, int? IsPaging, int? itemid)
        {
            List<vmRequisition> ItemRate = null;

            try
            {
                ItemRate = objRequisitionService.LaodItemRateNUnit(pageNumber, pageSize, IsPaging, itemid).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ItemRate;
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
            CmnDocumentPath objDocumentPath = objRequisitionService.GetUploadPath(TransactionTypeID);

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
                        hpf.SaveAs(directory + SPRNo + "_Doc_" + fileSerial + exttension);
                        iUploadedCnt = iUploadedCnt + 1;
                        hpf.InputStream.Dispose();
                    }
                }
            }
            SPRNo = "";

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

           [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveSRequisitionMasterDetails(object[] data)
        {
            InvRequisitionMaster RequisitionMaster = JsonConvert.DeserializeObject<InvRequisitionMaster>(data[0].ToString());
            List<vmRequisitionDetails> RequisitionDetails = JsonConvert.DeserializeObject<List<vmRequisitionDetails>>(data[1].ToString());
          //  int menuID = Convert.ToInt16(data[2]);
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && RequisitionMaster != null && RequisitionDetails.Count > 0 && commonEntity != null)
                {
                    result = objRequisitionService.SaveSRequisitionMasterDetails(RequisitionMaster, RequisitionDetails, commonEntity);
                    NotificationHubs.BroadcastData(new NotificationEntity());
                    SPRNo = result;
                    TransactionTypeID = RequisitionMaster.RequisitionTypeID;
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
        public HttpResponseMessage SaveRequisitionMasterDetails(object[] data)
        {
            InvRequisitionMaster RequisitionMaster = JsonConvert.DeserializeObject<InvRequisitionMaster>(data[0].ToString());
            List<vmRequisitionDetails> RequisitionDetails = JsonConvert.DeserializeObject<List<vmRequisitionDetails>>(data[1].ToString());
          //  int menuID = Convert.ToInt16(data[2]);
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            ArrayList fileNames = JsonConvert.DeserializeObject<ArrayList>(data[3].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid && RequisitionMaster != null && RequisitionDetails.Count > 0 && commonEntity != null)
                {
                    result = objRequisitionService.SaveRequisitionMasterDetails(RequisitionMaster, RequisitionDetails, commonEntity, fileNames);
                    NotificationHubs.BroadcastData(new NotificationEntity());
                    SPRNo = result;
                    TransactionTypeID = RequisitionMaster.RequisitionTypeID;
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
        public IHttpActionResult GetRequisitionMaster(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<vmRequisition> objRequisitionMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int RequisitionTypeId = Convert.ToInt16(data[1].ToString());
            try
            {
                objRequisitionMaster = objRequisitionService.GetRequisitionMaster(objcmnParam, out recordsTotal, RequisitionTypeId).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objRequisitionMaster
            });

        }
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetRequisitionMasterSPR(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<vmRequisition> objRequisitionMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int RequisitionTypeId = Convert.ToInt16(data[1].ToString());
            try
            {
                objRequisitionMaster = objRequisitionService.GetRequisitionMasterSPR(objcmnParam, out recordsTotal, RequisitionTypeId).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objRequisitionMaster
            });

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
                objFileDetail = objRequisitionService.GetFileDetailsById(id, TransTypeID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objFileDetail;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmSPR> GetItmDetailByItmCode(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ItemCode = data[1].ToString();

            IEnumerable<vmSPR> objItemDtls = null;
            try
            {
                objItemDtls = objRequisitionService.GetItmDetailByItmCode(objcmnParam, ItemCode);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemDtls;
        }

        [HttpPost, BasicAuthorization]
        public IEnumerable<vmSPR> GetItmDetailByItemId(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string ItemID = data[1].ToString();

            IEnumerable<vmSPR> objItemDtls = null;
            try
            {
                objItemDtls = objRequisitionService.GetItmDetailByItemId(objcmnParam, ItemID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemDtls;
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItmDetail(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
          int recordsTotal = 0;
            IEnumerable<vmSPR> objItem = null;
            try
            {
                objItem = objRequisitionService.GetItmDetail(objcmnParam , out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objItem
            });
            
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult SprUpdateDelete(object[] data)
        {
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string Result = string.Empty;
            try
            {
                Result = objRequisitionService.SprUpdateDelete(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                Result
            });

        }

        [Route("GetRequisitonMasterByRequisitionID/{RequisitionId:int}/{CompanyId:int}")]
        [ResponseType(typeof(vmRequisition))]
        [HttpGet, BasicAuthorization]
        public vmRequisition GetRequisitonMasterByRequisitionID(int? RequisitionId,int CompanyId)
        {
            vmRequisition RequisitionList = null;

            try
            {
                RequisitionList = objRequisitionService.GetRequisitonMasterByRequisitionID(RequisitionId,CompanyId);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return RequisitionList;
        }


        [Route("GetRequisitonDetailByRequisitionID/{RequisitionId:int}/{CompanyId:int}")]
        [ResponseType(typeof(vmRequisitionDetails))]
        [HttpGet, BasicAuthorization]
        public IEnumerable<vmRequisitionDetails> GetRequisitonDetailByRequisitionID(int? RequisitionId,int CompanyId )
        {
            IEnumerable<vmRequisitionDetails> RequisitionItemList = null;

            try
            {
                RequisitionItemList = objRequisitionService.GetRequisitonDetailByRequisitionID(RequisitionId, CompanyId);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return RequisitionItemList;
        }

    }

}
