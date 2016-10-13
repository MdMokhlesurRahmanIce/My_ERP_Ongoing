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
    [RoutePrefix("Production/api/ChemicalSetup")]//DyingChemicalPreparation
    public class ChemicalSetupController : ApiController
    {
        private iChemicalSetupMgt objChemicalService = null;
        public ChemicalSetupController()
        {
            objChemicalService = new ChemicalSetupMgt();
        }

        #region GetDetails ID
        // GET: GetMenuPermissionByParam/1/10/0
        [Route("GetDetailsByMasterID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DetailsID:int}")]
        [ResponseType(typeof(vmChemicalSetupDetail))]
        [HttpGet, BasicAuthorization]
        public IEnumerable<vmChemicalSetupDetail> GetDetailsByMasterID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DetailsID)
        {
            List<vmChemicalSetupDetail> detailsList = new List<vmChemicalSetupDetail>();
            try
            {
                detailsList = objChemicalService.GetDetailsByMasterID(companyID, loggedUser, pageNumber, pageSize, IsPaging, DetailsID).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return detailsList;
        }
        #endregion GetDetails ID

        [BasicAuthorization]
        public HttpResponseMessage Save(object[] data)
        {
            PrdBWSlist itemMaster = JsonConvert.DeserializeObject<PrdBWSlist>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    
                  //  result = objBWSService.SaveUpdateBWS(itemMaster);
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
        public IHttpActionResult SaveChemicalPrepartion(object[] data)
        {
            int result = 0;
            PrdDyingChemicalSetup master = JsonConvert.DeserializeObject<PrdDyingChemicalSetup>(data[0].ToString());
            List<PrdDyingChemicalSetupDetail> Details = JsonConvert.DeserializeObject<List<PrdDyingChemicalSetupDetail>>(data[1].ToString());

            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            var loggedCompnyID = commonEntity.loggedCompnyID;
            var loggedUserID = commonEntity.loggedUserID;
            var MenuID = commonEntity.currentMenuID;
            var loggedUserBranchID = commonEntity.loggedUserBranchID;
            var currentModuleID = commonEntity.currentModuleID;
            var loggedUserDepartmentID = commonEntity.loggedUserDepartmentID;

            var TransactionTypeID = commonEntity.currentTransactionTypeID;
            List<CmnMenu> menuList = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.MenuList.ToString());
            List<CmnMenu> ChildMenues = JsonConvert.DeserializeObject<List<CmnMenu>>(commonEntity.ChildMenues.ToString());

            #endregion Shared Entity
            try
            {
            
                result = objChemicalService.SaveChemicalPrepartion(master, Details, commonEntity);
               // result = 1;
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteChemicalPreparationMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objChemicalService.DeleteChemicalPreparationMasterDetail(objcmnParam);
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

        #region GetAll 
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetChemicalSetupList(object[] data)
        {
            int recordsTotal = 0;
            List<vmChemicalSetup> finalList = new List<vmChemicalSetup>();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                finalList = objChemicalService.GetChemicalSetupList(objcmnParam, out recordsTotal).ToList();
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
    }
}
