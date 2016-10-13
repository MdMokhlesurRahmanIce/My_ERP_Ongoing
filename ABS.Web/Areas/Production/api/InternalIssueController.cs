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
    [RoutePrefix("Production/api/InternalIssue")]
    public class InternalIssueController : ApiController
    {
        private iProductionDDLMgt _objProductionDDL = null;
        private iInternalIssue _objInternalIssue = null;

        public InternalIssueController()
        {
            _objProductionDDL = new ProductionDDLMgt();
            _objInternalIssue = new InternalIssueMgt();
        }
        [Route("GetArticals/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmItemMaster> GetArticals(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            List<vmItemMaster> itemMasters = null;
            try
            {
                itemMasters = _objProductionDDL.GetArticals(pageNumber, pageSize, IsPaging, CompanyId);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemMasters;
        }

        [Route("GetInternalIssueDetial/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}/{IsIssuedBall:bool}/{IsReceivedDy:bool}/{IsIssuedDy:bool}/{IsReceivedLCB:bool}")]
        [HttpGet]
        public List<vmIssueDetail> GetInternalIssueDetial(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId, bool IsIssuedBall, bool IsReceivedDy, bool IsIssuedDy, bool IsReceivedLCB)
        {

            List<vmIssueDetail> IssueDetails = null;
            try
            {
                IssueDetails = _objInternalIssue.GetInternalIssueDetial(pageNumber, pageSize, IsPaging, CompanyId, IsIssuedBall, IsReceivedDy, IsIssuedDy, IsReceivedLCB);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return IssueDetails;
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

        [Route("GetCanNoByDeapartmentId/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DepartmentID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmOutputUnit> GetCanNoByDeapartmentId(int? pageNumber, int? pageSize, int? IsPaging, int? DepartmentID)
        {
            List<vmOutputUnit> _setDetailobj = null;
            try
            {
                _setDetailobj = _objProductionDDL.GetCanNoByDeapartmentId(pageNumber, pageSize, IsPaging, DepartmentID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetSetDetailsBySetNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{SetNo:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmSetSetupMasterDetail GetSetDetailsBySetNo(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo)
        {
            vmSetSetupMasterDetail _setDetailobj = null;
            try
            {
                _setDetailobj = _objInternalIssue.GetSetDetailsBySetNo(pageNumber, pageSize, IsPaging, SetNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetSetDetailsByIssueID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{IssueID:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmSetSetupMasterDetail GetSetDetailsByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID)
        {
            vmSetSetupMasterDetail _setDetailobj = null;
            try
            {
                _setDetailobj = _objInternalIssue.GetSetDetailsByIssueID(pageNumber, pageSize, IsPaging, IssueID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _setDetailobj;
        }

        [Route("GetIssueDetailByIssueID/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{IssueID:int}")]
        // [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmIssueDetail> GetIssueDetailByIssueID(int? pageNumber, int? pageSize, int? IsPaging, int? IssueID)
        {
            List<vmIssueDetail> _IssueDetails = null;
            try
            {
                _IssueDetails = _objInternalIssue.GetIssueDetailByIssueID(pageNumber, pageSize, IsPaging, IssueID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _IssueDetails;
        }


        [Route("GetIssueDetailBySetNO/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{SetNo:int}")]
        //[ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public List<vmIssueDetail> GetIssueDetailBySetNO(int? pageNumber, int? pageSize, int? IsPaging, int? SetNo)
        {
            List<vmIssueDetail> _IssueDetails = null;
            try
            {
                _IssueDetails = _objInternalIssue.GetIssueDetailBySetNO(pageNumber, pageSize, IsPaging, SetNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _IssueDetails;

        }


        [HttpPost]
        public int SaveInternalIssue(object[] data)
        {
            int result = 0;
            if (data[0]!=null)
            {
                List<vmIssueDetail> _objIssueDetails = JsonConvert.DeserializeObject<List<vmIssueDetail>>(data[0].ToString());
                vmInternalIssue _objInternalIssueMaster = JsonConvert.DeserializeObject<vmInternalIssue>(data[1].ToString());
                
                try
                {
                    result = _objInternalIssue.SaveInternalIssue(_objIssueDetails, _objInternalIssueMaster);

                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return result;
        }

        //Corrected Validation
        //public int SaveInternalIssue(object[] data)
        //{
        //    int result = 0; List<vmIssueDetail> _objIssueDetails = null; vmInternalIssue _objInternalIssueMaster = null;
        //    if ((data[0] != null) && (data[1] != null))
        //    {
        //        _objIssueDetails = JsonConvert.DeserializeObject<List<vmIssueDetail>>(data[0].ToString());
        //        _objInternalIssueMaster = JsonConvert.DeserializeObject<vmInternalIssue>(data[1].ToString());

        //        try
        //        {
        //            result = _objInternalIssue.SaveInternalIssue(_objIssueDetails, _objInternalIssueMaster);

        //        }
        //        catch (Exception e)
        //        {
        //            e.ToString();
        //        }
        //    }
        //    return result;
        //}
    }
}
