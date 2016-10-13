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



namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/Issue")]
    public class IssueController : ApiController
    {
        iSystemCommonDDL objCmnItemMgtEF = null;
        iCmnRawMaterial objCmnRawMaterial = null;
        iIssueMgt objIssueService = null;
        
        public IssueController()
        {
            objCmnItemMgtEF = new SystemCommonDDL();
            objCmnRawMaterial = new CmnRawMaterialMgt();
            objIssueService = new IssueMgt();
        }

        [Route("GetRequisitionNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{RequisitionTypeID:int}")]
        [ResponseType(typeof(InvRequisitionMaster))]
        public List<InvRequisitionMaster> GetRequisitionNo(int? pageNumber, int? pageSize, int? IsPaging, int CompanyID, int RequisitionTypeID)
        {
            List<InvRequisitionMaster> ListRequisition = null;

            try
            {
                ListRequisition = objIssueService.GetRequisitionNo(pageNumber, pageSize, IsPaging, CompanyID, RequisitionTypeID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return ListRequisition;
        }

        [Route("GetRequisitionItemList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{RequisitionID:int}/{MrrID:int}")]
        [ResponseType(typeof(vmRequisition))]
        [HttpGet, BasicAuthorization]
        public List<vmRequisition> GetRequisitionItemList(int? pageNumber, int? pageSize, int? IsPaging, int? RequisitionID, int? MrrID)
        {
            List<vmRequisition> RequisitionItemList = null;

            try
            {
                RequisitionItemList = objIssueService.GetRequisitionItemList(pageNumber, pageSize, IsPaging, RequisitionID, MrrID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return RequisitionItemList;
        }


        [Route("GetUnits/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnUOM))]
        [HttpGet,BasicAuthorization]
        public List<vmUnit> GetUnits(int? pageNumber, int? pageSize, int? IsPaging,int? CompanyID)
        {
            List<vmUnit> Units = null;

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

        [Route("GetMRRList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(InvMrrMaster))]
        [HttpGet,BasicAuthorization]
        public List<InvMrrMaster> GetMRRList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<InvMrrMaster> Mrrlist = null;

            try
            {
                Mrrlist = objIssueService.GetMRRList(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Mrrlist;
        }

        [Route("GetChallanList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(InvMrrMaster))]
        [HttpGet, BasicAuthorization]
        public List<InvRChallanMaster> GetChallanList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<InvRChallanMaster> Challanlist = null;

            try
            {
               Challanlist = objIssueService.GetChallanList(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Challanlist;
        }

        [Route("GetGRRList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(InvMrrMaster))]
        [HttpGet, BasicAuthorization]
        public List<InvGrrMaster> GetGRRList(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<InvGrrMaster> Grrlist = null;

            try
            {
               Grrlist = objIssueService.GetGRRList(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Grrlist;
        }

        [Route("GetAllUsers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(CmnUser))]
        [HttpGet, BasicAuthorization]
        public List<CmnUser> GetUsers(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<CmnUser> users = null;

            try
            {
                //users = objRequisitionService.GetUsers(pageNumber, pageSize, IsPaging).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return users;
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveIssueMasterDetails(object[] data)
        {
            InvIssueMaster IssueMaster = JsonConvert.DeserializeObject<InvIssueMaster>(data[0].ToString());
            List<InvIssueDetail> IssueDetails = JsonConvert.DeserializeObject<List<InvIssueDetail>>(data[1].ToString());
            int menuID = Convert.ToInt16(data[2]);           
            string result = "";
            try
            {
                if (ModelState.IsValid && IssueMaster != null && IssueDetails.Count > 0 && menuID != null)
                {
                    result = objIssueService.SaveIssueMasterDetails(IssueMaster, IssueDetails, menuID);
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
        public IHttpActionResult GetIssueMasterList(object[] data)
        {
            List<vmIssueMaster> objVmIssue = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objVmIssue = objIssueService.GetIssueMasterList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objVmIssue
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetRequisitionMaster(object[] data)
        {

            int recordsTotal = 0;
            IEnumerable<InvRequisitionMaster> objRequisitionMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
               // objRequisitionMaster = objRequisitionService.GetRequisitionMaster(objcmnParam, out recordsTotal).ToList();
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

        [Route("GetIssueMasterByIssueId/{IssueId:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmIssueMaster))]
        [HttpGet, BasicAuthorization]
        public vmIssueMaster GetIssueMasterByIssueId(int? IssueId, int CompanyID)
        {
            vmIssueMaster Issuelist = null;

            try
            {
                Issuelist = objIssueService.GetIssueMasterByIssueId(IssueId, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Issuelist;
        }


        [Route("GetIssueDetailByIssueId/{IssueId:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmIssueMaster))]
        [HttpGet, BasicAuthorization]
        public IEnumerable<vmRequisition> GetIssueDetailByIssueId(int? IssueId, int CompanyID)
        {
            IEnumerable<vmRequisition> Issuelist = null;

            try
            {
                Issuelist = objIssueService.GetIssueDetailByIssueId(IssueId, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Issuelist;
        }
    }

}
