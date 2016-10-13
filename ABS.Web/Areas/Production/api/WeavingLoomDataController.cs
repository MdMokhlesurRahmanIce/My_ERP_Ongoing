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
    [RoutePrefix("Production/api/WeavingLoomData")]
    public class WeavingLoomDataController : ApiController
    {
        private WeavingLoomDataMgt objLoomDataService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public WeavingLoomDataController()
        {
            objLoomDataService = new WeavingLoomDataMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetShifts(object[] data)
        {
            IEnumerable<vmShiftName> ShiftList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ShiftList = objCmnDDLService.GetShifts(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ShiftList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingLine(object[] data)
        {
            IEnumerable<vmWeavingLine> LineList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            //int recordsTotal = 0;
            try
            {
                LineList = objCmnDDLService.GetWeavingLine(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                //recordsTotal,
                LineList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDataToSetWeavingLoomDetail(object[] data)
        {
            IEnumerable<vmWeavingLoomDataMasterDetail> LoomDetailList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            //int recordsTotal = 0;
            try
            {
                LoomDetailList = objLoomDataService.GetDataToSetWeavingLoomDetail(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                //recordsTotal,
                LoomDetailList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetOperators(object[] data)
        {
            IEnumerable<vmOperator> OperatorList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                OperatorList = objCmnDDLService.GetOperators(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                OperatorList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLoadMachineStopCauses(object[] data)
        {
            IEnumerable<PrdBWSlist> CausesList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                CausesList = objCmnDDLService.GetLoadMachineStopCauses(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                CausesList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingLoomDataMaster(object[] data)
        {
            IEnumerable<vmWeavingLoomDataMasterDetail> ListLoomMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListLoomMaster = objLoomDataService.GetWeavingLoomDataMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListLoomMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingLoomDataMasterByID(object[] data)
        {
            vmWeavingLoomDataMasterDetail SinglLoomMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                SinglLoomMaster = objLoomDataService.GetWeavingLoomDataMasterByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                SinglLoomMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetWeavingLoomDataDetailByID(object[] data)
        {
            IEnumerable<vmWeavingLoomDataMasterDetail> ListLoomDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListLoomDetail = objLoomDataService.GetWeavingLoomDataDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListLoomDetail
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStopDetailByID(object[] data)
        {
            IEnumerable<vmWeavingLoomDataMasterDetail> ListStopDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListStopDetail = objLoomDataService.GetStopDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListStopDetail
            });
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateWeavingLoom(object[] data)
        {
            vmWeavingLoomDataMasterDetail itemMaster = JsonConvert.DeserializeObject<vmWeavingLoomDataMasterDetail>(data[0].ToString());
            List<vmWeavingLoomDataMasterDetail> MainDetail = JsonConvert.DeserializeObject<List<vmWeavingLoomDataMasterDetail>>(data[1].ToString());
            List<vmWeavingLoomDataMasterDetail> MachinStopDetail = JsonConvert.DeserializeObject<List<vmWeavingLoomDataMasterDetail>>(data[2].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[3].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && MainDetail.Count > 0)
                {
                    result = objLoomDataService.SaveUpdateWeavingLoom(itemMaster, MainDetail, MachinStopDetail, objcmnParam);
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
        public IHttpActionResult DeletePrdWeavingLoomMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objLoomDataService.DeletePrdWeavingLoomMasterDetail(objcmnParam);
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
