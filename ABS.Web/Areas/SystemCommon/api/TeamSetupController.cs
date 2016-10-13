using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Web.Areas.SystemCommon.Hubs;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/TeamSetup")]
    public class TeamSetupController : ApiController
    {
        private iTeamSetupMgt objService = null;
        public TeamSetupController()
        {
            this.objService = new TeamSetupMgt();
        }
            [HttpPost]
        public IHttpActionResult DeleteTeam(object[] data)
        {
            int result = 0;
            vCmnUserTeam master = JsonConvert.DeserializeObject<vCmnUserTeam>(data[0].ToString());
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            try
            {
                result = objService.DeleteTeam(master, commonEntity);
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return Json(new
            {
                result
            });
            //return _finishGoodes;
        }
        [HttpPost]
        public IHttpActionResult SaveTeam(object[] data)
        {
            int result = 0;
            vCmnUserTeam master = JsonConvert.DeserializeObject<vCmnUserTeam>(data[0].ToString());
            List<vCmnuserTeamDetail> Details = JsonConvert.DeserializeObject<List<vCmnuserTeamDetail>>(data[1].ToString());
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            try
            {
                result = objService.SaveTeam(master, Details, commonEntity);
            }
            catch (Exception e)
            {
                e.ToString();
                result = 0;
            }
            return Json(new
            {
                result
            });
            //return _finishGoodes;
        }

        [Route("GetTeam/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(vCmnUserTeam))]
        [HttpGet]
        public IEnumerable<vCmnUserTeam> GetTeam(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vCmnUserTeam> finalList = new List<vCmnUserTeam>();
            try
            {
                finalList = objService.GetTeam(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finalList;
        }

        #region GetDetails ID
        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetDetailsByMasterID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DetailsID:int}")]
        [ResponseType(typeof(vCmnuserTeamDetail))]
        [HttpGet]
        public IEnumerable<vCmnuserTeamDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            List<vCmnuserTeamDetail> detailsList = new List<vCmnuserTeamDetail>();
            try
            {
                detailsList = objService.GetDetailsByMasterID(companyID, loggedUser, pageNumber, pageSize, IsPaging, DetailsID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return detailsList;
        }
        #endregion GetDetails ID
    }
}
