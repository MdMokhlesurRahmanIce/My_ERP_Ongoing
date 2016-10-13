using ABS.Models;
using ABS.Models.ViewModel.Production;
using ABS.Models.ViewModel.SystemCommon;
using ABS.Service.Production.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using ABS.Web.Attributes;
using ABS.Web.Areas.SystemCommon.Hubs;
using System.Threading.Tasks;

namespace ABS.Web.Areas.Production.api
{
    [RoutePrefix("Production/api/SetSetup")]
    public class SetSetupController : ApiController
    {
        private SetSetupMgt objSetService = null;

        public SetSetupController()
        {
            objSetService = new SetSetupMgt();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetPI(object[] data)
        {
            IEnumerable<vmPISetSetup> PIList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                PIList = objSetService.GetPI(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                PIList
            });
            //return PIList.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetItem(object[] data)
        {
            IEnumerable<vmItemSetSetup> ItemList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ItemList = objSetService.GetItem(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ItemList
            });
            //return PIList.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSelectedItemData(object[] data)
        {
            vmSelectedItemDataSetSetup objItemDataById = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objItemDataById = objSetService.GetSelectedItemData(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objItemDataById
            });
        }

        // GET: GetSupplier/1/10/0
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSupplier(object[] data)
        {
            IEnumerable<vmBuyer> SupplierList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                SupplierList = objSetService.GetSupplier(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                SupplierList
            });
            //return objListBuyer.ToList();
        }

        // GET: GetSupplier/1/10/0
        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetRefSet(object[] data)
        {
            IEnumerable<vmSetDetail> RefSetList = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                RefSetList = objSetService.GetRefSet(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                RefSetList
            });
            //return objListBuyer.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetWiseDate(object[] data)
        {
            vmSetDetail objSetWiseDate = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSetWiseDate = objSetService.GetSetWiseDate(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objSetWiseDate
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetSetupMaster(object[] data)
        {
            IEnumerable<vmSetMaster> ListSetMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSetMaster = objSetService.GetSetSetupMaster(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                recordsTotal,
                ListSetMaster
            });

            //return objDOMaster.ToList();
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetMasterByID(object[] data)
        {
            vmSetSetupMasterDetail objSingleSetMaster = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                objSingleSetMaster = objSetService.GetSetMasterByID(objcmnParam);
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return Json(new
            {
                objSingleSetMaster
            });
        }

        [HttpPost, BasicAuthorization]
        public IHttpActionResult GetSetDetailByID(object[] data)
        {
            IEnumerable<vmSetSetupMasterDetail> ListSetDetail = null;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            int recordsTotal = 0;
            try
            {
                ListSetDetail = objSetService.GetSetSetupDetailByID(objcmnParam, out recordsTotal).ToList();
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return Json(new
            {
                recordsTotal,
                ListSetDetail
            });
            //return objDOMaster.ToList();
        }


        [HttpPost, BasicAuthorization]
        public async Task<IHttpActionResult> SaveUpdateSetSetupMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmSetSetupMasterDetail ModelMaster = JsonConvert.DeserializeObject<vmSetSetupMasterDetail>(data[0].ToString());
            List<vmSetSetupMasterDetail> ModelDetail = JsonConvert.DeserializeObject<List<vmSetSetupMasterDetail>>(data[1].ToString());
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[2].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result =await objSetService.SaveUpdateSetSetupMasterDetail(ModelMaster, ModelDetail, objcmnParam);
                NotificationHubs.BroadcastData(new NotificationEntity());
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

        [HttpPost, BasicAuthorization]
        public IHttpActionResult DelUpdateSetMasterDetail(object[] data)
        {
            string result = string.Empty;
            vmCmnParameters objcmnParam = JsonConvert.DeserializeObject<vmCmnParameters>(data[0].ToString());
            try
            {
                //if (ModelState.IsValid)
                //{
                result = objSetService.DelUpdateSetMasterDetail(objcmnParam);
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
