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
    [RoutePrefix("Production/api/SizingOutputEntry")]
    public class SizingOutputEntryController : ApiController
    {
        private SizingOutputEntryMgt objSizingEntryService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public SizingOutputEntryController()
        {
            objSizingEntryService = new SizingOutputEntryMgt();
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
        public IHttpActionResult GetSetInformation(object[] data)
        {
            vmBallWarpingInformation model = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                model = objSizingEntryService.GetSetInformation(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                model
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMachine(object[] data)
        {
            IEnumerable<vmItemSetSetup> ListMachine = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
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
        public IHttpActionResult GetBeamQuality(object[] data)
        {
            IEnumerable<vmBeamQuality> BeamQualityList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                BeamQualityList = objCmnDDLService.GetBeamQuality(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                BeamQualityList
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
        public IHttpActionResult GetSizingMRRMaster(object[] data)
        {
            IEnumerable<vmPrdSizingMRRMaster> ListSizingMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSizingMaster = objSizingEntryService.GetSizingMRRMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSizingMaster
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSizingDetailByID(object[] data)
        {
            IEnumerable<vmPrdSizingMRRMaster> ListSizingDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSizingDetail = objSizingEntryService.GetSizingDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSizingDetail
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStopDetailByID(object[] data)
        {
            IEnumerable<vmPrdSizingMRRMaster> ListStopDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListStopDetail = objSizingEntryService.GetStopDetailByID(objcmnParam, out recordsTotal).ToList();
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
            IEnumerable<vmPrdSizingMRRMaster> ListBreakageDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListBreakageDetail = objSizingEntryService.GetBreakageDetailByID(objcmnParam, out recordsTotal).ToList();
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
        public HttpResponseMessage SaveUpdateSizing(object[] data)
        {
            vmPrdSizingMRRMaster itemMaster = JsonConvert.DeserializeObject<vmPrdSizingMRRMaster>(data[0].ToString());
            List<vmPrdSizingMRRMaster> MainDetail = JsonConvert.DeserializeObject<List<vmPrdSizingMRRMaster>>(data[1].ToString());
            List<vmPrdSizingMRRMaster> MachinStopDetail = JsonConvert.DeserializeObject<List<vmPrdSizingMRRMaster>>(data[2].ToString());
            List<vmPrdSizingMRRMaster> BreakageTypeDetail = JsonConvert.DeserializeObject<List<vmPrdSizingMRRMaster>>(data[3].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[4].ToString());

            PrdBallMRRMaster obj = new PrdBallMRRMaster();
            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && MainDetail.Count > 0)
                {
                    result = objSizingEntryService.SaveUpdateSizing(itemMaster, MainDetail, MachinStopDetail, BreakageTypeDetail, objcmnParam);
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
        public IHttpActionResult DeletePrdSizingMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objSizingEntryService.DeletePrdSizingMasterDetail(objcmnParam);
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
