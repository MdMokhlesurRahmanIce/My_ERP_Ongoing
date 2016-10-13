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
    [RoutePrefix("Production/api/DyingPRDChemicalConsumption")]
    public class DyingPRDChemicalConsumptionController : ApiController
    {
        private iDyingPRDChemicalConsumptionMgt businessObject = null;
        public DyingPRDChemicalConsumptionController()
        {
            //
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteConsumption(object[] data)
        {
            Int64 result = 0;
            vmCmnParameters commonEntity = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                businessObject = new DyingPRDChemicalConsumptionMgt();
                result = businessObject.DeleteConsumption(commonEntity);
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
        public IHttpActionResult Save(object[] data)
        {
            int result = 0;
            PrdDyingConsumptionMaster master = JsonConvert.DeserializeObject<PrdDyingConsumptionMaster>(data[0].ToString());
            List<PrdDyingConsumptionDetail> DetailsList = JsonConvert.DeserializeObject<List<PrdDyingConsumptionDetail>>(data[1].ToString());
            #region Shared Entity
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[2].ToString());
            #endregion Shared Entity
            try
            {
                businessObject = new DyingPRDChemicalConsumptionMgt();
                result = businessObject.SaveChemicalConsumption(master, DetailsList, commonEntity);
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


        #region GetAll
        [Route("GetConsumptionByID/{companyID:int}/{loggedUser:int}/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ConsumptionID:int}"), HttpGet, BasicAuthorization, ResponseType(typeof(vmPrdDyingConsumptionMaster))]
        public vmPrdDyingConsumptionMaster GetConsumptionByID(int? companyID, int? loggedUser, int? pageNumber, int? pageSize, int? IsPaging, int? ConsumptionID)
        {
            vmPrdDyingConsumptionMaster finalList = new vmPrdDyingConsumptionMaster();
            try
            {
                businessObject = new DyingPRDChemicalConsumptionMgt();
                finalList = businessObject.GetConsumptionByID(companyID, loggedUser, pageNumber, pageSize, IsPaging, ConsumptionID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finalList;
        }
        [Route("GetChemicalConsumptionByID/{companyID:int}/{loggedUser:int}/{ConsumptionID:int}"), HttpGet, BasicAuthorization]
        public IEnumerable<object> GetChemicalConsumptionByID(int? companyID, int? loggedUser, int? ConsumptionID)
        {
            IEnumerable<object> finalList1 = null;

            List<vmPrdDyingConsumptionDetail> finalList = new List<vmPrdDyingConsumptionDetail>();
            try
            {
                PrdDyingConsumptionDetail ent = new PrdDyingConsumptionDetail();
                businessObject = new DyingPRDChemicalConsumptionMgt();
                finalList = businessObject.GetChemicalConsumptionByID(companyID, loggedUser, ConsumptionID);
                finalList1 = businessObject.GetChemicalConsumptionDetailsByID(companyID, loggedUser, ConsumptionID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return finalList1;
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetAllConsumption(object[] data)
        {
            int recordsTotal = 0;
            List<vmPrdDyingConsumptionMaster> finalList = new List<vmPrdDyingConsumptionMaster>();
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //List<vmPrdDyingConsumptionMaster> list = new List<vmPrdDyingConsumptionMaster>();
                businessObject = new DyingPRDChemicalConsumptionMgt();
                finalList = businessObject.GetAllConsumption(objcmnParam, out recordsTotal).ToList();
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
            //return finalList;
        }
        #endregion GetAll End

    }
}