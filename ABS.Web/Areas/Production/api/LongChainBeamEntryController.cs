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

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/LongChainBeamEntry")]
    public class LongChainBeamEntryController : ApiController
    {
        private LongChainBeamMgt objLCBEntryService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public LongChainBeamEntryController()
        {
            objLCBEntryService = new LongChainBeamMgt();
            objCmnDDLService = new ProductionDDLMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetNo(object[] data)
        {
            IEnumerable<vmSetDetail> ListSet = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSet = objCmnDDLService.GetSetNo(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSet
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public vmPrdLCBMRRMasterDetail GetSetInformation(object[] data)
        {
            vmPrdLCBMRRMasterDetail model = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                model = objLCBEntryService.GetSetInformation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return model;
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
        public IHttpActionResult GetBeams(object[] data)
        {
            IEnumerable<vmBallInfo> BeamList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                BeamList = objCmnDDLService.GetBeams(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                BeamList
            });
            //return objDOMaster.ToList();
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
            //return objDOMaster.ToList();
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
            //return objDOMaster.ToList();
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
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLoadMachineBrekages(object[] data)
        {
            IEnumerable<PrdBWSlist> BrekagesList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                BrekagesList = objCmnDDLService.GetLoadMachineBrekages(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                BrekagesList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCBMRRMaster(object[] data)
        {
            IEnumerable<vmPrdLCBMRRMasterDetail> ListLCBMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListLCBMaster = objLCBEntryService.GetLCBMRRMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListLCBMaster
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCBDetailByID(object[] data)
        {
            IEnumerable<vmPrdLCBMRRMasterDetail> ListLCBDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListLCBDetail = objLCBEntryService.GetLCBDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListLCBDetail
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStopDetailByID(object[] data)
        {
            IEnumerable<vmPrdLCBMRRMasterDetail> ListStopDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListStopDetail = objLCBEntryService.GetStopDetailByID(objcmnParam, out recordsTotal).ToList();
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

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBreakageDetailByID(object[] data)
        {
            IEnumerable<vmPrdLCBMRRMasterDetail> ListBreakageDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListBreakageDetail = objLCBEntryService.GetBreakageDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListBreakageDetail
            });

            //return objDOMaster.ToList();
        }

        [BasicAuthorization]
        public HttpResponseMessage SaveUpdateLCB(object[] data)
        {
            vmPrdLCBMRRMasterDetail itemMaster = JsonConvert.DeserializeObject<vmPrdLCBMRRMasterDetail>(data[0].ToString());
            List<vmPrdLCBMRRMasterDetail> MainDetail = JsonConvert.DeserializeObject<List<vmPrdLCBMRRMasterDetail>>(data[1].ToString());
            List<vmPrdLCBMRRMasterDetail> MachinStopDetail = JsonConvert.DeserializeObject<List<vmPrdLCBMRRMasterDetail>>(data[2].ToString());
            List<vmPrdLCBMRRMasterDetail> BreakageTypeDetail = JsonConvert.DeserializeObject<List<vmPrdLCBMRRMasterDetail>>(data[3].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[4].ToString());

            PrdBallMRRMaster obj = new PrdBallMRRMaster();
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && MainDetail.Count > 0)
                {
                    result = objLCBEntryService.SaveUpdateLCB(itemMaster, MainDetail, MachinStopDetail, BreakageTypeDetail, objcmnParam);
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
        public IHttpActionResult DeletePrdLCBMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objLCBEntryService.DeletePrdLCBMasterDetail(objcmnParam);
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