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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/DyingChemicalConsumption")]
    public class DyingChemicalConsumptionController : ApiController
    {
        private iDyingChemicalConsumptionMgt objChemicalConsumption = null;

        public DyingChemicalConsumptionController()
        {

        }
        #region GetAll 
        [Route("GetMachineSetupDetails/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{MasterID:int}/{DetailsID:int}"), HttpGet, BasicAuthorization]
        public dynamic GetMachineSetupDetails(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? MasterID, int? DetailsID)
        {
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                return service.GetMachineSetupDetails(companyID, loggedUser, pageNumber, pageSize, IsPaging, MasterID, DetailsID);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return 0;
        }

        [Route("GetOperationSetup/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemID:int}/{filterID:int}"), HttpGet, BasicAuthorization]
        public List<vmPrdDyingOperationSetup> GetOperationSetup(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ItemID, int? filterID)
        {
            List<vmPrdDyingOperationSetup> returnList = new List<vmPrdDyingOperationSetup>();
            try
            {

                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                    returnList = service.GetOperationSetup(companyID, loggedUser, pageNumber, pageSize, IsPaging, ItemID, filterID);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return returnList;
        }
        #endregion GetAll End
        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveMRRSet(object[] data)
        {
            int result = 0;
            List<vmProductionPrdSetSetupDDL> masterList = JsonConvert.DeserializeObject<List<vmProductionPrdSetSetupDDL>>(data[0].ToString());
            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());
            #endregion Shared Entity
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                    result= service.SaveMRRSet(masterList, commonEntity);
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
        public IHttpActionResult SaveMRR(object[] data)
        {
            int result = 0;
            vmPrdDyingMRRMaster master = JsonConvert.DeserializeObject<vmPrdDyingMRRMaster>(data[0].ToString());
            List<vmPrdDyingMRRDetail> DetailsList = JsonConvert.DeserializeObject<List<vmPrdDyingMRRDetail>>(data[1].ToString());
            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            #endregion Shared Entity
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                result = service.SaveMRR(master, DetailsList, commonEntity); 
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
        public IHttpActionResult DeleteChemicalProcessMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                result = service.DeleteChemicalProcessMasterDetail(objcmnParam);
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

        #region Get 
        [Route("GetProcessByID/{companyID:int}/{loggedUser:int}/{DyingMRRID:int}"), HttpGet, BasicAuthorization]
        public vmPrdDyingMRRMaster GetProcessByID(int? companyID, int? loggedUser, int? DyingMRRID)
        {
            vmPrdDyingMRRMaster retunList = new vmPrdDyingMRRMaster();
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                retunList = service.GetProcessByID(companyID, loggedUser, DyingMRRID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return retunList;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllProcess(object[] data)
        {
            int recordsTotal = 0;
            List<vmPrdDyingMRRMaster> ListChemProcess = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                ListChemProcess = service.GetAllProcess(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListChemProcess
            });

        }

        //[Route("GetAllProcess/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        //[ResponseType(typeof(vmPrdDyingMRRMaster))]
        //[HttpGet]
        //public List<vmPrdDyingMRRMaster> GetAllProcess(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging)
        //{
        //    List<vmPrdDyingMRRMaster> finalList = new List<vmPrdDyingMRRMaster>();
        //    try
        //    {
        //        DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
        //        finalList = service.GetAllProcess(companyID, loggedUser, pageNumber, pageSize, IsPaging);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //    }
        //    return finalList;
        //}

        [Route("GetProcessDetailsByID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{DyingMRRID:int}"), HttpGet, BasicAuthorization, ResponseType(typeof(vmPrdDyingMRRDetail))]
        public List<vmPrdDyingMRRDetail> GetProcessDetailsByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? DyingMRRID)
        {
            List<vmPrdDyingMRRDetail> finalList = new List<vmPrdDyingMRRDetail>();
            try
            {
                DyingChemicalConsumptionMgt service = new DyingChemicalConsumptionMgt();
                finalList = service.GetProcessDetailsByID(companyID, loggedUser, pageNumber, pageSize, IsPaging, DyingMRRID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finalList;
        }
        #endregion Get
    }
}