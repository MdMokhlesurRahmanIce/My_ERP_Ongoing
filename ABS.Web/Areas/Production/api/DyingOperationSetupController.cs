using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using ABS.Web.Attributes;
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
    [RoutePrefix("Production/api/DyingOperationSetup")]
    public class DyingOperationSetupController : ApiController
    {
        private iDyingOperationSetupMgt objOperationSetup = null;
        public DyingOperationSetupController()
        {
            objOperationSetup = new DyingOperationSetupMgt();
        }

        #region GetAll 
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllOperationSetup(object[] data)
        {
            int recordsTotal = 0;
            List<vmPrdDyingOperationSetup> finalList = new List<vmPrdDyingOperationSetup>();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                finalList = objOperationSetup.GetAllOperationSetup(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                finalList
            });
        }
        #endregion GetAll End


        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveOperationSetup(object[] data)
        {
            int result = 0;
            List<PrdDyingOperationSetup> masterList = JsonConvert.DeserializeObject<List<PrdDyingOperationSetup>>(data[0].ToString());
            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());
            var loggedCompnyID = commonEntity.loggedCompnyID;
            var loggedUserID = commonEntity.loggedUserID;
            var MenuID = commonEntity.currentMenuID;
            var loggedUserBranchID = commonEntity.loggedUserBranchID;
            var currentModuleID = commonEntity.currentModuleID;

            var TransactionTypeID = commonEntity.currentTransactionTypeID;
            List<CmnMenu> menuList = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.MenuList.ToString());
            List<CmnMenu> ChildMenues = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.ChildMenues.ToString());

            #endregion Shared Entity
            try
            {
                result = objOperationSetup.SaveOperationSetup(masterList, commonEntity);
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
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteChemicalOperation(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objOperationSetup.DeleteChemicalOperation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Json(new
            {
                result
            });
        }

    }
}
