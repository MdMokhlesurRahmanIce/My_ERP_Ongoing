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
    [RoutePrefix("SystemCommon/api/RawMaterial")]
    public class RawMaterialController : ApiController
    {
        iSystemCommonDDL objCmnItemGroupMgtEF = null;
        iCmnRawMaterial objCmnRawMaterial = null;
        public RawMaterialController()
        {
            objCmnItemGroupMgtEF = new SystemCommonDDL();
            objCmnRawMaterial = new CmnRawMaterialMgt();
        }

        [Route("GetItemGroups/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{ItemTypeID:int}/{CompanyId:int}")]
        [ResponseType(typeof(CmnItemGroup))]
        [HttpGet]
        public List<vmItemGroup> GetItemGroups(int? pageNumber, int? pageSize, int? IsPaging, int? ItemTypeID, int? CompanyId)
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

        [Route("GetUnits/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(CmnUOM))]
        [HttpGet]
        public List<vmUnit> GetUnits(int? pageNumber, int? pageSize, int? IsPaging, int? CompanyID)
        {
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

        //[ResponseType(typeof(CmnItemMaster))]
        //[HttpPost]
        //public HttpResponseMessage SaveRowMaterial(CmnItemMaster model)
        //{
        //    string result = "";
        //    try
        //    {
        //        result = objCmnRawMaterial.SaveRowMaterial(model);
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = "";
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, result);

        //}

        [HttpPost]
        public HttpResponseMessage SaveRowMaterial(object[] data) 
        { 
            string result = "";
            try
            {
                CmnItemMaster itemMaster = JsonConvert.DeserializeObject<CmnItemMaster>(data[0].ToString());
                int accDetailID = Convert.ToInt16(data[1]);
                int ACTypeID = Convert.ToInt16(data[2]); 

                if (ModelState.IsValid && itemMaster != null && accDetailID != null)
                {
                    result = objCmnRawMaterial.SaveRowMaterial(itemMaster, accDetailID, ACTypeID);
                  
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        public IHttpActionResult GetAllRowMaterial(object[] data)
        {

            int recordsTotal = 0;
            List<vmFinishGood> objRawMaterials = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objRawMaterials = objCmnRawMaterial.GetAllRowMaterial(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objRawMaterials
            });

        }

        [ResponseType(typeof(CmnItemMaster))]
        [HttpPut]
        public HttpResponseMessage DeleteRawMaterial(CmnItemMaster model)
        {
            int result = 0;
            try
            {
                result = objCmnRawMaterial.DeleteRawMaterial(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("GetRawMaterial/{id:int}/{typeId:int}/{companyID}")]
        [ResponseType(typeof(CmnItemMaster))]
        [HttpGet]
        public vmFinishGood GetRawMaterial(int id, int typeId, int companyID)
        {
            vmFinishGood _objFinishGood = null;
            try
            {

                _objFinishGood = objCmnRawMaterial.GetRawMaterial(id, typeId, companyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return _objFinishGood;
        }        
        [ResponseType(typeof(CmnItemMaster))]
        [HttpPut]
        public HttpResponseMessage UpdateRawMaterial(CmnItemMaster model)
        {
            int result = 0;
            try
            {
                result = objCmnRawMaterial.UpdateRawMaterial(model);
            }
            catch (Exception e)
            {
                e.ToString();
                result = -0;
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);

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
    }
}
