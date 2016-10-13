using ABS.Models;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.SystemCommon.api
{
    [RoutePrefix("SystemCommon/api/ModulePermission")]
    public class ModulePermissionController : ApiController
    {
        private iCmnModulePermissionMgt objService = null;
        public ModulePermissionController()
        {
            this.objService = new CmnModulePermissionMgt();
        }


        #region Get Data 
        // GET: GetModules/0/10/0
        [Route("GetAllModulePermission/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
       // [ResponseType(typeof(CmnModulePermission))]
        [HttpGet]
        public List<vmModulePermission> GetAllModulePermission(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<vmModulePermission> objlist = new List<vmModulePermission>();
            try
            {
                objlist = objService.GetAllModulePermission(companyID, loggedUser, pageNumber, pageSize, IsPaging).ToList();
             //   objlist = objListModules.ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objlist;
        }
        #endregion Get Data 
        #region Create
        // POST SaveUser
        [ResponseType(typeof(vmUser))]
        [HttpPost]
        public HttpResponseMessage SaveModulePermission(CmnModulePermission model)
        {
            int result = 0;
            try
            {
                if (model != null)
                    result = objService.SaveModulePermission(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion

        #region Delete Data 
      
        [Route("DeletePermission/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage DeletePermission(int id)
        {
            int result = 0;
            try
            {
                result = objService.DeletePermission(id);
                //result = objItemSizeService.DeleteItemSize(id);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion Delete Data


    }
}
