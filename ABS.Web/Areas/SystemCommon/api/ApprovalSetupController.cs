using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/ApprovalSetup")]
    public class ApprovalSetupController : ApiController
    {
        private iApprovalSetupMgt objApprovalSetupService = null;
        private iSystemCommonDDL objSystemCommonDll = null;

        public ApprovalSetupController()
        {
            objApprovalSetupService = new ApprovalSetupMgt();
        }

        [HttpPost]
        public HttpResponseMessage SaveUpdateApprovalSetup(object[] data)
        {
            CmnWorkFlowMaster workFlowMaster = JsonConvert.DeserializeObject<CmnWorkFlowMaster>(data[0].ToString());
            List<CmnWorkFlowDetail> workFlowDetails = JsonConvert.DeserializeObject<List<CmnWorkFlowDetail>>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && workFlowMaster != null && workFlowMaster.CreateBy.ToString() != "" && workFlowDetails.Count > 0)
                {
                    result = objApprovalSetupService.SaveUpdateApprovalSetup(workFlowMaster, workFlowDetails);
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
        public IEnumerable<vmCmnMenuPermission> GetApprovalSetupRecords(object data)
        {
            IEnumerable<vmCmnMenuPermission> objVmCmnMenuPermission = null;
            try
            {
                objVmCmnMenuPermission = objApprovalSetupService.GetApprovalSetupRecords();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objVmCmnMenuPermission;
        }

        [HttpPost]
        public IEnumerable<vmCmnMenuPermission> GetApprovalDetailsByWorkFlowID(object data)
        {
            int workFlowID = Convert.ToInt16(data);
            IEnumerable<vmCmnMenuPermission> objVmCmnMenuPermission = null;
            try
            {
                objVmCmnMenuPermission = objApprovalSetupService.GetApprovalDetailsByWorkFlowID(workFlowID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objVmCmnMenuPermission;
        }

        [Route("GetBranchDetails/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet]
        public List<vmBranch> GetBranchDetails(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            objSystemCommonDll = new SystemCommonDDL();
            List<vmBranch> _objBranchs = null;
            try
            {
                _objBranchs = objSystemCommonDll.GetBranchDetails(pageNumber, pageSize, IsPaging, CompanyId).ToList();

            }
            catch (Exception e)
            {
                _objBranchs = null;
                e.ToString();
            }
            return _objBranchs;

        }


        [Route("GetTemsByDepartmentID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DepartmentID:int}")]
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet]
        public List<vmTeam> GetTemsByDepartmentID(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID)
        {
            objSystemCommonDll = new SystemCommonDDL();
            List<vmTeam> _objvmTems = null;
            try
            {
                _objvmTems = objSystemCommonDll.GetTemsByDepartmentID(pageNumber, pageSize, IsPaging, DepartmentID).ToList();

            }
            catch 
            {
                _objvmTems = null;
                ToString();
            }
            return _objvmTems;

        }

         [Route("GetTeamsUserByTemID/{id:int}")]      
        [ResponseType(typeof(CmnOrganogram))]
        [HttpGet]
        public List<vmTeam> GetTeamsUserByTemID(int? id)
        {
            objSystemCommonDll = new SystemCommonDDL();
            List<vmTeam> _objvmTems = null;
            try
            {
                _objvmTems = objApprovalSetupService.GetTeamsUserByTemID(id).ToList();

            }
            catch
            {
                _objvmTems = null;
                ToString();
            }
            return _objvmTems;

        }

        

    }
}
