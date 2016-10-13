using ABS.Models;
using ABS.Models.ViewModel.Inventory;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Inventory.Factories;
using ABS.Service.Inventory.Interfaces;
using ABS.Service.MenuMgt;
using ABS.Service.SystemCommon.Factories;
using ABS.Service.SystemCommon.Interfaces;
using ABS.Utility;
using ABS.Web.Areas.Inventory.Hubs;
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

namespace ABS.Web.Areas.Inventory.api
{
    [RoutePrefix("Inventory/api/MRRAccount")]
    public class MRRAccountController : ApiController
    {
        private iMRRAccountMgt objMRR = null;
        private iSystemCommonDDL objSystemCommonDDL = null;

        public MRRAccountController()
        {
            objMRR = new MRRAccountMgt();
            objSystemCommonDDL = new SystemCommonDDL();
        }

        [Route("GetGRRNo/{pageNumber:int}/{pageSize:int}/{IsPaging:int}")]
        [ResponseType(typeof(InvGrrMaster))]
        [HttpGet]
        public List<InvGrrMaster> GetGRRNo(int? pageNumber, int? pageSize, int? IsPaging)
        {
            List<InvGrrMaster> lstInvGrrMaster = null;
            try
            {
                lstInvGrrMaster = objMRR.GetGRRNo(pageNumber, pageSize, IsPaging);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return lstInvGrrMaster;
        }

        [Route("GetSuppliers/{pageNumber:int}/{pageSize:int}/{IsPaging:int}/{CompanyID:int}")]
        [ResponseType(typeof(vmBuyer))]
        [HttpGet]
        public List<vmBuyer> GetSuppliers(int pageNumber, int pageSize, int IsPaging, int? CompanyID)
        {
            List<vmBuyer> objListSuppliers = null;
            try
            {
                objListSuppliers = objSystemCommonDDL.GetSuppliers(pageNumber, pageSize, IsPaging, CompanyID);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objListSuppliers;
        }



        [HttpPost]
        public IHttpActionResult GetMrrType(object[] data)
        {
            List<CmnCombo> lstMrrType = null;
            string mrrType = data[0].ToString();

            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[1].ToString());
                lstMrrType = objMRR.GetMrrType(mrrType, objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                lstMrrType
            });
        }

        [HttpPost]
        public IHttpActionResult GetSuppliers(object[] data)
        {
            List<vmGrr> lstSuppliers = null;

            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstSuppliers = objMRR.GetSuppliers(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                lstSuppliers
            });
        }

        [HttpPost]
        public IHttpActionResult GetIssueNo(object[] data)
        {
            List<InvIssueMaster> lstIssue = null;

            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                lstIssue = objMRR.GetIssueNo(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                lstIssue
            });
        }


        [HttpPost]
        public IHttpActionResult GetMasterInfoByGrrNo(object[] data)
        {
            IEnumerable<vmGrr> lstMasterInfoByGrrNo = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 grrID = Convert.ToInt64(data[1]);
                lstMasterInfoByGrrNo = objMRR.GetMasterInfoByGrrNo(objcmnParam, grrID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstMasterInfoByGrrNo
            });
        }

        [HttpPost]
        public IHttpActionResult GetDetailInfoByGrrNo(object[] data)
        {
            IEnumerable<vmQC> lstDetailInfoByGrrNo = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 grrID = Convert.ToInt64(data[1]);
                lstDetailInfoByGrrNo = objMRR.GetDetailInfoByGrrNo(objcmnParam, grrID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstDetailInfoByGrrNo
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMrrMasterListAccount(object[] data)
        {
            IEnumerable<vmGrr> lstMrrMaster = null;

            int recordsTotal = 0;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());

                lstMrrMaster = objMRR.GetMrrMasterListAccount(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstMrrMaster
            });
        }


        [HttpPost]
        public IHttpActionResult GetQCListByGrrNo(object[] data)
        {
            IEnumerable<vmQC> lstQCByGrrNo = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 grrID = Convert.ToInt64(data[1]);
                lstQCByGrrNo = objMRR.GetQCListByGrrNo(objcmnParam, grrID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstQCByGrrNo
            });
        }

        [HttpPost]
        public IHttpActionResult GetQCList(object[] data)
        {
            IEnumerable<InvMrrQcMaster> lstQCByGrrNo = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                lstQCByGrrNo = objMRR.GetQCList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstQCByGrrNo
            });
        }

        [HttpPost]
        public IEnumerable<InvMrrMaster> ChkDuplicateNo(object[] data)
        {

            IEnumerable<InvMrrMaster> objNo = null;
            try
            {
                vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
                string MNo = data[1].ToString();
                objNo = objMRR.ChkDuplicateNo(objcmnParam, MNo);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return objNo;
        }


        [HttpPost]
        public IHttpActionResult GetWherehouseList(object[] data)
        {
            object[] lstWherehouse = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                lstWherehouse = objMRR.GetWherehouseList(objcmnParam, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstWherehouse
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDetailInfoByQCID(object[] data)
        {
            IEnumerable<vmQC> lstQC = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 QCID = Convert.ToInt64(data[1]);
                lstQC = objMRR.GetDetailInfoByQCID(objcmnParam, QCID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstQC
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetDetailInfoByIssueID(object[] data)
        {
            IEnumerable<vmQC> lstIssue = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 IssueID = Convert.ToInt64(data[1]);
                lstIssue = objMRR.GetDetailInfoByIssueID(objcmnParam, IssueID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstIssue
            });
        }


        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetMrrDetailsListByMrrID(object[] data)
        {
            IEnumerable<vmQC> lstDetailInfoByMrrID = null;

            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                Int64 mrrID = Convert.ToInt64(data[1]);
                lstDetailInfoByMrrID = objMRR.GetMrrDetailsListByMrrID(objcmnParam, mrrID, out recordsTotal);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                lstDetailInfoByMrrID
            });
        }


        [HttpPost, BasicAuthorization]
        public HttpResponseMessage SaveUpdateMrrMasterNdetails(object[] data)
        {
            InvMrrMaster mrrMaster = JsonConvert.DeserializeObject<InvMrrMaster>(data[0].ToString());
            List<InvMrrDetail> mrrDetails = JsonConvert.DeserializeObject<List<InvMrrDetail>>(data[1].ToString());

            int menuID = Convert.ToInt16(data[2]);

            UserCommonEntity commonEntity = JsonConvert.DeserializeObject<UserCommonEntity>(data[3].ToString());

            string result = "";
            try
            {
                if (ModelState.IsValid && mrrMaster != null && mrrMaster.MrrDate.ToString() != "" && mrrDetails.Count > 0 && menuID != 0)
                {
                    result = objMRR.SaveUpdateMrrMasterNdetails(mrrMaster, mrrDetails, menuID, commonEntity).Result;
                    NotificationHubs.BroadcastData(new NotificationEntity());
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
        public HttpResponseMessage SaveLot(object[] data)
        {
            CmnLot objCmnLot = JsonConvert.DeserializeObject<CmnLot>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && objCmnLot != null && objCmnLot.LotNo.ToString() != "")
                {
                    result = objMRR.SaveLot(objCmnLot);
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
        public HttpResponseMessage SaveBatch(object[] data)
        {
            CmnBatch objCmnBatch = JsonConvert.DeserializeObject<CmnBatch>(data[0].ToString());
            string result = "";
            try
            {
                if (ModelState.IsValid && objCmnBatch != null && objCmnBatch.BatchNo.ToString() != "")
                {
                    result = objMRR.SaveBatch(objCmnBatch);
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

    }
}
