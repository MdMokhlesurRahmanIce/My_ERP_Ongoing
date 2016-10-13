using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/SizeBeamIssue")]
    public class SizeBeamIssueController : ApiController
    {
        private iProductionDDLMgt _objProductionDDL = null;
        private iSizeBeamIssueMgt _objiSizeBeamIssueMgt = null;
        public SizeBeamIssueController()
        {
            _objProductionDDL = new ProductionDDLMgt();

        }


        //[Route("GetMachineByTypeAndGroupID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{ItemTypeID:int}/{ItemGroupID:int}")]
        //[ResponseType(typeof(CmnItemMaster))]
        //[HttpGet]
        //public List<vmItemMaster> GetMachineByTypeAndGroupID(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? ItemTypeID, int? ItemGroupID)
        //{
        //    List<vmItemMaster> itemMasters = null;
        //    try
        //    {
        //        itemMasters = _objProductionDDL.GetMachineByTypeAndGroupID(pageNumber, pageSize, IsPaging, CompanyId, ItemTypeID, ItemGroupID);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return itemMasters;
        //}

        [Route("GetAllArticalByItemType/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{Type:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmItemMaster> GetAllArticalByItemType(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, int? Type)
        {
            List<vmItemMaster> itemMasters = null;
            try
            {
                itemMasters = _objProductionDDL.GetAllArticalByItemType(pageNumber, pageSize, IsPaging, CompanyId, Type);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemMasters;
        }
        [Route("GetSetNoByArticalNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmSetDetail> GetSetNoByArticalNo(int? pageNumber, int? pageSize, int? IsPaging, int? ItemID)
        {
            List<vmSetDetail> _setDetailobj = null;
            try
            {
                _setDetailobj = _objProductionDDL.GetSetNoByArticalNo(pageNumber, pageSize, IsPaging, ItemID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetSetDeatailBySetNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{SetNo:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmSizeBeamIssue GetSetDeatailBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID)
        {
            vmSizeBeamIssue _setDetailobj = null;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();

                _setDetailobj = _objiSizeBeamIssueMgt.GetSetDeatailBySetNo(pageNumber, pageSize, IsPaging, SetNo, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetSizeBeamIssuemasterDetailByBeamIssueID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{BeamIssueId:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmSizeBeamIssue GetSizeBeamIssuemasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID)
        {
            vmSizeBeamIssue _setDetailobj = null;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();

                _setDetailobj = _objiSizeBeamIssueMgt.GetSizeBeamIssuemasterDetailByBeamIssueID(pageNumber, pageSize, IsPaging, BeamIssueId, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }


        [Route("GetSizingMRRMasterDetailByBeamIssueID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{BeamIssueId:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmSizeBeamIssue> GetSizingMRRMasterDetailByBeamIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? BeamIssueId, int? LoginCompanyID)
        {
            List<vmSizeBeamIssue> _setDetailobj = null;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();

                _setDetailobj = _objiSizeBeamIssueMgt.GetSizingMRRMasterDetailByBeamIssueID(pageNumber, pageSize, IsPaging, BeamIssueId, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetSizingMRRMasterDetailBySetID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{SetNo:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmSizeBeamIssue> GetSizingMRRMasterDetailBySetID(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo, int? LoginCompanyID)
        {
            List<vmSizeBeamIssue> _sizeBeamIssues = null;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();

                _sizeBeamIssues = _objiSizeBeamIssueMgt.GetSizingMRRMasterDetailBySetID(pageNumber, pageSize, IsPaging, SetNo, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _sizeBeamIssues;
        }

        [Route("GetSizeBeamIssueDetails/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{LoginCompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmSizeBeamIssue> GetSizeBeamIssueDetails(int? pageNumber, int? pageSize, int? IsPaging, int? LoginCompanyID)
        {
            List<vmSizeBeamIssue> _sizeBeamIssues = null;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();

                _sizeBeamIssues = _objiSizeBeamIssueMgt.GetSizeBeamIssueDetails(pageNumber, pageSize, IsPaging, LoginCompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _sizeBeamIssues;
        }


        [Route("GetLoom/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmLoom> GetLoom(int? CompanyID)
        {
            List<vmLoom> _setDetailobj = null;
            try
            {
                _setDetailobj = _objProductionDDL.GetLoom(CompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [HttpPost]
        public int SaveSizeBeamIssue(object[] data)
        {
            List<vmSizeBeamIssue> _objSizeIssueDetails = JsonConvert.DeserializeObject<List<vmSizeBeamIssue>>(data[0].ToString());
            vmSizeBeamIssue _objSizeIssueMaster = JsonConvert.DeserializeObject<vmSizeBeamIssue>(data[1].ToString());
            int result = 0;
            try
            {
                _objiSizeBeamIssueMgt = new SizeBeamIssueMgt();
                result = _objiSizeBeamIssueMgt.SaveSizeBeamIssue(_objSizeIssueDetails, _objSizeIssueMaster);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return result;
        }

    }
}

