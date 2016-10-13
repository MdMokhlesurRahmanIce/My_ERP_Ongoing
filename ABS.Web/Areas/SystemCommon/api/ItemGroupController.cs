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
    [RoutePrefix("SystemCommon/api/ItemGroup")]
    public class ItemGroupController : ApiController
    {

        iSystemCommonDDL objCmnItemGroupMgtEF = null;
        iCmnItemGroupMgt objItemGroupMgtFF = null; 
        public ItemGroupController()
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objItemGroupMgtFF = new CmnItemGroupMgt();
        }


        [Route("GetItemTypes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemType))]
        [HttpGet]
        public List<vmItemType> GetItemTypes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyId)
        {
            List<vmItemType> itemTypes = null;
            try
            {
                itemTypes = objCmnItemGroupMgtEF.GetItemTypes(pageNumber, pageSize, IsPaging, CompanyId).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemTypes;
        }

        [HttpPost]
        public IHttpActionResult GetAllItemGroups(object[] data)
        {
            int itemGroupsTotal = 0;
            List<vmItemGroup> itemGroups = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                itemGroups = objItemGroupMgtFF.GetAllItemGroups(objcmnParam, out itemGroupsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal = itemGroupsTotal,
                itemGroups
            });

            //return itemGroups;
        }

        [Route("GetItemParentes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public List<vmItemGroup> GetItemParentes(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            List<vmItemGroup> itemParents = null;

            try
            {
                itemParents = objCmnItemGroupMgtEF.GetItemGroupsByTypeID(pageNumber, pageSize, IsPaging, ItemTypeID, CompanyId).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemParents;
        }

        [HttpPost]
        public List<AccACDetail> GetLedger(object[] data) 
        {
            List<AccACDetail> objLedger = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                int acc1Id = Convert.ToInt32(data[1]);
                objLedger = objCmnItemGroupMgtEF.GetLedger(objcmnParam, acc1Id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objLedger;
        }
         

        [Route("GetItemGroupParenteList/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public List<vmGroup> GetItemGroupParenteList(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            List<vmGroup> itemParents = null;
            try
            {
                itemParents = objCmnItemGroupMgtEF.GetItemGroupParenteList(pageNumber, pageSize, IsPaging, ItemTypeID, CompanyId).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return itemParents;

        }




        [ResponseType(typeof(CmnItemGroup))]
        [HttpPost]
        public HttpResponseMessage SaveItemGroup(CmnItemGroup model)
        {
            int result = 0;
            try
            {
                result = objItemGroupMgtFF.SaveItemGroup(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
        [ResponseType(typeof(CmnItemGroup))]
        [HttpPut]
        public HttpResponseMessage UpdateItemGroup(CmnItemGroup model)
        {
            int result = 0;
            try
            {
                result = objItemGroupMgtFF.UpdateItemGroup(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [ResponseType(typeof(CmnItemGroup))]
        [HttpPut]
        public HttpResponseMessage DeleteItemGroup(CmnItemGroup model)
        {
            int result = 0;
            try
            {
                result = objItemGroupMgtFF.DeleteItemGroup(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("GetItemGroupById/{GID:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public vmItemGroup GetItemGroupById(int? GID,int? CompanyID)
        {
            vmItemGroup objItemGroup = null;
            try
            {
                objItemGroup = objItemGroupMgtFF.GetItemGroupByID(GID, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objItemGroup;
        }

        [Route("GetMaxByParentID/{ParentID:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public int GetMaxByParentID(int? ParentID, int? CompanyID)
        {
            int maxValue = 0;
            try
            {
                maxValue = objItemGroupMgtFF.GetMaxByParentID(ParentID, CompanyID);

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return maxValue;
        }

         [Route("CheckItemGroupCode/{GroupCode:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public int CheckItemGroupCode(int? GroupCode)
        {
            int isExist = 0;
            try
            {
                isExist = objItemGroupMgtFF.CheckItemGroupCode(GroupCode);

            }
            catch (Exception e)
            {
                e.ToString();
            }

            return isExist;
        }

        
    }


}
