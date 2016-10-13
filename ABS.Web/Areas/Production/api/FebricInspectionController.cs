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
    [RoutePrefix("Production/api/FebricInspection")]
    public class FebricInspectionController : ApiController
    {
        private iProductionDDLMgt objCmnDDLService = null;
        private iFebricInspection _objFebricInspection = null;

        public FebricInspectionController()
        {
            _objFebricInspection = new FebricInspectionMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStyleNo(object[] data)
        {
            List<vmStyle> Styles = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                Styles = objCmnDDLService.GetStyleNoes(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                Styles
            });
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
        public IHttpActionResult GetMachine(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListMachine = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListMachine = objCmnDDLService.GetDetailsMachine(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListMachine
            });
            //return objDOMaster.ToList();
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPlates(object[] data)
        {
            IEnumerable<CmnOrganogram> ListDept = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                ListDept = objCmnDDLService.GetDepartment(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                ListDept
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFabricInspectionByStyle(object[] data)
        {
            vmFabricInspection _fabricInspection = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                _fabricInspection = _objFebricInspection.Get_FabricInspectionByStyle(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                _fabricInspection
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDefectPoints(object[] data)
        {
            List<vmDefectPoint> _objDefectPoints = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                _objDefectPoints = objCmnDDLService.GetDefectPoints(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                _objDefectPoints
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetUnit(object[] data)
        {
            IEnumerable<vmProductionUOMDropDown> ListUOM = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListUOM = objCmnDDLService.GetUnit(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListUOM
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetFebricInspectionByInspectionID(object[] data)
        {
            vmFabricInspectionMaster objFabricInspection = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objFabricInspection = _objFebricInspection.GetFebricInspectionByInspectionID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objFabricInspection);
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage GetFebricInspectionDetailsID(object[] data)
        {
            List<vmFinishingInspactionDetail> objFabricInspectionDetails = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objFabricInspectionDetails = _objFebricInspection.GetFebricInspectionDetailsID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Request.CreateResponse(HttpStatusCode.OK, objFabricInspectionDetails);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult FabricInspectionDetails(object[] data)
        {
            //_objFebricInspection = new FebricInspectionMgt();
            int recordsTotal = 0;
            List<vmFabricInspectionMaster> _fabricInspectonMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                _fabricInspectonMaster = _objFebricInspection.FabricInspectionDetails(objcmnParam, out recordsTotal).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                fabricInspectionMasgter = _fabricInspectonMaster
            });

        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateFebricInspection(object[] data)
        {
            PrdFinishingInspactionMaster _objFinishingInspactionMaster = JsonConvert.DeserializeObject<PrdFinishingInspactionMaster>(data[0].ToString());
            List<vmFinishingInspactionDetail> _objInspactionDetailsList = JsonConvert.DeserializeObject<List<vmFinishingInspactionDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = string.Empty;
            try
            {
                result = _objFebricInspection.SaveUpdateFebricInspection(_objFinishingInspactionMaster, _objInspactionDetailsList, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }  

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateFabricInspectionMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = _objFebricInspection.DeleteUpdateFabricInspectionMasterDetail(objcmnParam);
                //}
            }
            catch (Exception e)
            {
                e.ToString();
                result = "0";
            }
            return Json(new
            {
                result
            });
        }
    }
}
