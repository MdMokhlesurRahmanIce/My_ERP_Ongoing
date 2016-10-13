using ABS.Models;
using ABS.Models.ViewModel.Sales;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Sales.Factories;
using ABS.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;



namespace ABS.Web.Areas.Sales.api
{
    [RoutePrefix("Sales/api/FactorySalesDeliveryOrderEntry")]
    public class FactorySalesDeliveryOrderEntryController : ApiController
    {
        private FactorySalesDeliveryOrderMgt objDOService = null;

        public FactorySalesDeliveryOrderEntryController()
        {
            objDOService = new FactorySalesDeliveryOrderMgt();
        }

        #region Factory Sales DeliveryOrderEntry

        //GetCompany
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

        //GetDepartment
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDepartment(object[] data)
        {
            IEnumerable<CmnOrganogram> objListDept = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListDept = objDOService.GetDepartment(objcmnParam).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objListDept
            });
        }

        //GetLotByItemId
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetLotByItemId(object[] data)
        {
            IEnumerable<CmnLot> objListLot = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objListLot = objDOService.GetLotByItemId(objcmnParam).ToList();

            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objListLot
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
                objDOMaster = objDOService.GetDOMaster(objcmnParam, out recordsTotal).ToList();
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

        // GET: GetDODetailByID/1
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDODetailByID(object[] data)
        {
            IEnumerable<vmHDODetail> objDODetailById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objDODetailById = objDOService.GetDODetailByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objDODetailById
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFDOMaster(object[] data)
        {
            IEnumerable<SalFDOMaster> objFDOMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objFDOMaster = objDOService.GetFDOMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objFDOMaster
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetFDODetail(object[] data)
        {
            IEnumerable<SalFDODetail> objFDODetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                objFDODetail = objDOService.GetFDODetail(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                objFDODetail
            });

            //return objDOMaster.ToList();
        }

        // POST SaveUpdateFactoryDeliverOrderDetail
        [HttpPost, BasicAuthorization]
        public IHttpActionResult SaveUpdateFactorySalesDeliveryOrderMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmSalFDODetail Master = JsonConvert.DeserializeObject<vmSalFDODetail>(data[0].ToString());
            List<vmSalFDODetail> Detail = JsonConvert.DeserializeObject<List<vmSalFDODetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objDOService.SaveUpdateFactorySalesDeliveryOrderMasterDetail(Master, Detail, objcmnParam);
                //}
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
            //return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        #endregion Factory Sales DeliveryOrderEntry
    }
}
