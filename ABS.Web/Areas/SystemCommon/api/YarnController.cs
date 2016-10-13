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
    [RoutePrefix("SystemCommon/api/Yarn")]
    public class YarnController : ApiController
    {
        iSystemCommonDDL objCmnItemGroupMgtEF = null;
        iYarnMgt objYarnMgt = null;
        iFinishGood _objFinishGood = null;

        [Route("GetItemGroups/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public List<vmItemGroup> GetItemGroups(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
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

        [Route("GetUnits/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID}")]
        [ResponseType(typeof(CmnUOM))]
        [HttpGet]
        public List<vmUnit> GetUnits(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            List<vmUnit> Units = null;

            try
            {
                Units = objCmnItemGroupMgtEF.GetAllUnit(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Units;
        }


        [Route("GetColors/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemColor))]
        [HttpGet]
        public List<vmColor> GetColors(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            List<vmColor> Colors = null;

            try
            {
                Colors = objCmnItemGroupMgtEF.GetAllColor(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Colors;
        }

        [Route("GetSizes/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemColor))]
        [HttpGet]
        public List<vmSize> GetSizes(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            List<vmSize> Sizes = null;

            try
            {
                Sizes = objCmnItemGroupMgtEF.GetAllSizes(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Sizes;
        }

        [Route("GetBrands/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemColor))]
        [HttpGet]
        public List<vmBrand> GetBrands(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            List<vmBrand> Brands = null;

            try
            {
                Brands = objCmnItemGroupMgtEF.GetBrands(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Brands;
        }


        [Route("GetModels/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnItemColor))]
        [HttpGet]
        public List<vmModel> GetModels(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            List<vmModel> Models = null;

            try
            {
                Models = objCmnItemGroupMgtEF.GetModels(pageNumber, pageSize, IsPaging, CompanyID).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Models;
        }

        [ResponseType(typeof(CmnItemMaster))]
        [HttpPost]
        public HttpResponseMessage SaveYarn(CmnItemMaster model)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            string result = string.Empty;
            try
            {
                result = objYarnMgt.SaveYarn(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }      
        
        [HttpPost]
        public IHttpActionResult GetAllYarn(object[] data)
        {
            _objFinishGood = new CmnFinishGoodMgt();
            int recordsTotal = 0;
            List<vmFinishGood> objFinishGoods = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());


            try
            {
                objFinishGoods = _objFinishGood.GetFinishGoods(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objFinishGoods
            });

        }

        [HttpPost]
        public HttpResponseMessage CheckItemCode(CmnItemMaster _obj)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            bool status = false;
            try
            {

                status = objYarnMgt.CheckItemCode(_obj.ItemName);

            }
            catch (Exception)
            {

                throw;
            }
            return Request.CreateResponse(HttpStatusCode.OK, status);
        }

        [ResponseType(typeof(CmnItemMaster))]
        [HttpPut]
        public HttpResponseMessage DeleteYarn(CmnItemMaster model)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            int result = 0;
            try
            {
                result = objYarnMgt.DeleteYarn(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetYarn/{id:int}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public VmItemMater GetYarn(int id)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            VmItemMater _objItemMaster = null;
            try
            {

                _objItemMaster = objYarnMgt.GetYarn(id);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objItemMaster;
        }




        [ResponseType(typeof(CmnItemMaster))]
        [HttpPut]
        public HttpResponseMessage UpdateYarn(CmnItemMaster model)
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objYarnMgt = new CmnYarnMgt();
            int result = 0;
            try
            {
                result = objYarnMgt.UpdateYarn(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
