using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using ABS.Service.Production.Interfaces;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/WastageEntry")]
    public class WastageEntryController : ApiController
    {
        private WastageEntryMgt objWastageEntryService = null;
        private iProductionDDLMgt objCmnDDLService = null;
        private iSystemCommonDDL objSystemCommonDll = null;

        public WastageEntryController()
        {
            objWastageEntryService = new WastageEntryMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartmentByID(object[] data)
        {
            CmnOrganogram SingleDept = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                SingleDept = objCmnDDLService.GetDepartmentByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                SingleDept
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartmentDetails(object[] data)
        {
            objSystemCommonDll = new SystemCommonDDL();
            IEnumerable<vmBranch> ListDeptDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListDeptDetail = objSystemCommonDll.GetBranchDetails(objcmnParam.pageNumber, objcmnParam.pageSize, objcmnParam.IsPaging, objcmnParam.loggedCompany).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                ListDeptDetail
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetUnitSingle(object[] data)
        {
            vmProductionUOMDropDown ListUnit = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListUnit = objCmnDDLService.GetUnitSingle(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListUnit
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetArticle(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListArticle = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListArticle = objCmnDDLService.GetArticle(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListArticle
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWastageMaster(object[] data)
        {
            IEnumerable<vmWastageMasterDetail> WastageMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                WastageMaster = objWastageEntryService.GetWastageMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                WastageMaster
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWastageDetailByID(object[] data)
        {
            IEnumerable<vmWastageMasterDetail> WastageDetailByID = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                WastageDetailByID = objWastageEntryService.GetWastageDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                WastageDetailByID
            });

            //return objDOMaster.ToList();
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateWastage(object[] data)
        {
            vmWastageMasterDetail itemMaster = JsonConvert.DeserializeObject<vmWastageMasterDetail>(data[0].ToString());
            List<vmWastageMasterDetail> MainDetail = JsonConvert.DeserializeObject<List<vmWastageMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && MainDetail.Count > 0)
                {
                    result = objWastageEntryService.SaveUpdateWastage(itemMaster, MainDetail, objcmnParam);
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteWastageMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objWastageEntryService.DeleteWastageMasterDetail(objcmnParam);
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
