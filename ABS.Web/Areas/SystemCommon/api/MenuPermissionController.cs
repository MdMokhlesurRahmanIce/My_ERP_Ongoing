using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
//using System.Web.Mvc;

namespace ABS.Web.SystemCommon.api
{

    [RoutePrefix("SystemCommon/api/MenuPermission")]
    public class MenuPermissionController :ApiController  
    {
        private iCmnMenuPermissionMgt objService = null;

        public MenuPermissionController()
        {
            this.objService = new CmnMenuPermissionMgt();
        }

        #region GetPermissionBy Param
        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetMenuPermissionByParam/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{pModuleID:int}/{pUserGroupID:int}/{pUserID:int}/{pOrgannogramID:int}")]
        [ResponseType(typeof(vmCmnMenuPermission))] 
        [HttpGet]
        public IEnumerable<vmCmnMenuPermission> GetMenuPermissionByParam(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? pModuleID, int? pUserGroupID, int? pUserID, int?  pOrgannogramID)
        {
           // IEnumerable<vmCmnMenuPermission> obj= null;
            List<vmCmnMenuPermission> finalList = new List<vmCmnMenuPermission>();
            
            try
            {
               if (pUserID <=Convert.ToInt32(0)) 
               {
                   finalList = objService.GetMenuPermissionByParams(companyID, loggedUser, pageNumber, pageSize, IsPaging, pModuleID, pUserGroupID, 0, 0).ToList();
               }
               else finalList = objService.GetMenuPermissionByParamsUser(companyID, loggedUser, pageNumber, pageSize, IsPaging, pModuleID, 0, pUserID, 0).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            var session = HttpContext.Current.Session;

            return finalList.OrderBy(x=>x.ModuleID).ThenByDescending(x=>x.Sequence).ThenBy(x=>x.MenuID).ThenBy(x=>x.MenuPermissionID);
        }
        #endregion GetPermissionBy Param

        #region Save
        [ResponseType(typeof(CmnMenuPermission))]
        [HttpPost]
        public HttpResponseMessage SaveMenuPermission(List<CmnMenuPermission> Listmodel)
        {
            int result = 0;
            try
            {
                result = objService.SaveMenuPermission(Listmodel);               
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        #endregion Save

    }
}
