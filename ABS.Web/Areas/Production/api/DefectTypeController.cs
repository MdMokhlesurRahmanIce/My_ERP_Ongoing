using ABS.Models;
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
    [RoutePrefix("Production/api/DefectType")]
    public class DefectTypeController : ApiController
    {
        private iDefectTypeMgt objDefectTypeService = null;

        public DefectTypeController()
        {
            objDefectTypeService = new DefectTypeMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDefectType(object[] data)
        {
            IEnumerable<PrdDefectType> TypeList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                TypeList = objDefectTypeService.GetDefectType(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                TypeList
            });

        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDefectTypeInfo(object[] data)
        {
            IEnumerable<PrdDefectList> objDefectTypeMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objDefectTypeMaster = objDefectTypeService.GetDefectTypeInfo(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objDefectTypeMaster
            });
            //return objvmSalLCDetail;
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateDefectType(object[] data)
        {
            PrdDefectList itemMaster = JsonConvert.DeserializeObject<PrdDefectList>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    result = objDefectTypeService.SaveUpdateDefectType(itemMaster, objcmnParam);
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
        public IHttpActionResult DeleteUpdateDefectList(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objDefectTypeService.DeleteUpdateDefectList(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                result
            });
        }
    }
}
