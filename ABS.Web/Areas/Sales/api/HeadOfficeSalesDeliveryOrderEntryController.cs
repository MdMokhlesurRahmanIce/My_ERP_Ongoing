using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Sales.Factories;
using ABS.Utility.Attributes;
using ABS.Web.Areas.Sales.Hubs;
using ABS.Web.Areas.SystemCommon.Hubs;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/HeadOfficeSalesDeliveryOrderEntry")]
    public class HeadOfficeSalesDeliveryOrderEntryController : ApiController
    {
        private HeadOfficeSalesDeliveryOrderMgt objDOService = null;

        public HeadOfficeSalesDeliveryOrderEntryController()
        {
            objDOService = new HeadOfficeSalesDeliveryOrderMgt();
        }

        #region Head Office Sales DeliveryOrderEntry
        // GET: GetCompany/1/10/0
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetCompany(object[] data)
        {
            IEnumerable<CmnCompany> objListCompany = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListCompany = objDOService.GetCompany(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objListCompany
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDOMaster(object[] data)
        {
            IEnumerable<vmSalHDOMaster> objDOMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objDOMaster = objDOService.GetHDOMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objDOMaster
            });

            //return objDOMaster.ToList();
        }

        // GET: GetLC/1/10/0
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLC(object[] data)
        {
            IEnumerable<SalLCMaster> objLC = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objLC = objDOService.GetLC(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objLC
            });
        }

        // GET: GetDOMasterById/1
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDOMasterById(object[] data)
        {
            vmSalHDOMaster objDoMasterById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objDoMasterById = objDOService.GetDOMasterById(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objDoMasterById
            });
        }

        // GET: GetBuyer/1/10/0
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetBuyer(object[] data)
        {
            IEnumerable<vmBuyerHDO> BuyerList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                BuyerList = objDOService.GetBuyer(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                BuyerList
            });
            //return objListBuyer.ToList();
        }

        // GET: GetLCByBuyerId/1
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCByBuyerId(object[] data)
        {
            IEnumerable<SalLCMaster> objLCBYBId = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objLCBYBId = objDOService.GetLCByBuyerId(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objLCBYBId
            });
        }


        // GET: GetLCMasterById/1
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCMasterById(object[] data)
        {
            IEnumerable<vmSalLCDetail> objLCMasterById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objLCMasterById = objDOService.GetLCMasterById(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objLCMasterById
            });
            //return objLCMasterById;
        }
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLCDetailByID(object[] data)
        {
            IEnumerable<vmSalLCDetail> objLCDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objLCDetail = objDOService.GetLCDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objLCDetail
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPIDetailsById(object[] data)
        {
            IEnumerable<vmPIDetail> objPIDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objPIDetail = objDOService.GetPIDetailsById(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objPIDetail
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetProductRevised(object[] data)
        {
            IEnumerable<vmPIDetail> objHDDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objHDDetail = objDOService.GetProductRevised(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                objHDDetail
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetProducts(object[] data)
        {
            IEnumerable<CmnItemMaster> ListProduct = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListProduct = objDOService.GetProducts(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListProduct
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public async Task<IHttpActionResult> SaveUpdateHeadOfficeSalesDeliveryOrder(object[] data)
        {
            vmSalHDOMaster model = JsonConvert.DeserializeObject<vmSalHDOMaster>(data[0].ToString());
            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[1].ToString());
            string result = string.Empty;
   
            //int recordsTotal = 0;
            try
            {
                result = await objDOService.SaveUpdateHeadOfficeSalesDeliveryOrder(model, commonEntity);
                NotificationHubs.BroadcastData(new NotificationEntity()); // for sending notification
            }
            catch (Exception e)
            {
                e.ToString();
                result = "";
            }

            return Json(new
            {
                //recordsTotal,
                result
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult RevisedHeadOfficeSalesDeliveryOrder(object[] data)
        {
            vmSalHDOMaster model = JsonConvert.DeserializeObject<vmSalHDOMaster>(data[0].ToString());
            List<vmPIDetail> ListModel = JsonConvert.DeserializeObject<List<vmPIDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            string result = string.Empty;
            //int recordsTotal = 0;
            try
            {
                result = objDOService.RevisedHeadOfficeSalesDeliveryOrder(model, ListModel, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
                result = string.Empty;
            }

            return Json(new
            {
                //recordsTotal,
                result
            });
        }


        #region Approval
        [HttpPost, BasicAuthorization]
        public IHttpActionResult NotificationApproval(object[] data)
        {
            vmCmnParameters model = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = objDOService.ApproveModel(model);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                result = string.Empty;
            }
            return Json(new
            {
                result
            });
        }

        #endregion Approval


        //// POST RevisedHeadOfficeSalesDeliveryOrder
        //[ResponseType(typeof(vmSalHDOMaster))]
        //[HttpPost]
        //public HttpResponseMessage RevisedHeadOfficeSalesDeliveryOrder(vmSalHDOMaster model)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            result = objDOService.RevisedHeadOfficeSalesDeliveryOrder(model);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        result = string.Empty;
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
        #endregion Head Office Sales DeliveryOrderEntry
    }
}
