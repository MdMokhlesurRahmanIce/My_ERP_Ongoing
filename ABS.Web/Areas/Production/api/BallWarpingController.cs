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
    [RoutePrefix("Production/api/BallWarping")]
    public class BallWarpingController : ApiController
    {
        private BallWarpingMgt objBallWarpingService = null;
        private iProductionDDLMgt objCmnDDLService = null;

        public BallWarpingController()
        {
            objBallWarpingService = new BallWarpingMgt();
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
        public IHttpActionResult GetBallWarpingMaster(object[] data)
        {
            IEnumerable<vmBallWarpingInformation> objvmBallWarping = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objvmBallWarping = objBallWarpingService.GetBallWarpingMaster(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objvmBallWarping
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetInformation(object[] data)
        {
            IEnumerable<vmBallWarpingInformation> model = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                model = objBallWarpingService.GetSetInformation(objcmnParam);
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
        public IHttpActionResult GetItemWiseStock(object[] data)
        {
            vmBallInfo ItemStockList = null;
            vmBallInfo ItemInfo=JsonConvert.DeserializeObject<vmBallInfo>(data[0].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
            try
            {
                ItemStockList = objBallWarpingService.GetItemWiseStock(ItemInfo, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                ItemStockList
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBallWarpingDetail(object[] data)
        {
            IEnumerable<vmBallWarpingInformation> objBallDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objBallDetail = objBallWarpingService.GetBallWarpingDetail(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objBallDetail
            });
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
        public IHttpActionResult GetBallWarpingMasterById(object[] data)
        {
            IEnumerable<vmBallWarpingInformation> BWMById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());            
            try
            {
                BWMById = objBallWarpingService.GetBallWarpingMasterById(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                BWMById
            });
            //return objDOMaster.ToList();
        }
        
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBallDetailByID(object[] data)
        {
            IEnumerable<vmBallWarpingInformation> ListBallDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListBallDetail = objBallWarpingService.GetBallDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListBallDetail
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetStopDetailByID(object[] data)
        {
            IEnumerable<vmBallMachineStopAndBrekage> ListStopDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListStopDetail = objBallWarpingService.GetStopDetailByID(objcmnParam, out recordsTotal).ToList();
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
            IEnumerable<vmBallMachineStopAndBrekage> ListBreakageDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListBreakageDetail = objBallWarpingService.GetBreakageDetailByID(objcmnParam, out recordsTotal).ToList();
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBallConsumptionByID(object[] data)
        {
            IEnumerable<vmBallConsumption> ConsumptionList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ConsumptionList = objBallWarpingService.GetBallConsumptionByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ConsumptionList
            });
            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateBallMRR(object[] data)
        {
            PrdBallMRRMaster itemMaster = JsonConvert.DeserializeObject<PrdBallMRRMaster>(data[0].ToString());
            List<vmBallWarpingInformation> itemDetails = JsonConvert.DeserializeObject<List<vmBallWarpingInformation>>(data[1].ToString());
            List<vmBallMachineStopAndBrekage> MachineStopDetail = JsonConvert.DeserializeObject<List<vmBallMachineStopAndBrekage>>(data[2].ToString());
            List<vmBallMachineStopAndBrekage> BreakageTypeDetail = JsonConvert.DeserializeObject<List<vmBallMachineStopAndBrekage>>(data[3].ToString());
            List<vmBallConsumption> ConsumptionInfo = JsonConvert.DeserializeObject<List<vmBallConsumption>>(data[4].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[5].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid && itemMaster != null && itemDetails.Count > 0)
                {
                    result = objBallWarpingService.SaveUpdateBallMRR(itemMaster, itemDetails, MachineStopDetail, BreakageTypeDetail, ConsumptionInfo, objcmnParam);
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
            // System.Web.HttpContext.Current.Session.Add("LCReferenceNo", result);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DeleteUpdateBallMrrMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                result = objBallWarpingService.DeleteUpdateBallMrrMasterDetail(objcmnParam);
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
            //return objDOMaster.ToList();
        }

    }
}
